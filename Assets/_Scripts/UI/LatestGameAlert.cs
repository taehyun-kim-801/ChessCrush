using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class LatestGameAlert: MonoBehaviour
    {
        [SerializeField] private Button okayButton;
        [SerializeField] private Button cancelButton;
        private Animator animator;

        private void Awake()
        {
            animator = gameObject.GetComponent<Animator>();
            okayButton.OnClickAsObservable().Subscribe(_ => animator.Play($"{nameof(LatestGameAlert)}_Disappear")).AddTo(gameObject);
            cancelButton.OnClickAsObservable().Subscribe(_ => animator.Play($"{nameof(LatestGameAlert)}_Disappear")).AddTo(gameObject);
        }

        private void OnEnable()
        {
            animator?.Play($"{nameof(LatestGameAlert)}_Appear");
        }

        private void DisappearAnimationFinished() => gameObject.SetActive(false);
    }
}