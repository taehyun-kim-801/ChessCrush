using ChessCrush.Game;
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

        public readonly ReactiveProperty<float> LessTime = new ReactiveProperty<float>();

        private BackendDirector backendDirector;

        private void Awake()
        {
            LessTime.Subscribe(value =>
            {
                inputTimeCircle.fillAmount = value / 30f;
                timeText.text = ((int)value).ToString();
            }).AddTo(gameObject);
            LessTime.Where(value => value <= 0).Subscribe(_ => gameObject.SetActive(false)).AddTo(gameObject);

            gameObject.UpdateAsObservable().Where(_ => !backendDirector.OppositeDisconnected).Subscribe(_ => LessTime.Value -= Time.deltaTime).AddTo(gameObject);
        }

        private void Start()
        {
            backendDirector = Director.instance.GetSubDirector<BackendDirector>();
        }

        private void OnEnable()
        {
            LessTime.Value = 30f;
        }
    }
}