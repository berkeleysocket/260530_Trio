using BackEnd;
using System;
using UnityEngine;
using Utility.Debug;

namespace Runtime.Networks
{
    public enum SignUpError
    {
        Success = 200,
    }

    public enum LoginError
    {
        Success = 200,
    }

    public enum NicknameError
    {
        Success = 204,
        InvalidLength = 400,
        Duplicate = 409
    }

    public class LoginService : MonoBehaviour
    {
        private event Action SignUpCompleted;
        private event Action<int> SignUpFailed;

        private event Action LoginCompleted;
        private event Action<int> LoginFailed;

        private event Action UpdateNicknameCompleted;
        private event Action<int> UpdateNicknameFailed;

        private event Action ValidateNicknameCompleted;
        private event Action<int> VaValidateNicknameFailed;

        public void CustomSignup(string id, string password,
            Action signUpCompleted = null, Action<int> signUpFailed = null)
        {
            if (signUpCompleted != null)
                this.SignUpCompleted += signUpCompleted;
            if (signUpFailed != null)
                this.SignUpFailed += signUpFailed;

            Backend.BMember.CustomSignUp(id, password, HandleSignup);
        }

        private void HandleSignup(BackendReturnObject bro)
        {
            if (bro.IsSuccess() && bro.StatusCode == 200)
            {
                CustomLog.LogSuccess("회원 가입에 성공했습니다.");
                SignUpCompleted?.Invoke();
                SignUpCompleted = null;
            }
            else
            {
                CustomLog.LogSuccess("회원 가입에 실패했습니다.");
                SignUpFailed?.Invoke(bro.StatusCode);
                SignUpFailed = null;
            }
        }

        public void CustomLogin(string id, string password,
            Action loginCompleted = null, Action<int> loginFailed = null)
        {
            if (loginCompleted != null)
                this.LoginCompleted += loginCompleted;
            if (loginFailed != null)
                this.LoginFailed += loginFailed;

            Backend.BMember.CustomLogin(id, password, HandleCustomLogin);
        }

        private void HandleCustomLogin(BackendReturnObject bro)
        {
            if(bro.IsSuccess() && bro.StatusCode == 200)
            {
                CustomLog.LogSuccess("로그인에 성공했습니다.");
                LoginCompleted?.Invoke();
                LoginCompleted = null;
            }
            else
            {
                CustomLog.LogError("회원 가입에 실패했습니다.");
                LoginFailed?.Invoke(bro.StatusCode);
                LoginFailed = null;
            }
        }

        public void ValidateNickname(string nickname,
            Action validateNicknameCompleted = null, Action<int> validateNicknameFailed = null)
        {
            if(string.IsNullOrEmpty(nickname) || nickname.Length > 20)
            {
                validateNicknameFailed?.Invoke((int)NicknameError.InvalidLength);
                return;
            }

            Backend.BMember.CheckNicknameDuplication(nickname, HandleValidateNickname);
        }

        private void HandleValidateNickname(BackendReturnObject bro)
        {
            if(bro.IsSuccess() && bro.StatusCode == (int)NicknameError.Success)
            {
                CustomLog.LogSuccess("닉네임 유효성 검사에 성공했습니다.");
                ValidateNicknameCompleted?.Invoke();
                ValidateNicknameCompleted = null;
            }
            else
            {
                CustomLog.LogSuccess("닉네임 유효성 검사에 실패했습니다.");
                VaValidateNicknameFailed?.Invoke(bro.StatusCode);
                VaValidateNicknameFailed = null;
            }
        }

        public void UpdateNickname(string nickname, 
            Action updateNicknameCompleted = null, Action<int> updateNicknameFailed = null)
        {
            if (updateNicknameCompleted != null)
                this.UpdateNicknameCompleted += updateNicknameCompleted;
            if (updateNicknameFailed != null)
                this.UpdateNicknameFailed += updateNicknameFailed;

            Backend.BMember.UpdateNickname(nickname, HandleUpdateNickname);
        }

        private void HandleUpdateNickname(BackendReturnObject bro)
        {
            if(bro.IsSuccess() && bro.StatusCode == 200)
            {
                CustomLog.LogSuccess("닉네임 변경에 성공했습니다.");
                UpdateNicknameCompleted?.Invoke();
                UpdateNicknameCompleted = null;
            }
            else
            {
                CustomLog.LogError("닉네임 변경에 실패했습니다.");
                UpdateNicknameFailed?.Invoke(bro.StatusCode);
                UpdateNicknameFailed = null;
            }
        }
    }
}