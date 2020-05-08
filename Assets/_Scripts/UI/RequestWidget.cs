using BackEnd;
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

        private void Awake()
        {
            requestButton.OnClickAsObservable().Subscribe(_ => SubscribeRequestButton());
        }

        private void SubscribeRequestButton()
        {
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
                        var inDate = (string)jsonData["rows"][0]["inDate"]["S"];
                        if (!ReferenceEquals(inDate, null))
                            RequestFriend(inDate);
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