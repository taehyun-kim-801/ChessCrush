using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class InputTimeCircle: MonoBehaviour
    {
        [SerializeField]
        private Image inputTimeCircle;
        [SerializeField]
        private Text timeText;

        private float enableTime;
        public float LessTime { get { return Time.time - enableTime; } }

        private void Awake()
        {
            inputTimeCircle.UpdateAsObservable().Subscribe(_ =>
            {
                inputTimeCircle.fillAmount = (30f - (Time.time - enableTime)) / 30f;
                if (inputTimeCircle.fillAmount <= 0)
                    gameObject.SetActive(false);
            }).AddTo(gameObject);

            timeText.UpdateAsObservable().Subscribe(_ => timeText.text = (30 - ((int)(Time.time - enableTime))).ToString()).AddTo(gameObject);
        }

        private void OnEnable()
        {
            enableTime = Time.time;
        }
    }
}