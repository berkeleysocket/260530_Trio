using DG.Tweening;
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
        [SerializeField] private Button btn_switchLoginUI;
        [SerializeField] private CanvasGroup loginGroup;
        [SerializeField] private TMP_InputField inputName;
        [SerializeField] private TMP_InputField inputId;
        [SerializeField] private TMP_InputField inputPassword;

        private CanvasGroup _elementGroup;

        private void Awake()
        {
            _elementGroup = GetComponent<CanvasGroup>();

            btn_signup.onClick.AddListener(OnClickedSignupButton);
            btn_switchLoginUI.onClick.AddListener(OnClickedSwitchButton);
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

        private void OnClickedSwitchButton()
        {
            _elementGroup.interactable = false;
            _elementGroup.DOFade(0f, 0.35f)
                .OnComplete(()=>gameObject.SetActive(false));
            loginGroup.gameObject.SetActive(true);
            loginGroup.interactable = true;
            loginGroup.DOFade(1f, 0.35f);
        }
    }
}
