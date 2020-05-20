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
        private GameObject setInfoField;
        [SerializeField]
        private InputField nicknameInputField;
        [SerializeField]
        private Button infoSetButton;
        [SerializeField]
        private Button exitButton;

        private StartSceneDirector startSceneDirector;

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
                        startSceneDirector.signedIn.Value = true;
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

            Director.instance.GetSubDirector<BackendDirector>().CustomSignUp(signUpIDInputField.text, signUpPWInputField.text, SetAfterSignUp, str => MessageBoxUI.UseWithComponent(str));
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

            var success = new ReactiveProperty<bool>();
            var bro = new BackendReturnObject();

            Backend.BMember.CreateNickname(nicknameInputField.text, c =>
            {
                bro = c;
                success.Value = true;
            });

            success.ObserveOnMainThread().Subscribe(value =>
            {
                if (value)
                {
                    var saveToken = Backend.BMember.SaveToken(bro);
                    if(bro.IsSuccess())
                    {
                        MessageBoxUI.UseWithComponent("Success to sign in");
                        Director.instance.GetUserInfo();
                        gameObject.SetActive(false);
                    }
                    else
                    {
                        switch((SetNicknameCode)Convert.ToInt32(bro.GetStatusCode()))
                        {
                            case SetNicknameCode.BadParameterException:
                                MessageBoxUI.UseWithComponent("Failed to set nickname: Nickname doesn't fit");
                                break;
                            case SetNicknameCode.DuplicatedParameterException:
                                MessageBoxUI.UseWithComponent("Failed to set nickname: Duplicated nickname");
                                break;
                            case SetNicknameCode.Etc:
                                MessageBoxUI.UseWithComponent("Failed to set nickname");
                                break;
                        }
                    }

                    bro.Clear();
                    success.Dispose();
                }
            });
        }
    }
}