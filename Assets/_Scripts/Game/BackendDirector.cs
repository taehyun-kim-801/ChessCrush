using BackEnd;
using ChessCrush.OperationResultCode;
using ChessCrush.UI;
using System;
using UniRx;
using UniRx.Triggers;

namespace ChessCrush.Game
{
    public class BackendDirector: SubDirector
    {
        private string roomToken;

        private void Awake()
        {
            Backend.Initialize(() =>
            {
                if (Backend.IsInitialized)
                    gameObject.UpdateAsObservable().Subscribe(_ => Backend.Match.poll()).AddTo(gameObject);
                else
                    MessageBoxUI.UseWithComponent("Failed to connect to server");
            });
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
                    if (bro.IsSuccess())
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
    }
}