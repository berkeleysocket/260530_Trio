using BackEnd;
using System;
using System.Threading.Tasks;
using Utility.Debug;

namespace Runtime.Networks
{
    public enum SignUpError
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

    public enum LoginError
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

    public enum NicknameError
    {
        Success = 204,

        //닉네임의 길이가 20자를 넘어가거나 0이하일 경우
        InvalidLength = 400,

        //닉네임이 중복되었을 경우
        Duplicate = 409
    }

    public class LoginService
    {
        private event Action SignUpCompleted;
        private event Action<int> SignUpFailed;

        private event Action LoginCompleted;
        private event Action<int> LoginFailed;

        private event Action UpdateNicknameCompleted;
        private event Action<int> UpdateNicknameFailed;

        private event Action ValidateNicknameCompleted;
        private event Action<int> ValidateNicknameFailed;

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
            bool isSuccess = bro.IsSuccess();
            if (isSuccess && bro.StatusCode == (int)SignUpError.Success)
            {
                CustomLog.LogSuccess("회원 가입에 성공했습니다.");
                SignUpCompleted?.Invoke();
                SignUpCompleted = null;
            }
            else
            {
                CustomLog.LogError($"회원 가입에 실패했습니다. IsSuccess : {isSuccess}, ErrorCode : {(SignUpError)bro.StatusCode}");
                CustomLog.LogError($"Message : {bro.ErrorMessage}");
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
            if(bro.IsSuccess() && bro.StatusCode == (int)LoginError.Success)
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
            CustomLog.Log($"{Backend.IsInitialized}");
            CustomLog.Log($"{bro.IsSuccess()}, {(NicknameError)bro.StatusCode}, {bro.Code}");
            if(bro.IsSuccess() && bro.StatusCode == (int)NicknameError.Success)
            {
                CustomLog.LogSuccess("닉네임 유효성 검사에 성공했습니다.");
                ValidateNicknameCompleted?.Invoke();
                ValidateNicknameCompleted = null;
            }
            else
            {
                CustomLog.LogError("닉네임 유효성 검사에 실패했습니다.");
                ValidateNicknameFailed?.Invoke(bro.StatusCode);
                ValidateNicknameFailed = null;
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
            if(bro.IsSuccess() && bro.StatusCode == (int)NicknameError.Success)
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