using DG.Tweening;
using Runtime.Networks;
using Runtime.Shared.Core;
using Runtime.Utility.EventChannel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class SignUpUI : MonoBehaviour
    {
        [SerializeField] private Button btn_confirm;
        [SerializeField] private Button btn_switchLoginUI;
        [SerializeField] private CanvasGroup loginGroup;
        [SerializeField] private TMP_InputField field_name;
        [SerializeField] private TMP_InputField field_id;
        [SerializeField] private TMP_InputField field_password;
        [SerializeField] private ErrorMessageUI errorMessage;

        private CanvasGroup _elementGroup;
        private string inputName;

        private void Awake()
        {
            _elementGroup = GetComponent<CanvasGroup>();

            errorMessage.Initialize();

            btn_confirm.onClick.AddListener(OnClickedSignUpButton);
            btn_switchLoginUI.onClick.AddListener(OnClickedSwitchButton);
        }

        private void OnDisable()
        {
            if (field_name != null)
                field_name.text = string.Empty;
            if (field_id != null)
                field_id.text = string.Empty;
            if (field_password != null)
                field_password.text = string.Empty;
        }

        private void OnClickedSignUpButton()
        {
            string name = field_name.text.Trim();
            string id = field_id.text.Trim();
            string password = field_password.text.Trim();

            this.inputName = name;

            EventChannel.AddListener<OnCustomSignUpCompleteEvent>(OnCustomSignUpComplete);
            EventChannel.AddListener<OnCustomSignUpFailedEvent>(OnCustomSignUpFailed);
            NetworkManager.Instance.Login.CustomSignUp(id, password);
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

        private void OnCustomSignUpComplete(OnCustomSignUpCompleteEvent args)
        {
            EventChannel.AddListener<OnValidateNicknameCompleteEvent>(OnValidateNicknameComplete);
            EventChannel.AddListener<OnValidateNicknameFailedEvent>(OnValidateNicknameFailed);

            NetworkManager.Instance.Login.ValidateNickname(inputName);
        }

        private void OnCustomSignUpFailed(OnCustomSignUpFailedEvent args)
        {
            ushort statusCode = args.StatusCode;
            string message = null;
            Color failedColor = Color.red;

            switch ((SignUpStatus)statusCode)
            {
                case SignUpStatus.DeviceInfoIsNull:
                    {
                        message = "디바이스 정보를 확인할 수 없습니다.";
                        break;
                    }
                case SignUpStatus.ProjectIsMaintenance:
                    {
                        message = "서버가 점검중입니다. 나중에 다시 시도해주세요.";
                        break;
                    }
                case SignUpStatus.BlockedDevice:
                    {
                        message = "서버로부터 차단당한 계정입니다.";
                        break;
                    }
                case SignUpStatus.Duplicate:
                    {
                        message = "중복된 아이디입니다. 다른 아이디로 다시 시도해주세요.";
                        break;
                    }
                default:
                    {
                        message = "계정 생성에 실패했습니다. 다시 시도해주세요.";
                        break;
                    }
            }

            errorMessage.SetMessage(message, failedColor, true);
        }

        private void OnValidateNicknameComplete(OnValidateNicknameCompleteEvent args)
        {
            EventChannel.AddListener<OnUpdateNicknameCompleteEvent>(OnUpdateNicknameComplete);
            EventChannel.AddListener<OnUpdateNicknameFailedEvent>(OnUpdateNicknameFailed);

            NetworkManager.Instance.Login.UpdateNickname(inputName);
        }

        private void OnValidateNicknameFailed(OnValidateNicknameFailedEvent args)
        {
            ushort statusCode = args.StatusCode;
            string message = null;
            Color failedColor = Color.red;

            switch ((UpdateNicknameStatus)statusCode)
            {
                case UpdateNicknameStatus.InvalidLength:
                    {
                        message = "닉네임이 너무 짧거나 깁니다. 2자 이상 20자 이하로 해주세요.";
                        break;
                    }
                case UpdateNicknameStatus.Duplicate:
                    {
                        message = "이미 존재하는 닉네임입니다. 다른 닉네임으로 시도해주세요.";
                        break;
                    }
                default:
                    {
                        message = "닉네임 생성에 실패했습니다. 다시 시도해주세요.";
                        break;
                    }
            }

            errorMessage.SetMessage(message, failedColor, true);
        }

        private void OnUpdateNicknameComplete(OnUpdateNicknameCompleteEvent args) 
        {
            string message = "계정 생성에 성공했습니다!";
            Color successColor = Color.green;
            errorMessage.SetMessage(message, successColor, false);
        }

        private void OnUpdateNicknameFailed(OnUpdateNicknameFailedEvent args)
        {
            ushort statusCode = args.StatusCode;
            string message = null;
            Color failedColor = Color.red;

            switch((UpdateNicknameStatus)statusCode)
            {
                case UpdateNicknameStatus.InvalidLength:
                    {
                        message = "닉네임이 너무 짧거나 깁니다. 2자 이상 20자 이하로 해주세요.";
                        break;
                    }
                case UpdateNicknameStatus.Duplicate:
                    {
                        message = "이미 존재하는 닉네임입니다. 다른 닉네임으로 시도해주세요.";
                        break;
                    }
                default:
                    {
                        message = "닉네임 생성에 실패했습니다. 다시 시도해주세요.";
                        break;
                    }
            }

            errorMessage.SetMessage(message, failedColor, true);
        }
    }
}