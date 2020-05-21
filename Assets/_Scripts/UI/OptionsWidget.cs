using BackEnd;
using ChessCrush.Game;
using ChessCrush.OperationResultCode;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class OptionsWidget:MonoBehaviour
    {
        [SerializeField]
        private InputField nameInputField;
        [SerializeField]
        private Button nameChangeButton;
        [SerializeField]
        private Button exitButton;
        [SerializeField]
        private Button signOutButton;
        [SerializeField]
        private Button logOutButton;

        private StartSceneDirector startSceneDirector;
        private BackendDirector backendDirector;

        private void Awake()
        {
            Director.instance.userInfo.Subscribe(info => nameInputField.text = info.nickname).AddTo(Director.instance);

            nameChangeButton.OnClickAsObservable().Subscribe(_ => backendDirector.UpdateNickname(nameInputField.text, SetAfterUpdateNickname, str => MessageBoxUI.UseWithComponent(str))).AddTo(gameObject);
            exitButton.OnClickAsObservable().Subscribe(_ => gameObject.SetActive(false)).AddTo(gameObject);
            signOutButton.OnClickAsObservable().Subscribe(_ => SubscribeSignOutButton()).AddTo(gameObject);
            logOutButton.OnClickAsObservable().Subscribe(_ => SubscribeLogOutButton()).AddTo(gameObject);
        }

        private void Start()
        {
            startSceneDirector = Director.instance.GetSubDirector<StartSceneDirector>();
            backendDirector = Director.instance.GetSubDirector<BackendDirector>();
        }

        private void SetAfterUpdateNickname()
        {
            MessageBoxUI.UseWithComponent("Success to update nickname");
            Director.instance.GetUserInfo();
        }

        private void SubscribeSignOutButton()
        {
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
                    if (bro.IsSuccess())
                    {
                        MessageBoxUI.UseWithComponent("Success to sign out");
                        PlayerPrefs.DeleteKey("access_token");
                        PlayerPrefs.DeleteKey("refresh_token");
                        startSceneDirector.signedIn.Value = false;
                        gameObject.SetActive(false);
                    }
                    else
                        MessageBoxUI.UseWithComponent("Faield to sign out");

                    bro.Clear();
                    success.Dispose();
                }
            });
        }

        private void SubscribeLogOutButton()
        {
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
                    if (bro.IsSuccess())
                    {
                        MessageBoxUI.UseWithComponent("Success to log out");
                        startSceneDirector.signedIn.Value = false;
                        gameObject.SetActive(false);
                    }
                    else
                        MessageBoxUI.UseWithComponent("Faield to log out");

                    bro.Clear();
                    success.Dispose();
                }
            });
        }
    }
}