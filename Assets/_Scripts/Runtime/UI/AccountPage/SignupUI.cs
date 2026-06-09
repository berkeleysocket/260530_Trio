using DG.Tweening;
using Runtime.Networks;
using Runtime.Shared.Core;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility.Debug;

namespace Runtime.UI
{
    public class SignupUI : MonoBehaviour
    {
        [SerializeField] private Button btn_confirm;
        [SerializeField] private Button btn_switchLoginUI;
        [SerializeField] private CanvasGroup loginGroup;
        [SerializeField] private TMP_InputField inputName;
        [SerializeField] private TMP_InputField inputId;
        [SerializeField] private TMP_InputField inputPassword;
        [SerializeField] private ErrorMessageUI errorMessage;

        private CanvasGroup _elementGroup;

        private void Awake()
        {
            _elementGroup = GetComponent<CanvasGroup>();

            errorMessage.Initialize();

            btn_confirm.onClick.AddListener(OnClickedSignupButton);
            btn_switchLoginUI.onClick.AddListener(OnClickedSwitchButton);
        }

        private void OnDisable()
        {
            if (inputName != null)
                inputName.text = string.Empty;
            if (inputId != null)
                inputId.text = string.Empty;
            if (inputPassword != null)
                inputPassword.text = string.Empty;
        }

        private void OnClickedSignupButton()
        {
            string name = inputName.text.Trim();
            string id = inputId.text.Trim();
            string password = inputPassword.text.Trim();

            NetworkManager.Instance.Login.CustomSignup(id, password, OnCustomSignupComplete, OnCustomSignupFailed);
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

        private void OnCustomSignupComplete()
        {
            NetworkManager.Instance.Login.ValidateNickname(name, OnValidateNicknameComplete, OnValidateNicknameFailed);
        }

        private void OnCustomSignupFailed(ushort errorCode)
        {
            string message = null;
            Color failedColor = Color.red;

            switch ((SignUpError)errorCode)
            {
                case SignUpError.DeviceInfoIsNull:
                    {
                        message = "디바이스 정보를 확인할 수 없습니다.";
                        break;
                    }
                case SignUpError.ProjectIsMaintenance:
                    {
                        message = "서버가 점검중입니다. 나중에 다시 시도해주세요.";
                        break;
                    }
                case SignUpError.BlockedDevice:
                    {
                        message = "서버로부터 차단당한 계정입니다.";
                        break;
                    }
                case SignUpError.Duplicate:
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

            errorMessage.SetMessage(message, failedColor);
        }

        private void OnValidateNicknameComplete()
        {
            NetworkManager.Instance.Login.UpdateNickname(name, OnUpdateNicknameComplete, OnUpdateNicknameFailed);
        }

        private void OnValidateNicknameFailed(ushort errorCode)
        {
            string message = null;
            Color failedColor = Color.red;

            switch ((NicknameError)errorCode)
            {
                case NicknameError.InvalidLength:
                    {
                        message = "닉네임이 너무 짧거나 깁니다. 2자 이상 20자 이하로 해주세요.";
                        break;
                    }
                case NicknameError.Duplicate:
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

            errorMessage.SetMessage(message, failedColor);
        }

        private void OnUpdateNicknameComplete() { }

        private void OnUpdateNicknameFailed(ushort errorCode)
        {
            string message = null;
            Color failedColor = Color.red;

            switch((NicknameError)errorCode)
            {
                case NicknameError.InvalidLength:
                    {
                        message = "닉네임이 너무 짧거나 깁니다. 2자 이상 20자 이하로 해주세요.";
                        break;
                    }
                case NicknameError.Duplicate:
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

            errorMessage.SetMessage(message, failedColor);
        }
    }
}