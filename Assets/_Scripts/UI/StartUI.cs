using BackEnd;
using ChessCrush.Game;
using System.Collections;
using System.Threading.Tasks;
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
        private Button quitButton;
        [SerializeField]
        private GameObject loadingWidget;
        [SerializeField]
        private GameObject optionsWidget;

        private StartSceneDirector startSceneDirector;

        private void Awake()
        {
            signInButton.OnClickAsObservable().Subscribe(_ => signWidget.gameObject.SetActive(true)).AddTo(gameObject);
            startButton.OnClickAsObservable().Subscribe(_ => SubscribeStartButton()).AddTo(gameObject);
            optionsButton.OnClickAsObservable().Subscribe(_ => SubscribeOptionsButton()).AddTo(gameObject);
            quitButton.OnClickAsObservable().Subscribe(_ => SubscribeQuitButton()).AddTo(gameObject);
        }

        private void Start()
        {
            startSceneDirector = Director.instance.GetSubDirector<StartSceneDirector>();
            startSceneDirector.signedIn.Subscribe(_ => SubscribeSignedIn(_)).AddTo(startSceneDirector);

            if (PlayerPrefs.HasKey("access_token"))
            {
                var value = PlayerPrefs.GetString("access_token");
                var bro = Backend.BMember.LoginWithTheBackendToken();
                if (bro.IsSuccess())
                    startSceneDirector.signedIn.Value = true;
                else
                    MessageBoxUI.UseWithComponent("Failed to login");
            }
        }

        private void SubscribeStartButton()
        {
            StartCoroutine(CoSubscribeStartButton());
        }

        private IEnumerator CoSubscribeStartButton()
        {
            var networkHelper = Director.instance.networkHelper;
            if (!networkHelper.socketConnected)
            {
                Debug.Log("Socket isn't connected");
                yield break;
            }

            loadingWidget.SetActive(true);
            var result = Task.Run(() => networkHelper.ParticipateGame());
            yield return new WaitUntil(() => result.IsCompleted);

            if (result.Result != -1)
            {
                Director.instance.GetSubDirector<ChessGameDirector>();
                gameObject.SetActive(false);
            }
            else
                MessageBoxUI.UseWithComponent("Failed to participating game");
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