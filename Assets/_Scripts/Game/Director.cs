using ChessCrush.UI;
using System.Collections;
using UnityEngine;
using BackEnd;
using System;
using UniRx;
using UniRx.Triggers;

namespace ChessCrush.Game
{
    public class Director: MonoBehaviour
    {
        static public Director instance;
        [NonSerialized]
        public ObjectPool nonUiObjectPool;

        public ReactiveProperty<UserInfo> userInfo = new ReactiveProperty<UserInfo>();
        public string playerName;
        public NetworkHelper networkHelper;

        private void Awake()
        {
            if(instance)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
            instance = this;

            networkHelper = new NetworkHelper();

            Backend.Initialize(() =>
            {
                if (Backend.IsInitialized)
                {
                    SetBackendSetting();
                    gameObject.UpdateAsObservable().Subscribe(_ =>
                    {
                        Backend.Match.poll();
                    }).AddTo(gameObject);
                    networkHelper.connected = true;
                }
                else
                    networkHelper.connected = false;
            });

            nonUiObjectPool = Instantiate(Resources.Load("Prefabs/NonUIObjectPool") as GameObject).GetComponent<ObjectPool>();
            Instantiate(Resources.Load("Prefabs/MainCanvas") as GameObject);
        }


        private IEnumerator Start()
        {
            yield return new WaitUntil(() => nonUiObjectPool.isCreated && MainCanvas.instance.objectPool.isCreated && Backend.IsInitialized);
            GetSubDirector<StartSceneDirector>();
        }

        private void SetBackendSetting()
        {
            Backend.Match.OnJoinMatchMakingServer += args => 
            {
                Backend.Match.RequestMatchMaking(BackEnd.Tcp.MatchType.MMR, BackEnd.Tcp.MatchModeType.OneOnOne);
            };
            Backend.Match.OnLeaveMatchMakingServer += args => { };
            Backend.Match.OnMatchMakingResponse += args => { };
            Backend.Match.OnException += args => { };

            Backend.Match.OnSessionJoinInServer += args => { };
            Backend.Match.OnSessionOnline += args => { };
            Backend.Match.OnSessionListInServer += args => { };
            Backend.Match.OnMatchInGameAccess += args => { };
            Backend.Match.OnMatchInGameStart += () => { };
            Backend.Match.OnMatchRelay += args => { };
            Backend.Match.OnMatchChat += args => { };
            Backend.Match.OnMatchResult += args => { };
            Backend.Match.OnLeaveInGameServer += args => { };
            Backend.Match.OnSessionOffline += args => { };
        }

        public T GetSubDirector<T>() where T:SubDirector
        {
            var director = SubDirectorsSet.Find<T>();

            var parameterType = typeof(T);
            var resultObj = SubDirectorsSet.directorDictionary[parameterType];

            if(resultObj is null)
            {
                resultObj = Instantiate(director.gameObject) as GameObject;
                SubDirectorsSet.directorDictionary[parameterType] = resultObj;
            }

            return resultObj.GetComponent<T>();
        }

        public void GetUserInfo()
        {
            var success = new ReactiveProperty<bool>();
            var bro = new BackendReturnObject();

            Backend.BMember.GetUserInfo(c =>
            {
                bro = c;
                success.Value = true;
            });

            success.Subscribe(value =>
            {
                if (value)
                {
                    if (bro.IsSuccess())
                    {
                        var infoJson = bro.GetReturnValuetoJSON()["row"];
                        SetUserInfoUsingJson(infoJson);
                    }
                }
            });
        }

        private void SetUserInfoUsingJson(LitJson.JsonData jsonData)
        {
            var newUserInfo = new UserInfo();
            newUserInfo.nickname = ReferenceEquals(jsonData["nickname"], null) ? null : (string)jsonData["nickname"];
            newUserInfo.inDate = ReferenceEquals(jsonData["inDate"], null) ? null : (string)jsonData["inDate"];
            newUserInfo.subscriptionType = ReferenceEquals(jsonData["subscriptionType"], null) ? null : (string)jsonData["subscriptionType"];
            newUserInfo.emailForFindPassword = ReferenceEquals(jsonData["emailForFindPassword"], null) ? null : (string)jsonData["emailForFindPassword"];

            userInfo.Value = newUserInfo;
        }
    }
}