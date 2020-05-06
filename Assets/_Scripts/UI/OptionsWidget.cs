using BackEnd;
using ChessCrush.Game;
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

        private void Awake()
        {
            Director.instance.userInfo.Subscribe(info => nameInputField.text = info.nickname).AddTo(Director.instance);

            nameChangeButton.OnClickAsObservable().Subscribe(_ => SubscribeNameChangeButton()).AddTo(gameObject);
            exitButton.OnClickAsObservable().Subscribe(_ => SubscribeExitButton()).AddTo(gameObject);
            signOutButton.OnClickAsObservable().Subscribe(_ => SubscribeSignOutButton()).AddTo(gameObject);
            logOutButton.OnClickAsObservable().Subscribe(_ => SubscribeLogOutButton()).AddTo(gameObject);
        }

        private void Start()
        {
            startSceneDirector = Director.instance.GetSubDirector<StartSceneDirector>();
        }

        private void SubscribeNameChangeButton()
        {
            var name = nameInputField.text;
        }

        private void SubscribeExitButton()
        {
            gameObject.SetActive(false);
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
                }
            });
        }
    }
}