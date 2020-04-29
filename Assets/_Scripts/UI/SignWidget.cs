using BackEnd;
using ChessCrush.Game;
using ChessCrush.OperationResultCode;
using System;
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
            if (signInIDInputField.text == "" || signInPWInputField.text == "")
            {
                MessageBoxUI.UseWithComponent("Please input all input fields");
                return;
            }

            var success = new ReactiveProperty<bool>();
            var bro = new BackendReturnObject();

            Backend.BMember.CustomLogin(signInIDInputField.text, signInPWInputField.text, c =>
              {
                  bro = c;
                  success.Value = true;
              });

            success.ObserveOnMainThread().Subscribe(value =>
            {
                if(value)
                {
                    var saveToken = Backend.BMember.SaveToken(bro);
                    if(bro.IsSuccess())
                    {
                        MessageBoxUI.UseWithComponent("Success to sign in");
                        Director.instance.playerName = signInIDInputField.text;
                        startUI.SetAfterSignIn();
                        gameObject.SetActive(false);
                    }
                    else
                    {
                        switch((SignInCode)Convert.ToInt32(bro.GetStatusCode()))
                        {
                            case SignInCode.BadUnauthorizedException:
                                MessageBoxUI.UseWithComponent("Failed to sign in: wrong id or password");
                                break;
                            case SignInCode.Blocked:
                                MessageBoxUI.UseWithComponent("Failed to sign in: blocked user");
                                break;
                            case SignInCode.Etc:
                                return;
                        }
                    }
                }
            });
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

            var success = new ReactiveProperty<bool>();
            var bro = new BackendReturnObject();

            Backend.BMember.CustomSignUp(signUpIDInputField.text, signUpPWInputField.text, c =>
            {
                bro = c;
                success.Value = true;
            });

            success.ObserveOnMainThread().Subscribe(value =>
            {
                if (value)
                {
                    var saveToken = Backend.BMember.SaveToken(bro);
                    if (bro.IsSuccess())
                    {
                        MessageBoxUI.UseWithComponent("Success to sign up");
                        Director.instance.playerName = signUpIDInputField.text;
                        startUI.SetAfterSignIn();
                        gameObject.SetActive(false);
                    }
                    else
                    {
                        switch ((SignUpCode)Convert.ToInt32(bro.GetStatusCode()))
                        {
                            case SignUpCode.DuplicatedParameterException:
                                MessageBoxUI.UseWithComponent("Failed to sign up: Duplicated id");
                                break;
                            case SignUpCode.Etc:
                                MessageBoxUI.UseWithComponent("Failed to sign up");
                                break;
                            default:
                                return;
                        }
                    }

                    bro.Clear();
                    success.Dispose();
                }
            });
        }
    }
}