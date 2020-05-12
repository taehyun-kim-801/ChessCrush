using BackEnd;
using ChessCrush.Game;
using ChessCrush.OperationResultCode;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class RequestWidget: MonoBehaviour
    {
        [SerializeField]
        private InputField nicknameInputField;
        [SerializeField]
        private Button requestButton;
        [SerializeField]
        private Button exitButton;

        private void Awake()
        {
            requestButton.OnClickAsObservable().Subscribe(_ => SubscribeRequestButton()).AddTo(gameObject);
            exitButton.OnClickAsObservable().Subscribe(_ => gameObject.SetActive(false)).AddTo(gameObject);
        }

        private void OnEnable()
        {
            nicknameInputField.text = "";
        }

        private void SubscribeRequestButton()
        {
            if(nicknameInputField.text==Director.instance.userInfo.Value.nickname)
            {
                MessageBoxUI.UseWithComponent("Please input except your nickname");
                return;
            }

            var success = new ReactiveProperty<bool>();
            var bro = new BackendReturnObject();

            Backend.Social.GetGamerIndateByNickname(nicknameInputField.text, c =>
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
                        if (jsonData["rows"].Count != 0)
                            RequestFriend((string)jsonData["rows"][0]["inDate"]["S"]);
                        else
                            MessageBoxUI.UseWithComponent("There's no player");
                    }

                    bro.Clear();
                    success.Dispose();
                }
            });
        }

        private void RequestFriend(string inDate)
        {
            var success = new ReactiveProperty<bool>();
            var bro = new BackendReturnObject();

            Backend.Social.Friend.RequestFriend(inDate, c =>
            {
                bro = c;
                success.Value = true;
            });

            success.ObserveOnMainThread().Subscribe(value =>
            {
                if (value)
                {
                    if (bro.IsSuccess())
                        MessageBoxUI.UseWithComponent("Success to request friend");
                    else
                    {
                        switch((RequestFriendCode)Convert.ToInt32(bro.GetStatusCode()))
                        {
                            case RequestFriendCode.ForbiddenException:
                                MessageBoxUI.UseWithComponent("Failed to request friend: forbidden");
                                break;
                            case RequestFriendCode.DuplicatedParameterException:
                                MessageBoxUI.UseWithComponent("Failed to request friend: already requested");
                                break;
                            case RequestFriendCode.PreconditionFailed:
                                MessageBoxUI.UseWithComponent("Failed to request friend: you or other's request is max");
                                break;
                            case RequestFriendCode.Etc:
                                MessageBoxUI.UseWithComponent("Failed to request friend");
                                break;
                        }
                    }

                    bro.Clear();
                    success.Dispose();
                }
            });
        }
    }
}