using BackEnd;
using ChessCrush.Game;
using ChessCrush.OperationResultCode;
using System;
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
        private GameObject setInfoField;
        [SerializeField]
        private InputField nicknameInputField;
        [SerializeField]
        private Button infoSetButton;
        [SerializeField]
        private Button exitButton;

        private StartSceneDirector startSceneDirector;
        private BackendDirector backendDirector;

        private void Awake()
        {
            signInSignInButton.OnClickAsObservable().Subscribe(_ => SubscribeSignInButton());
            signInSignUpButton.OnClickAsObservable().Subscribe(_ => SubscribeSignInSignUpButton());
            signUpSignUpButton.OnClickAsObservable().Subscribe(_ => SubscribeSignUpSignUpButton());
            infoSetButton.OnClickAsObservable().Subscribe(_ => SubscribeInfoSetButton());
            exitButton.OnClickAsObservable().Subscribe(_ => gameObject.SetActive(false));
        }

        private void Start()
        {
            startSceneDirector = Director.instance.GetSubDirector<StartSceneDirector>();
            backendDirector = Director.instance.GetSubDirector<BackendDirector>();
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
            setInfoField.SetActive(false);
        }

        private void SubscribeSignInButton()
        {
            if (signInIDInputField.text == "" || signInPWInputField.text == "")
            {
                MessageBoxUI.UseWithComponent("Please input all input fields");
                return;
            }

            backendDirector.CustomLogin(signInIDInputField.text, signInPWInputField.text, SetAfterSignIn, str => MessageBoxUI.UseWithComponent(str));
        }

        private void SetAfterSignIn()
        {
            MessageBoxUI.UseWithComponent("Success to sign in");
            startSceneDirector.signedIn.Value = true;
            gameObject.SetActive(false);
        }

        private void SubscribeSignInSignUpButton()
        {
            signInField.SetActive(false);
            signUpField.SetActive(true);
        }

        private void SubscribeSignUpSignUpButton()
        {
            if (signUpIDInputField.text == "" || signUpPWInputField.text == "" || signUpConfirmInputField.text == "")
            {
                MessageBoxUI.UseWithComponent("Please input all input fields");
                return;
            }

            if (signUpPWInputField.text != signUpConfirmInputField.text)
            {
                signUpConfirmInputField.text = "";
                MessageBoxUI.UseWithComponent("Password and confirm are different");
                return;
            }

            backendDirector.CustomSignUp(signUpIDInputField.text, signUpPWInputField.text, SetAfterSignUp, str => MessageBoxUI.UseWithComponent(str));
        }

        private void SetAfterSignUp()
        {
            startSceneDirector.signedIn.Value = true;
            signUpField.SetActive(false);
            setInfoField.SetActive(true);
        }

        private void SubscribeInfoSetButton() 
        {
            if (nicknameInputField.text == "")
            {
                MessageBoxUI.UseWithComponent("Please input nickname field");
                return;
            }

            backendDirector.CreateNickname(nicknameInputField.text, SetAfterInfoSet, str => MessageBoxUI.UseWithComponent(str));
        }

        private void SetAfterInfoSet()
        {
            MessageBoxUI.UseWithComponent("Success to sign in");
            Director.instance.GetUserInfo();
            gameObject.SetActive(false);
        }
    }
}