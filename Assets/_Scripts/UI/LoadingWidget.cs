using BackEnd;
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
        [SerializeField]
        private Button cancelButton;
        [SerializeField]
        private float rotateSpeed = 200f;

        private void Awake()
        {
            loadingCircle.UpdateAsObservable().Subscribe(_ => loadingCircle.Rotate(0f, 0f, rotateSpeed * Time.deltaTime)).AddTo(gameObject);
            cancelButton.OnClickAsObservable().Subscribe(_ => SubscribeCancelButton()).AddTo(gameObject);
        }

        private void SubscribeCancelButton()
        {
            Backend.Match.CancelMatchMaking();
            gameObject.SetActive(false);
        }
    }
}