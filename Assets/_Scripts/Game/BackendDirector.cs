using BackEnd;
using BackEnd.Tcp;
using ChessCrush.OperationResultCode;
using ChessCrush.UI;
using LitJson;
using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace ChessCrush.Game
{
    public class BackendDirector : SubDirector
    {
        private string roomToken;
        public List<SessionId> SessionIdList { get; private set; }
        public bool IsHost { get; private set; }
        private bool gameServerJoined;
        private bool roomJoined;
        private bool inGameReady;

        public bool MatchMakingServerJoined { get; private set; }
        public bool OppositeDisconnected { get; private set; }
        private int oppositeDisconnectedCount = 0;

        public Action matchMakingSuccessCallback;

        private ChessGameDirector chessGameDirector;

        private void Awake()
        {
            Backend.Initialize(() =>
            {
                if (Backend.IsInitialized)
                {
                    SetHandler();
                    gameObject.UpdateAsObservable().Subscribe(_ => Backend.Match.poll()).AddTo(gameObject);
                }
                else
                    MessageBoxUI.UseWithComponent("Failed to connect to server");
            });
        }

        private void OnApplicationQuit()
        {
            if (IsReconnect)
            {
                MatchEnd(false);
                LeaveGameServer();
            }
        }

        private void SetHandler()
        {
            string gameRoomToken = "";

            Backend.Match.OnJoinMatchMakingServer += args =>
            {
                if (args.ErrInfo != ErrorInfo.Success)
                    MessageBoxUI.UseWithComponent("Failed to join match making server");
            };

            Backend.Match.OnMatchMakingResponse += args =>
            {
                switch (args.ErrInfo)
                {
                    case ErrorCode.Success:
                        if (!gameServerJoined)
                        {
                            matchMakingSuccessCallback();
                            JoinGameServer(args.Address, args.Port, false);
                            gameServerJoined = true;
                            gameRoomToken = args.Token;
                        }
                        break;
                    case ErrorCode.Match_InvalidMatchType:
                    case ErrorCode.Match_InvalidModeType:
                    case ErrorCode.InvalidOperation:
                        MessageBoxUI.UseWithComponent("Failed to do match making");
                        break;
                    default:
                        return;
                }
            };

            Backend.Match.OnException += args =>
            {
                if(args is ObjectDisposedException)
                    return;

                MessageBoxUI.UseWithComponent(args.Message);
                if (gameServerJoined)
                {
                    Backend.Match.LeaveGameServer();
                    gameServerJoined = false;
                }
                Debug.Log(args.StackTrace);
                Debug.Log(args.Message);
            };

            Backend.Match.OnSessionJoinInServer += args =>
            {
                if(!roomJoined)
                {
                    if(!(latestGameRoom is null))
                    {
                        chessGameDirector = Director.instance.GetSubDirector<ChessGameDirector>();
                        Director.instance.DestroySubDirector(Director.instance.GetSubDirector<StartSceneDirector>());
                        gameServerJoined = true;
                        var oms = new OutputMemoryStream();
                        oms.Write(true);
                        Debug.Log("Send data to in game room on session join in server");
                        SendDataToInGameRoom(oms.buffer);
                    }
                    else
                        Backend.Match.JoinGameRoom(gameRoomToken);

                    roomJoined = true;
                }
            };
                
            Backend.Match.OnSessionListInServer += args =>
            {
                SessionIdList = new List<SessionId>();
                foreach (var session in args.SessionList)
                    SessionIdList.Add(session.SessionId);
                SessionIdList.Sort();
            };

            Backend.Match.OnMatchInGameAccess += args => 
            {
                if(SessionIdList is null)
                    SessionIdList = new List<SessionId>();

                foreach(var record in args.GameRecords)
                {
                    if (SessionIdList.Contains(record.m_sessionId))
                        continue;

                    SessionIdList.Add(record.m_sessionId);
                }
                SessionIdList.Sort();
            };
            Backend.Match.OnMatchInGameStart += () => 
            {
                if (!IsReconnect)
                {
                    if (chessGameDirector is null)
                        chessGameDirector = Director.instance.GetSubDirector<ChessGameDirector>();

                    TrySetHostSession();

                    if (IsHost)
                    {
                        //호스트: 0이 나오면 선공, 1이 나오면 후공
                        int rand = UnityEngine.Random.Range(0, 1);
                        OutputMemoryStream oms = new OutputMemoryStream();
                        oms.Write(rand);
                        Debug.Log("Send data to in game room on match in game start");
                        SendDataToInGameRoom(oms.buffer);
                    }
                }
            };
            Backend.Match.OnMatchRelay += args => 
            {
                if(!(latestGameRoom is null || inGameReady))
                {
                    if (args.From.SessionId != SessionId.None && !IsMySessionId(args.From.SessionId))
                    {
                        var ims = new InputMemoryStream(args.BinaryUserData);
                        ims.Read(out bool isHost);
                        ims.Read(out int enemyHp);
                        ims.Read(out int enemyEnergyPoint);
                        ims.Read(out int playerHp);
                        ims.Read(out int playerEnergyPoint);
                        ims.Read(out int turnCount);
                        ims.Read(out float lessTime);

                        IsHost = isHost;
                        chessGameDirector.SetPlayer(IsHost, GetNickNameBySessionId(SessionIdList[IsHost ? 1 : 0]));
                        chessGameDirector.player.Initialize(playerHp, playerEnergyPoint);
                        chessGameDirector.enemyPlayer.Initialize(enemyHp, enemyEnergyPoint);
                        chessGameDirector.turnCount.Value = turnCount;
                        chessGameDirector.chessGameUI.SetInputTimeCircle(lessTime);

                        ims.Read(out List<ChessPiece> pieces);

                        pieces.ForEach(piece =>
                        {
                            chessGameDirector.chessGameObjects.chessBoard.AddChessPiece(piece);
                        });

                        inGameReady = true;
                    }
                }
                else if(!inGameReady)
                {
                    var ims = new InputMemoryStream(args.BinaryUserData);
                    ims.Read(out int rand);

                    if (!IsHost)
                    {
                        //1을 받았을 경우 선공, 0을 받았을 경우 후공
                        if (rand == 1)
                            chessGameDirector.SetPlayer(true, GetNickNameBySessionId(SessionIdList[0]));
                        else
                            chessGameDirector.SetPlayer(false, GetNickNameBySessionId(SessionIdList[0]));
                    }
                    else
                    {
                        //1을 받았을 경우 후공, 0을 받았을 경우 선공
                        if (rand == 0)
                            chessGameDirector.SetPlayer(true, GetNickNameBySessionId(SessionIdList[1]));
                        else
                            chessGameDirector.SetPlayer(false, GetNickNameBySessionId(SessionIdList[1]));
                    }

                    inGameReady = true;
                }
                else
                {
                    if (GetOppositeSessionId() == args.From.SessionId)
                    {
                        if (OppositeDisconnected)
                        {
                            chessGameDirector.chessGameUI.UpdateReconnectOpposite();
                            var oms = new OutputMemoryStream();
                            oms.Write(!IsHost);
                            oms.Write(chessGameDirector.player.Hp.Value);
                            oms.Write(chessGameDirector.player.EnergyPoint.Value);
                            oms.Write(chessGameDirector.enemyPlayer.Hp.Value);
                            oms.Write(chessGameDirector.enemyPlayer.EnergyPoint.Value);
                            oms.Write(chessGameDirector.turnCount.Value);
                            oms.Write(chessGameDirector.chessGameUI.LessTime);
                            oms.Write(chessGameDirector.chessGameObjects.chessBoard.Pieces);
                            Debug.Log("Send data to in game room on match relay");
                            SendDataToInGameRoom(oms.buffer);
                            OppositeDisconnected = false;
                        }
                        else
                        {
                            var ims = new InputMemoryStream(args.BinaryUserData);
                            ims.Read(out chessGameDirector.enemyPlayer.chessActions);
                            chessGameDirector.ReceivedData.Value = true;
                        }
                    }
                }
            };

            Backend.Match.OnMatchChat += args => { };

            Backend.Match.OnLeaveInGameServer += args => 
            {
                gameServerJoined = false;
                roomJoined = false;
                inGameReady = false;
                roomToken = null;
                OppositeDisconnected = false;
                oppositeDisconnectedCount = 0;
                SessionIdList = null;
                latestGameRoom = null;
            };
            Backend.Match.OnSessionOnline += args =>
              {
              };

            Backend.Match.OnSessionOffline += args => 
            {
                if(oppositeDisconnectedCount > 0)
                {
                    chessGameDirector.isPlayerWin = true;
                    MatchEnd(true);
                    return;
                }
                oppositeDisconnectedCount++;
                OppositeDisconnected = true;
                chessGameDirector.chessGameUI.UpdateDisconnectOpposite();
            };
        }

        public void CustomSignUp(string id, string password, Action successCallback, Action<string> failedCallback)
        {
            var success = new ReactiveProperty<bool>();
            var bro = new BackendReturnObject();

            Backend.BMember.CustomSignUp(id, password, c =>
            {
                bro = c;
                success.Value = true;
            });

            success.ObserveOnMainThread().Subscribe(value =>
            {
                if (value)
                {
                    var saveToken = Backend.BMember.SaveToken(bro);
                    if (saveToken.IsSuccess())
                        successCallback();
                    else
                    {
                        switch ((SignUpCode)Convert.ToInt32(bro.GetStatusCode()))
                        {
                            case SignUpCode.DuplicatedParameterException:
                                failedCallback("Failed to sign up: Duplicated id");
                                break;
                            case SignUpCode.Etc:
                                failedCallback("Failed to sign up");
                                break;
                            default:
                                return;
                        }
                    }

                    bro.Clear();
                    success.Dispose();
                }
            });
        }

        public void CustomLogin(string id, string password, Action successCallback, Action<string> failedCallback)
        {
            var success = new ReactiveProperty<bool>();
            var bro = new BackendReturnObject();

            Backend.BMember.CustomLogin(id, password, c =>
            {
                bro = c;
                success.Value = true;
            });

            success.ObserveOnMainThread().Subscribe(value =>
            {
                if (value)
                {
                    var saveToken = Backend.BMember.SaveToken(bro);
                    if (saveToken.IsSuccess())
                        successCallback();
                    else
                    {
                        switch ((SignInCode)Convert.ToInt32(bro.GetStatusCode()))
                        {
                            case SignInCode.BadUnauthorizedException:
                                failedCallback("Failed to sign in: wrong id or password");
                                break;
                            case SignInCode.Blocked:
                                failedCallback("Failed to sign in: blocked user");
                                break;
                            case SignInCode.Etc:
                                failedCallback("Failed to sign in");
                                return;
                        }
                    }

                    bro.Clear();
                    success.Dispose();
                }
            });
        }

        public void LoginWithBackendToken(Action successCallback, Action<string> failedCallback)
        {
            if (PlayerPrefs.HasKey("access_token"))
            {
                var bro = Backend.BMember.LoginWithTheBackendToken();
                if (bro.IsSuccess())
                    successCallback();
                else
                    failedCallback("Failed to log in");
            }
        }

        public void SignOut(Action successCallback, Action<string> failedCallback)
        {
            if (MatchMakingServerJoined)
                LeaveMatchMakingServer();

            var success = new ReactiveProperty<bool>();
            var bro = new BackendReturnObject();

            Backend.BMember.SignOut(c =>
            {
                bro = c;
                success.Value = true;
            });

            success.ObserveOnMainThread().Subscribe(value =>
            {
                if (value)
                {
                    var saveToken = Backend.BMember.SaveToken(bro);
                    if (saveToken.IsSuccess())
                        successCallback();
                    else
                        failedCallback("Failed to sign out");

                    bro.Clear();
                    success.Dispose();
                }
            });
        }

        public void LogOut(Action successCallback, Action<string> failedCallback)
        {
            if (MatchMakingServerJoined)
                LeaveMatchMakingServer();

            var success = new ReactiveProperty<bool>();
            var bro = new BackendReturnObject();

            Backend.BMember.Logout(c =>
            {
                bro = c;
                success.Value = true;
            });

            success.ObserveOnMainThread().Subscribe(value =>
            {
                if (value)
                {
                    var saveToken = Backend.BMember.SaveToken(bro);
                    if (saveToken.IsSuccess())
                        successCallback();
                    else
                        failedCallback("Failed to log out");

                    bro.Clear();
                    success.Dispose();
                }
            });
        }

        public void GetUserInfo(Action<JsonData> successCallback)
        {
            var success = new ReactiveProperty<bool>();
            var bro = new BackendReturnObject();

            Backend.BMember.GetUserInfo(c =>
            {
                bro = c;
                success.Value = true;
            });

            success.ObserveOnMainThread().Subscribe(value =>
            {
                if (value)
                {
                    //var saveToken = Backend.BMember.SaveToken(bro);
                    if (bro.IsSuccess())
                    {
                        var infoJson = bro.GetReturnValuetoJSON()["row"];
                        successCallback(infoJson);
                    }

                    bro.Clear();
                    success.Dispose();
                }
            });
        }

        public void CreateNickname(string nickname, Action successCallback, Action<string> failedCallback)
        {
            var success = new ReactiveProperty<bool>();
            var bro = new BackendReturnObject();

            Backend.BMember.CreateNickname(nickname, c =>
            {
                bro = c;
                success.Value = true;
            });

            success.ObserveOnMainThread().Subscribe(value =>
            {
                if (value)
                {
                    var saveToken = Backend.BMember.SaveToken(bro);
                    if (saveToken.IsSuccess())
                        successCallback();
                    else
                    {
                        switch ((SetNicknameCode)Convert.ToInt32(bro.GetStatusCode()))
                        {
                            case SetNicknameCode.BadParameterException:
                                failedCallback("Failed to set nickname: Nickname doesn't fit");
                                break;
                            case SetNicknameCode.DuplicatedParameterException:
                                failedCallback("Failed to set nickname: Duplicated nickname");
                                break;
                            case SetNicknameCode.Etc:
                                failedCallback("Failed to set nickname");
                                break;
                        }
                    }

                    bro.Clear();
                    success.Dispose();
                }
            });
        }

        public void UpdateNickname(string nickname, Action successCallback,Action<string> failedCallback)
        {
            var success = new ReactiveProperty<bool>();
            var bro = new BackendReturnObject();

            Backend.BMember.UpdateNickname(nickname, c =>
            {
                bro = c;
                success.Value = true;
            });

            success.ObserveOnMainThread().Subscribe(value =>
            {
                if (value)
                {
                    var saveToken = Backend.BMember.SaveToken(bro);
                    if (saveToken.IsSuccess())
                        successCallback();
                    else
                    {
                        switch ((SetNicknameCode)Convert.ToInt32(bro.GetStatusCode()))
                        {
                            case SetNicknameCode.BadParameterException:
                                failedCallback("Failed to update nickname: nickname doesn't fit");
                                break;
                            case SetNicknameCode.DuplicatedParameterException:
                                failedCallback("Failed to update nickname: duplicated nickname");
                                break;
                            case SetNicknameCode.Etc:
                                failedCallback("Failed to update nickname");
                                break;
                        }
                    }

                    bro.Clear();
                    success.Dispose();
                }
            });
        }

        public void GetFriendList(Action<JsonData> successCallback)
        {
            var success = new ReactiveProperty<bool>();
            var bro = new BackendReturnObject();

            Backend.Social.Friend.GetFriendList(c =>
            {
                bro = c;
                success.Value = true;
            });

            success.ObserveOnMainThread().Subscribe(value =>
            {
                if (value)
                {
                    if (bro.IsSuccess())
                    {
                        LitJson.JsonData jsonData = bro.GetReturnValuetoJSON();
                        successCallback(jsonData);
                    }

                    bro.Clear();
                    success.Dispose();
                }
            });
        }

        public void GetReceivedRequestList(Action<JsonData> successCallback)
        {
            var success = new ReactiveProperty<bool>();
            var bro = new BackendReturnObject();

            Backend.Social.Friend.GetReceivedRequestList(c =>
            {
                bro = c;
                success.Value = true;
            });

            success.ObserveOnMainThread().Subscribe(value =>
            {
                if (value)
                {
                    if (bro.IsSuccess())
                    {
                        JsonData jsonData = bro.GetReturnValuetoJSON();
                        successCallback(jsonData);

                    }

                    bro.Clear();
                    success.Dispose();
                }
            });
        }

        public void GetSentRequestList(Action<JsonData> successCallback)
        {
            var success = new ReactiveProperty<bool>();
            var bro = new BackendReturnObject();

            Backend.Social.Friend.GetSentRequestList(c =>
            {
                bro = c;
                success.Value = true;
            });

            success.ObserveOnMainThread().Subscribe(value =>
            {
                if (value)
                {
                    var saveToken = Backend.BMember.SaveToken(bro);
                    if (saveToken.IsSuccess())
                    {
                        JsonData jsonData = saveToken.GetReturnValuetoJSON();
                        successCallback(jsonData);
                    }
                }

                bro.Clear();
                success.Dispose();
            });
        }

        public void AcceptFriend(string friendInDate, Action successCallback, Action<string> failedCallback)
        {
            var success = new ReactiveProperty<bool>();
            var bro = new BackendReturnObject();

            Backend.Social.Friend.AcceptFriend(friendInDate, c =>
            {
                bro = c;
                success.Value = true;
            });

            success.ObserveOnMainThread().Subscribe(value =>
            {
                if (value)
                {
                    var saveToken = Backend.BMember.SaveToken(bro);
                    if (saveToken.IsSuccess())
                        successCallback();
                    else
                        failedCallback("Failed to accept friend");

                    bro.Clear();
                    success.Dispose();
                }
            });
    }

        public void RejectFriend(string friendInDate,Action successCallback,Action<string> failedCallback)
        {
            var success = new ReactiveProperty<bool>();
            var bro = new BackendReturnObject();

            Backend.Social.Friend.RejectFriend(friendInDate, c =>
            {
                bro = c;
                success.Value = true;
            });

            success.ObserveOnMainThread().Subscribe(value =>
            {
                if (value)
                {
                    var saveToken = Backend.BMember.SaveToken(bro);
                    if (saveToken.IsSuccess())
                        successCallback();
                    else
                        failedCallback("Failed to reject friend");

                    bro.Clear();
                    success.Dispose();
                }
            });
        }

        public void JoinMatchMakingServer(Action<string> failedCallback)
        {
            if (!Backend.Match.JoinMatchMakingServer(out var errorInfo))
                failedCallback("Failed to join match making server");
            else
                MatchMakingServerJoined = true;
        }

        public void LeaveMatchMakingServer()
        {
            Backend.Match.LeaveMatchMakingServer();
            MatchMakingServerJoined = false;
        }

        public void RequestMatchMaking(Action successCallback)
        {
            Backend.Match.RequestMatchMaking(MatchType.MMR, MatchModeType.OneOnOne);
            Backend.Match.OnMatchInGameStart += () => successCallback();
        }

        public void CancelMatchMaking() => Backend.Match.CancelMatchMaking();

        public void JoinGameServer(string serverAddr, ushort serverPort, bool isReconnect)
        {
            if (!Backend.Match.JoinGameServer(serverAddr, serverPort, isReconnect, out var errorInfo))
                MessageBoxUI.UseWithComponent("Failed to join game server");
        }

        public bool IsMySessionId(SessionId session) => Backend.Match.GetMySessionId() == session;

        public bool TrySetHostSession()
        {
            if (SessionIdList.Count != 2)
                return false;

            SessionIdList.Sort();

            IsHost = IsMySessionId(SessionIdList[0]);
            return true;
        }

        public string GetNickNameBySessionId(SessionId sessionId) => Backend.Match.GetNickNameBySessionId(sessionId);

        public void JoinGameRoom() => Backend.Match.JoinGameRoom(roomToken);

        public void SendDataToInGameRoom(byte[] data) => Backend.Match.SendDataToInGameRoom(data);

        private SessionId GetMySessionId() => IsHost ? SessionIdList[0] : SessionIdList[1];
        private SessionId GetOppositeSessionId() => IsHost ? SessionIdList[1] : SessionIdList[0];

        private JsonData latestGameRoom;
        public bool IsReconnect => !(latestGameRoom is null);

        public void LatestGameRoomActivate(Action successCallback)
        {
            var success = new ReactiveProperty<bool>();
            var bro = new BackendReturnObject();

            Backend.Match.IsGameRoomActivate(c =>
            {
                bro = c;
                success.Value = true;
            });

            success.ObserveOnMainThread().Where(value=>value).Subscribe(value =>
            {
                if (bro.IsSuccess() && bro.GetStatusCode() == "200")
                {
                    latestGameRoom = bro.GetReturnValuetoJSON();
                    successCallback();
                }

                bro.Clear();
                success.Dispose();
            });
        }

        public void RejoinGameServer(bool join) 
        {
            if (join) JoinGameServer((string)latestGameRoom["serverPublicHostName"], (ushort)latestGameRoom["serverPort"], true);
            else latestGameRoom = null;
        }

        public void LeaveGameServer() => Backend.Match.LeaveGameServer();

        public void MatchEnd(bool playerWin)
        {
            if (playerWin)
                Backend.Match.MatchEnd(new MatchGameResult { m_winners = new List<SessionId> { GetMySessionId() }, m_losers = new List<SessionId> { GetOppositeSessionId() } });
            else
                Backend.Match.MatchEnd(new MatchGameResult { m_winners = new List<SessionId> { GetOppositeSessionId() }, m_losers = new List<SessionId> { GetMySessionId() } });

            chessGameDirector.chessGameUI.gameOverWidget.gameObject.SetActive(true);
        }
    }
}
