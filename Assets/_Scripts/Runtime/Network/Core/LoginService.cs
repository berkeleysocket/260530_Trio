using BackEnd;
using LitJson;
using Runtime.Utility.EventChannel;
using Utility.Debug;

namespace Runtime.Networks
{
    public enum SignUpStatus : ushort
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

    public enum LoginStatus : ushort
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

    public enum UpdateNicknameStatus : ushort
    {
        Success = 204,

        //닉네임의 길이가 20자를 넘어가거나 0이하일 경우
        InvalidLength = 400,

        //닉네임이 중복되었을 경우
        Duplicate = 409
    }

    public class LoginService
    {
        public void CustomSignUp(string id, string password)
        {
            Backend.BMember.CustomSignUp(id, password, HandleSignUp);
        }

        private void HandleSignUp(BackendReturnObject bro)
        {
            bool isSuccess = bro.IsSuccess();
            ushort statusCode = (ushort)bro.StatusCode;
            if (isSuccess && statusCode == (ushort)SignUpStatus.Success)
            {
                CustomLog.LogSuccess("회원 가입에 성공했습니다.");
                EventChannel.InvokeEvent(new OnCustomSignUpCompleteEvent());
            }
            else
            {
                CustomLog.LogError($"회원 가입에 실패했습니다. IsSuccess : {isSuccess}, ErrorCode : {(SignUpStatus)bro.StatusCode}");
                CustomLog.LogError($"Message : {bro.ErrorMessage}");
                EventChannel.InvokeEvent(new OnCustomSignUpFailedEvent(statusCode));
            }
        }

        public void CustomLogin(string id, string password)
        {
            Backend.BMember.CustomLogin(id, password, HandleCustomLogin);
        }

        private void HandleCustomLogin(BackendReturnObject bro)
        {
            bool isSuccess = bro.IsSuccess();
            if (isSuccess && bro.StatusCode == (ushort)LoginStatus.Success)
            {
                CustomLog.LogSuccess($"로그인에 성공했습니다. IsSuccess : {isSuccess}, ErrorCode : {(LoginStatus)bro.StatusCode}");
                string myNickname = Backend.UserNickName;

                EventChannel.InvokeEvent(new OnCustomLoginCompleteEvent(myNickname));
            }
            else
            {
                CustomLog.LogError($"로그인에 실패했습니다. IsSuccess : {isSuccess}, ErrorCode : {(LoginStatus)bro.StatusCode}");
                EventChannel.InvokeEvent(new OnCustomLoginFailedEvent((ushort)bro.StatusCode));
            }
        }

        public void ValidateNickname(string nickname)
        {
            if (string.IsNullOrEmpty(nickname) || nickname.Length > 20)
            {
                EventChannel.InvokeEvent(new OnValidateNicknameFailedEvent((ushort)UpdateNicknameStatus.InvalidLength));
                return;
            }

            Backend.BMember.CheckNicknameDuplication(nickname, HandleValidateNickname);
        }

        private void HandleValidateNickname(BackendReturnObject bro)
        {
            ushort statusCode = (ushort)bro.StatusCode;
            if (bro.IsSuccess() && statusCode == (ushort)UpdateNicknameStatus.Success)
            {
                CustomLog.LogSuccess("닉네임 유효성 검사에 성공했습니다.");
                EventChannel.InvokeEvent(new OnValidateNicknameCompleteEvent());
            }
            else
            {
                CustomLog.LogError("닉네임 유효성 검사에 실패했습니다.");
                EventChannel.InvokeEvent(new OnValidateNicknameFailedEvent(statusCode));
            }
        }

        public void UpdateNickname(string nickname)
        {
            Backend.BMember.UpdateNickname(nickname, HandleUpdateNickname);
        }

        private void HandleUpdateNickname(BackendReturnObject bro)
        {
            ushort statusCode = (ushort)bro.StatusCode;
            if (bro.IsSuccess() && statusCode == (ushort)UpdateNicknameStatus.Success)
            {
                CustomLog.LogSuccess("닉네임 변경에 성공했습니다.");
                EventChannel.InvokeEvent(new OnUpdateNicknameCompleteEvent());
            }
            else
            {
                CustomLog.LogError("닉네임 변경에 실패했습니다.");
                EventChannel.InvokeEvent(new OnUpdateNicknameFailedEvent(statusCode));
            }
        }
    }
}