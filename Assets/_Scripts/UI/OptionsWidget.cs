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
        private BackendDirector backendDirector;

        private void Awake()
        {
            Director.instance.userInfo.Subscribe(info => nameInputField.text = info.nickname).AddTo(Director.instance);

            nameChangeButton.OnClickAsObservable().Subscribe(_ => backendDirector.UpdateNickname(nameInputField.text, SetAfterUpdateNickname, str => MessageBoxUI.UseWithComponent(str))).AddTo(gameObject);
            exitButton.OnClickAsObservable().Subscribe(_ => gameObject.SetActive(false)).AddTo(gameObject);
            signOutButton.OnClickAsObservable().Subscribe(_ => backendDirector.SignOut(SetAfterSignOut,str=>MessageBoxUI.UseWithComponent(str))).AddTo(gameObject);
            logOutButton.OnClickAsObservable().Subscribe(_ => backendDirector.LogOut(SetAfterLogOut, str => MessageBoxUI.UseWithComponent(str))).AddTo(gameObject);
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

        private void SetAfterSignOut()
        {
            MessageBoxUI.UseWithComponent("Success to sign out");
            PlayerPrefs.DeleteKey("access_token");
            PlayerPrefs.DeleteKey("refresh_token");
            startSceneDirector.signedIn.Value = false;
            gameObject.SetActive(false);
        }

        private void SetAfterLogOut()
        {
            MessageBoxUI.UseWithComponent("Success to log out");
            startSceneDirector.signedIn.Value = false;
            gameObject.SetActive(false);
        }
    }
}