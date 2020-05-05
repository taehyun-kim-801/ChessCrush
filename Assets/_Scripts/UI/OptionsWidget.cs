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

        private StartSceneDirector startSceneDirector;

        private void Awake()
        {
            nameChangeButton.OnClickAsObservable().Subscribe(_ => SubscribeNameChangeButton()).AddTo(gameObject);
            exitButton.OnClickAsObservable().Subscribe(_ => SubscribeExitButton()).AddTo(gameObject);
            signOutButton.OnClickAsObservable().Subscribe(_ => SubscribeSignOutButton()).AddTo(gameObject);
            if (!(Director.instance.playerName is null))
                nameInputField.text = Director.instance.playerName;
        }

        private void Start()
        {
            startSceneDirector = Director.instance.GetSubDirector<StartSceneDirector>();
        }

        private void SubscribeNameChangeButton()
        {
            var name = nameInputField.text;
            Director.instance.playerName = name;
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
    }
}