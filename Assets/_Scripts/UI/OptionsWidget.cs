using ChessCrush.Game;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class OptionsWidget:MonoBehaviour
    {
        [SerializeField]
        private InputField nameInputField;
        [SerializeField]
        private Button nameChangeButton;
        [SerializeField]
        private Button exitButton;

        private void Awake()
        {
            nameChangeButton.OnClickAsObservable().Subscribe(_ => SubscribeNameChangeButton()).AddTo(gameObject);
            exitButton.OnClickAsObservable().Subscribe(_ => SubscribeExitButton()).AddTo(gameObject);
            if (!(Director.instance.playerName is null))
                nameInputField.text = Director.instance.playerName;
        }

        private void SubscribeNameChangeButton()
        {
            var name = nameInputField.text;
            Director.instance.playerName = name;
        }

        private void SubscribeExitButton()
        {
            gameObject.SetActive(false);
        }
    }
}