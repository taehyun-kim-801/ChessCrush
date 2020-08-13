using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class MessageBoxUI: MonoBehaviour
    {
        [SerializeField]
        private Button backgroundButton;
        [SerializeField]
        private Text text;

        private static MessageBoxUI _Instance;

        private void Awake()
        {
            backgroundButton.OnClickAsObservable().Subscribe(_ => gameObject.SetActive(false));
        }

        public static MessageBoxUI UseWithComponent(string text)
        {
            if(_Instance is null)
            {
                var obj = MainCanvas.instance.objectPool.Use(nameof(MessageBoxUI));
                _Instance = obj.GetComponent<MessageBoxUI>();
            }
            return _Instance.Set(text);
        }

        private MessageBoxUI Set(string text)
        {
            this.text.text = text;
            return this;
        }
    }
}