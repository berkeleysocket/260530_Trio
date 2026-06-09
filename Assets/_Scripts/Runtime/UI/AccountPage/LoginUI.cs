using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Runtime.Shared.Core;
using DG.Tweening;

namespace Runtime.UI
{
    public class LoginUI : MonoBehaviour
    {
        [SerializeField] private Button btn_confirm;
        [SerializeField] private Button btn_switchSignupUI;
        [SerializeField] private CanvasGroup signupGroup;
        [SerializeField] private TMP_InputField inputId;
        [SerializeField] private TMP_InputField inputPassword;

        private CanvasGroup _elementGroup;

        private void Awake()
        {
            _elementGroup = GetComponent<CanvasGroup>();

            btn_confirm.onClick.AddListener(OnClickedLoginButton);
            btn_switchSignupUI.onClick.AddListener(OnClickedSwitchButton);
        }

        private void OnClickedLoginButton()
        {
            string id = inputId.text.Trim();
            string password = inputPassword.text.Trim();
            NetworkManager.Instance.Login.CustomLogin(id, password);
        }

        private void OnClickedSwitchButton()
        {
            _elementGroup.interactable = false;
            _elementGroup.DOFade(0f, 0.35f)
                .OnComplete(() => gameObject.SetActive(false));
            signupGroup.gameObject.SetActive(true);
            signupGroup.interactable = true;
            signupGroup.DOFade(1f, 0.35f);
        }
    }
}