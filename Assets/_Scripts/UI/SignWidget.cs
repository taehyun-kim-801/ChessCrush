using ChessCrush.Game;
using System.Collections;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class SignWidget: MonoBehaviour
    {
        [SerializeField]
        private StartUI startUI;
        [SerializeField]
        private GameObject signInField;
        [SerializeField]
        private InputField signInIDInputField;
        [SerializeField]
        private InputField signInPWInputField;
        [SerializeField]
        private Button signInSignInButton;
        [SerializeField]
        private Button signInSignUpButton;
        [SerializeField]
        private GameObject signUpField;
        [SerializeField]
        private InputField signUpIDInputField;
        [SerializeField]
        private InputField signUpPWInputField;
        [SerializeField]
        private InputField signUpConfirmInputField;
        [SerializeField]
        private Button signUpSignUpButton;
        [SerializeField]
        private Button exitButton;

        private void Awake()
        {
            signInSignInButton.OnClickAsObservable().Subscribe(_ => SubscribeSignInButton());
            signInSignUpButton.OnClickAsObservable().Subscribe(_ => SubscribeSignInSignUpButton());
            signUpSignUpButton.OnClickAsObservable().Subscribe(_ => SubscribeSignUpSignUpButton());
            exitButton.OnClickAsObservable().Subscribe(_ => gameObject.SetActive(false));
        }

        private void OnEnable()
        {
            signInField.SetActive(true);
            signInIDInputField.text = "";
            signInPWInputField.text = "";
            signUpField.SetActive(false);
            signUpIDInputField.text = "";
            signUpPWInputField.text = "";
            signUpConfirmInputField.text = "";
        }

        private void SubscribeSignInButton()
        {
            StartCoroutine(CoSubscribeSignInButton());
        }

        private IEnumerator CoSubscribeSignInButton()
        {
            if (signInIDInputField.text == "" || signInPWInputField.text == "")
            {
                MessageBoxUI.UseWithComponent("Please input all input fields");
                yield break;
            }

            var result = Task.Run(() => Director.instance.networkHelper.SignIn(signInIDInputField.text, signInPWInputField.text));
            yield return new WaitUntil(() => result.IsCompleted);

            if (result.Result == OperationResultCode.SignInCode.Success)
            {
                MessageBoxUI.UseWithComponent("Success to sign in");
                Director.instance.playerName = signInIDInputField.text;
                startUI.SetAfterSignIn();
                gameObject.SetActive(false);
            }
            else
            {
                MessageBoxUI.UseWithComponent("Failed to sign in");
            }
        }

        private void SubscribeSignInSignUpButton()
        {
            signInField.SetActive(false);
            signUpField.SetActive(true);
        }

        private void SubscribeSignUpSignUpButton()
        {
            StartCoroutine(CoSubscribeSignUpSignUpButton());
        }

        private IEnumerator CoSubscribeSignUpSignUpButton()
        {
            if (signUpIDInputField.text == "" || signUpPWInputField.text == "" || signUpConfirmInputField.text == "")
            {
                MessageBoxUI.UseWithComponent("Please input all input fields");
                yield break;
            }

            if (signUpPWInputField.text != signUpConfirmInputField.text)
            {
                signUpConfirmInputField.text = "";
                MessageBoxUI.UseWithComponent("Password and confirm are different");
                yield break;
            }

            var result = Task.Run(() => Director.instance.networkHelper.SignUp(signUpIDInputField.text, signUpPWInputField.text));
            yield return new WaitUntil(() => result.IsCompleted);

            switch(result.Result)
            {
                case OperationResultCode.SignUpCode.Success:
                    MessageBoxUI.UseWithComponent("Success to sign up");
                    startUI.SetAfterSignIn();
                    gameObject.SetActive(false);
                    break;
                case OperationResultCode.SignUpCode.DuplicatedParameterException:
                    MessageBoxUI.UseWithComponent("Failed to sign up: Duplicated id");
                    break;
                case OperationResultCode.SignUpCode.Etc:
                    break;
            }
        }
    }
}