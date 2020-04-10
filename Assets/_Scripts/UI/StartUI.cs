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
        private Button startButton;
        [SerializeField]
        private Button optionsButton;
        [SerializeField]
        private Button quitButton;
        [SerializeField]
        private GameObject loadingWidget;
        [SerializeField]
        private GameObject optionsWidget;

        private void Awake()
        {
            startButton.OnClickAsObservable().Subscribe(_ => SubscribeStartButton()).AddTo(gameObject);
            optionsButton.OnClickAsObservable().Subscribe(_ => SubscribeOptionsButton()).AddTo(gameObject);
            quitButton.OnClickAsObservable().Subscribe(_ => SubscribeQuitButton()).AddTo(gameObject);
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
                Debug.Log("Failed to participating game");
        }

        private void SubscribeOptionsButton()
        {
            optionsWidget.SetActive(true);
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