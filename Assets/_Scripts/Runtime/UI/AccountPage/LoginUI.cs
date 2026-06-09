using DG.Tweening;
using Runtime.Networks;
using Runtime.Shared.Core;
using Runtime.Utility.EventChannel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class LoginUI : MonoBehaviour
    {
        [SerializeField] private Button btn_confirm;
        [SerializeField] private Button btn_switchSignUpUI;
        [SerializeField] private CanvasGroup SignUpGroup;
        [SerializeField] private TMP_InputField inputId;
        [SerializeField] private TMP_InputField inputPassword;
        [SerializeField] private ErrorMessageUI errorMessage;

        private CanvasGroup _elementGroup;

        private void Awake()
        {
            _elementGroup = GetComponent<CanvasGroup>();

            EventChannel.AddListener<OnCustomLoginCompleteEvent>(OnLoginComplete);
            EventChannel.AddListener<OnCustomLoginFailedEvent>(OnLoginFailed);
            btn_confirm.onClick.AddListener(OnClickedLoginButton);
            btn_switchSignUpUI.onClick.AddListener(OnClickedSwitchButton);

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

            EventChannel.AddListener<OnCustomLoginCompleteEvent>(OnLoginComplete);
            EventChannel.AddListener<OnCustomLoginFailedEvent>(OnLoginFailed);
            NetworkManager.Instance.Login.CustomLogin(id, password);
        }

        private void OnClickedSwitchButton()
        {
            _elementGroup.interactable = false;
            _elementGroup.DOFade(0f, 0.35f)
                .OnComplete(() => gameObject.SetActive(false));
            SignUpGroup.gameObject.SetActive(true);
            SignUpGroup.interactable = true;
            SignUpGroup.DOFade(1f, 0.35f);
        }

        private void OnLoginComplete(OnCustomLoginCompleteEvent args)
        {
            string message = "로그인에 성공했습니다.";
            Color successColor = Color.green;
            errorMessage.SetMessage(message, successColor, false);
        }

        private void OnLoginFailed(OnCustomLoginFailedEvent args)
        {
            ushort errorCode = args.StatusCode;
            string message = null;
            Color failedColor = Color.red;

            switch ((LoginStatus)errorCode)
            {
                case LoginStatus.DeviceInfoIsNull:
                    {
                        message = "디바이스 정보를 확인할 수 없습니다.";
                        break;
                    }
                case LoginStatus.UnDefined:
                    {
                        message = "존재하지 않는 아이디입니다.";
                        break;
                    }
                case LoginStatus.BlockedDevice:
                    {
                        message = "서버로부터 차단당한 계정입니다.";
                        break;
                    }
                case LoginStatus.WithdrawalInProcess:
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

            errorMessage.SetMessage(message, failedColor, true);
        }
    }
}