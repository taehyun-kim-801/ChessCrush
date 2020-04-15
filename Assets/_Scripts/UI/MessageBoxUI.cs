using ChessCrush.Game;
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

        private void Awake()
        {
            backgroundButton.OnClickAsObservable().Subscribe(_ => gameObject.SetActive(false));
        }

        public static MessageBoxUI UseWithComponent(string text)
        {
            var obj = MainCanvas.instance.objectPool.Use(nameof(MessageBoxUI));
            var messageBox = obj.GetComponent<MessageBoxUI>();
            messageBox.Set(text);
            return messageBox;
        }

        public void Set(string text)
        {
            this.text.text = text;
        }
    }
}