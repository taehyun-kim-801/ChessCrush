using TMPro;
using UnityEngine;

namespace ChessCrush.UI
{
    public class ChessGameAlert: MonoBehaviour
    {
        private Animator animator;
        [SerializeField] private TextMeshProUGUI text;

        private void Awake()
        {
            animator = gameObject.GetComponent<Animator>();
        }

        public void Appear(string text)
        {
            gameObject.SetActive(true);
            this.text.text = text;
            animator.Play($"{nameof(ChessGameAlert)}_Appear");
        }

        public void Disappear()
        {
            animator.Play($"{nameof(ChessGameAlert)}_Disappear");
            gameObject.SetActive(false);
        }
    }
}
