using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace ChessCrush.UI
{
    public class LoadingWidget: MonoBehaviour
    {
        [SerializeField]
        private RectTransform loadingCircle;
        [SerializeField]
        private float rotateSpeed = 200f;

        private void Awake()
        {
            loadingCircle.UpdateAsObservable().Subscribe(_ => loadingCircle.Rotate(0f, 0f, rotateSpeed * Time.deltaTime)).AddTo(gameObject);
        }
    }
}