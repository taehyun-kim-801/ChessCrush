using ChessCrush.Game;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class StartUI: MonoBehaviour
    {
        [SerializeField]
        private Button signInButton;
        [SerializeField]
        private SignWidget signWidget;
        [SerializeField]
        private GameObject afterSignInButtons;
        [SerializeField]
        private Button startButton;
        [SerializeField]
        private Button optionsButton;
        [SerializeField]
        private Button friendsButton;
        [SerializeField]
        private Button quitButton;
        [SerializeField]
        private GameObject loadingWidget;
        [SerializeField]
        private GameObject optionsWidget;
        [SerializeField]
        private GameObject friendsWidget;

        private StartSceneDirector startSceneDirector;
        private BackendDirector backendDirector;

        private void Awake()
        {
            signInButton.OnClickAsObservable().Subscribe(_ => signWidget.gameObject.SetActive(true)).AddTo(gameObject);
            startButton.OnClickAsObservable().Subscribe(_ => SubscribeStartButton()).AddTo(gameObject);
            optionsButton.OnClickAsObservable().Subscribe(_ => SubscribeOptionsButton()).AddTo(gameObject);
            friendsButton.OnClickAsObservable().Subscribe(_ => friendsWidget.SetActive(true)).AddTo(gameObject);
            quitButton.OnClickAsObservable().Subscribe(_ => SubscribeQuitButton()).AddTo(gameObject);
        }

        private void Start()
        {
            startSceneDirector = Director.instance.GetSubDirector<StartSceneDirector>();
            backendDirector = Director.instance.GetSubDirector<BackendDirector>();

            startSceneDirector.signedIn.Subscribe(_ => SubscribeSignedIn(_)).AddTo(startSceneDirector);

            backendDirector.LoginWithBackendToken(() => startSceneDirector.signedIn.Value = true, str => MessageBoxUI.UseWithComponent(str));
        }

        private void SubscribeStartButton()
        {
            if(ReferenceEquals(Director.instance.userInfo.Value.nickname, null))
            {
                MessageBoxUI.UseWithComponent("Please write your nickname first");
                return;
            }

            loadingWidget.SetActive(true);
            backendDirector.RequestMatchMaking(SetAfterRequestMatchMaking);
        }

        private void SetAfterRequestMatchMaking()
        {
            loadingWidget.SetActive(false);
            Director.instance.GetSubDirector<ChessGameDirector>();
            Director.instance.DestroySubDirector(startSceneDirector);
        }

        private void SubscribeOptionsButton()
        {
            optionsWidget.SetActive(true);
        }

        public void SubscribeSignedIn(bool value)
        {
            signInButton.gameObject.SetActive(!value);
            afterSignInButtons.gameObject.SetActive(value);
            Director.instance.GetUserInfo();
        }

        private void SubscribeQuitButton()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
    }
}