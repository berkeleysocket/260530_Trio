using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Runtime.Shared.Core;
using Utility.Debug;
using Runtime.Networks;
using System;

namespace Runtime.UI
{
    public class LoginUI : MonoBehaviour
    {
        [SerializeField] private Button btn_login;
        [SerializeField] private Button btn_signup;
        [SerializeField] private TMP_InputField inputName;
        [SerializeField] private TMP_InputField inputId;
        [SerializeField] private TMP_InputField inputPassword;

        private void Awake()
        {
            btn_login.onClick.AddListener(OnClickedLoginButton);
            btn_signup.onClick.AddListener(OnClickedSignupButton);
        }

        private void OnClickedLoginButton()
        {
            string id = inputId.text.Trim();
            string password = inputName.text.Trim();
            NetworkManager.Instance.Login.CustomLogin(id, password);
        }

        private void OnClickedSignupButton()
        {
            string name = inputName.text.Trim();
            string id = inputId.text.Trim();
            string password = inputName.text.Trim();
            CustomLog.Log($"Try Signup {name}, {id}, {password}");
            Action onSignupCompleted = null;
            Action onValidateNicknameCompleted = null;
            onSignupCompleted += () => NetworkManager.Instance.Login.ValidateNickname(name, onValidateNicknameCompleted);
            onValidateNicknameCompleted += () => NetworkManager.Instance.Login.UpdateNickname(name);
            NetworkManager.Instance.Login.CustomSignup(id, password, onSignupCompleted);
        }
    }
}