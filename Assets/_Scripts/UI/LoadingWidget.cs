using BackEnd;
using ChessCrush.Game;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class LoadingWidget: MonoBehaviour
    {
        [SerializeField]
        private RectTransform loadingCircle;
        public Button cancelButton;
        [SerializeField]
        private float rotateSpeed = 200f;

        private BackendDirector backendDirector;

        private void Awake()
        {
            loadingCircle.UpdateAsObservable().Subscribe(_ => loadingCircle.Rotate(0f, 0f, rotateSpeed * Time.deltaTime)).AddTo(gameObject);
            cancelButton.OnClickAsObservable().Subscribe(_ => SubscribeCancelButton()).AddTo(gameObject);
        }

        private void Start()
        {
            backendDirector = Director.instance.GetSubDirector<BackendDirector>();
            backendDirector.matchMakingSuccessCallback = () => cancelButton.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            cancelButton.gameObject.SetActive(true);
        }

        private void SubscribeCancelButton()
        {
            Backend.Match.CancelMatchMaking();
            gameObject.SetActive(false);
        }
    }
}