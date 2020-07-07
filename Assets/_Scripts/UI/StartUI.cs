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
        private LoadingObjects loadingObjects;
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
        [SerializeField]
        private GameObject latestGameAlert;

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

#if !UNITY_EDITOR
            backendDirector.LoginWithBackendToken(() => startSceneDirector.signedIn.Value = true, str => MessageBoxUI.UseWithComponent(str));
#endif
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
            if (value)
                loadingObjects.gameObject.SetActive(true);
            else
            {
                afterSignInButtons.gameObject.SetActive(value);
                Director.instance.userInfo.Value = default;
            }
        }

        public void AfterLoading()
        {
            loadingObjects.gameObject.SetActive(false);
            afterSignInButtons.gameObject.SetActive(true);
            backendDirector.LatestGameRoomActivate(() => latestGameAlert.GetComponent<Animator>().Play("Appear"));
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