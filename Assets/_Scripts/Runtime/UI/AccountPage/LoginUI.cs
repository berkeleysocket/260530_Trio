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
        [SerializeField] private TMP_InputField inputId;
        [SerializeField] private TMP_InputField inputPassword;

        private void Awake()
        {
            btn_login.onClick.AddListener(OnClickedLoginButton);
        }

        private void OnClickedLoginButton()
        {
            string id = inputId.text.Trim();
            string password = inputPassword.text.Trim();
            NetworkManager.Instance.Login.CustomLogin(id, password);
        }
    }
}