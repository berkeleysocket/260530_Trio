using BackEnd;
using System;
using Utility.Debug;

namespace Runtime.Networks
{
    public enum SignUpError : ushort
    {
        Success = 201,

        //디바이스 정보가 null일 경우
        DeviceInfoIsNull = 400,

        //프로젝트가 점검중일 경우
        ProjectIsMaintenance = 401,
        
        //차단되었을 경우
        BlockedDevice = 403,

        //아이디가 중복되었을 경우
        Duplicate = 409
    }

    public enum LoginError : ushort
    {
        Success = 200,

        //디바이스 정보가 null일 경우
        DeviceInfoIsNull = 400,

        //아이디/비밀번호가 틀렸을 경우 (없을 경우)
        UnDefined = 401,

        //차단되었을 경우
        BlockedDevice = 403,

        //탈퇴중일 경우
        WithdrawalInProcess = 410
    }

    public enum NicknameError : ushort
    {
        Success = 204,

        //닉네임의 길이가 20자를 넘어가거나 0이하일 경우
        InvalidLength = 400,

        //닉네임이 중복되었을 경우
        Duplicate = 409
    }

    public class LoginService
    {
        private event Action OnSignUpComplete;
        private event Action<ushort> SignUpFailed;

        private event Action OnLoginComplete;
        private event Action<ushort> LoginFailed;

        private event Action OnUpdateNicknameComplete;
        private event Action<ushort> UpdateNicknameFailed;

        private event Action OnValidateNicknameComplete;
        private event Action<ushort> ValidateNicknameFailed;

        public void CustomSignup(string id, string password,
            Action onCompleted = null, Action<ushort> onFailed = null)
        {
            if (onCompleted != null)
                this.OnSignUpComplete += onCompleted;
            if (onFailed != null)
                this.SignUpFailed += onFailed;

            Backend.BMember.CustomSignUp(id, password, HandleSignup);
        }

        private void HandleSignup(BackendReturnObject bro)
        {
            bool isSuccess = bro.IsSuccess();
            if (isSuccess && bro.StatusCode == (ushort)SignUpError.Success)
            {
                CustomLog.LogSuccess("회원 가입에 성공했습니다.");
                OnSignUpComplete?.Invoke();
                OnSignUpComplete = null;
            }
            else
            {
                CustomLog.LogError($"회원 가입에 실패했습니다. IsSuccess : {isSuccess}, ErrorCode : {(SignUpError)bro.StatusCode}");
                CustomLog.LogError($"Message : {bro.ErrorMessage}");
                SignUpFailed?.Invoke((ushort)bro.StatusCode);
                SignUpFailed = null;
            }
        }

        public void CustomLogin(string id, string password,
            Action onCompleted = null, Action<ushort> onLoginFailed = null)
        {
            if (onCompleted != null)
                this.OnLoginComplete += onCompleted;
            if (onLoginFailed != null)
                this.LoginFailed += onLoginFailed;

            Backend.BMember.CustomLogin(id, password, HandleCustomLogin);
        }

        private void HandleCustomLogin(BackendReturnObject bro)
        {
            bool isSuccess = bro.IsSuccess();
            if (isSuccess && bro.StatusCode == (ushort)LoginError.Success)
            {
                CustomLog.LogSuccess($"로그인에 성공했습니다. IsSuccess : {isSuccess}, ErrorCode : {(LoginError)bro.StatusCode}");
                OnLoginComplete?.Invoke();
                OnLoginComplete = null;
            }
            else
            {
                CustomLog.LogError($"로그인에 실패했습니다. IsSuccess : {isSuccess}, ErrorCode : {(LoginError)bro.StatusCode}");
                LoginFailed?.Invoke((ushort)bro.StatusCode);
                LoginFailed = null;
            }
        }

        public void ValidateNickname(string nickname,
            Action onCompleted = null, Action<ushort> onFailed = null)
        {
            if(onCompleted != null)
                this.OnValidateNicknameComplete += onCompleted;
            if (onFailed != null)
                this.ValidateNicknameFailed += onFailed;

            if (string.IsNullOrEmpty(nickname) || nickname.Length > 20)
            {
                onFailed?.Invoke((ushort)NicknameError.InvalidLength);
                return;
            }

            Backend.BMember.CheckNicknameDuplication(nickname, HandleValidateNickname);
        }

        private void HandleValidateNickname(BackendReturnObject bro)
        {
            if(bro.IsSuccess() && bro.StatusCode == (ushort)NicknameError.Success)
            {
                CustomLog.LogSuccess("닉네임 유효성 검사에 성공했습니다.");
                OnValidateNicknameComplete?.Invoke();
                OnValidateNicknameComplete = null;
            }
            else
            {
                CustomLog.LogError("닉네임 유효성 검사에 실패했습니다.");
                ValidateNicknameFailed?.Invoke((ushort)bro.StatusCode);
                ValidateNicknameFailed = null;
            }
        }

        public void UpdateNickname(string nickname, 
            Action onCompleted = null, Action<ushort> onFailed = null)
        {
            if (onCompleted != null)
                this.OnUpdateNicknameComplete += onCompleted;
            if (onFailed != null)
                this.UpdateNicknameFailed += onFailed;

            Backend.BMember.UpdateNickname(nickname, HandleUpdateNickname);
        }

        private void HandleUpdateNickname(BackendReturnObject bro)
        {
            if(bro.IsSuccess() && bro.StatusCode == (ushort)NicknameError.Success)
            {
                CustomLog.LogSuccess("닉네임 변경에 성공했습니다.");
                OnUpdateNicknameComplete?.Invoke();
                OnUpdateNicknameComplete = null;
            }
            else
            {
                CustomLog.LogError("닉네임 변경에 실패했습니다.");
                UpdateNicknameFailed?.Invoke((ushort)bro.StatusCode);
                UpdateNicknameFailed = null;
            }
        }
    }
}