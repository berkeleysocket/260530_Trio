using Runtime.Shared.Core;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class SignupUI : MonoBehaviour
    {
        [SerializeField] private Button btn_signup;
        [SerializeField] private Button link_login;
        [SerializeField] private TMP_InputField inputName;
        [SerializeField] private TMP_InputField inputId;
        [SerializeField] private TMP_InputField inputPassword;

        private void Awake()
        {
            btn_signup.onClick.AddListener(OnClickedSignupButton);
            link_login.onClick.AddListener(OnClickedLoginLink);
        }

        private void OnClickedLoginLink()
        {

        }

        private void OnClickedSignupButton()
        {
            string name = inputName.text.Trim();
            string id = inputId.text.Trim();
            string password = inputName.text.Trim();
            Action onSignupCompleted = null;
            Action onValidateNicknameCompleted = null;
            onSignupCompleted += () => NetworkManager.Instance.Login.ValidateNickname(name, onValidateNicknameCompleted);
            onValidateNicknameCompleted += () => NetworkManager.Instance.Login.UpdateNickname(name);
            NetworkManager.Instance.Login.CustomSignup(id, password, onSignupCompleted);
        }
    }
}
