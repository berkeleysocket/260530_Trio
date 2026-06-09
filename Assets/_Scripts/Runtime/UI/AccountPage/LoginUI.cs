using Codice.CM.Common;
using DG.Tweening;
using Runtime.Networks;
using Runtime.Shared.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class LoginUI : MonoBehaviour
    {
        [SerializeField] private Button btn_confirm;
        [SerializeField] private Button btn_switchSignupUI;
        [SerializeField] private CanvasGroup signupGroup;
        [SerializeField] private TMP_InputField inputId;
        [SerializeField] private TMP_InputField inputPassword;
        [SerializeField] private ErrorMessageUI errorMessage;

        private CanvasGroup _elementGroup;

        private void Awake()
        {
            _elementGroup = GetComponent<CanvasGroup>();

            btn_confirm.onClick.AddListener(OnClickedLoginButton);
            btn_switchSignupUI.onClick.AddListener(OnClickedSwitchButton);

            errorMessage.Initialize();
        }

        private void OnDisable()
        {
            if (inputId != null)
                inputId.text = string.Empty;
            if (inputPassword != null)
                inputPassword.text = string.Empty;
        }

        private void OnClickedLoginButton()
        {
            string id = inputId.text.Trim();
            string password = inputPassword.text.Trim();
            NetworkManager.Instance.Login.CustomLogin(id, password, OnLoginComplete, OnLoginFailed);
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

        private void OnLoginComplete()
        {
            string message = "로그인에 성공했습니다.";
            Color successColor = Color.green;
            errorMessage.SetMessage(message, successColor);
        }

        private void OnLoginFailed(ushort errorCode)
        {
            string message = null;
            Color failedColor = Color.red;

            switch ((LoginError)errorCode)
            {
                case LoginError.DeviceInfoIsNull:
                    {
                        message = "디바이스 정보를 확인할 수 없습니다.";
                        break;
                    }
                case LoginError.UnDefined:
                    {
                        message = "존재하지 않는 아이디입니다.";
                        break;
                    }
                case LoginError.BlockedDevice:
                    {
                        message = "서버로부터 차단당한 계정입니다.";
                        break;
                    }
                case LoginError.WithdrawalInProcess:
                    {
                        message = "삭제가 진행중인 계정입니다.";
                        break;
                    }
                default:
                    {
                        message = "로그인에 실패했습니다. 다시 시도해주세요.";
                        break;
                    }
            }

            errorMessage.SetMessage(message, failedColor);
        }
    }
}