using Runtime.Shared.Core;
using Runtime.Utility.EventChannel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility.Debug;

namespace Runtime.UI
{
    public class RoomManageUI : MonoBehaviour, IWindowElement
    {
        [SerializeField] private ErrorMessageUI errorMessage;
        [SerializeField] private Button btn_exit;
        [SerializeField] private Button btn_invite;
        [SerializeField] private TMP_InputField inputName;

        public void Initialize()
        {
            errorMessage.Initialize();

            btn_invite.onClick.AddListener(OnClickedInviteButton);
            btn_exit.onClick.AddListener(OnClickedExitButton);
            EventChannel.AddListener<OnMatchMakingRoomInviteCompleteEvent>(OnMatchMakingRoomInviteComplete);
            EventChannel.AddListener<OnMatchMakingRoomInviteFailedEvent>(OnMatchMakingRoomInviteFailed);
        }

        private void OnClickedInviteButton()
        {
            string userName = inputName.text.Trim();

            NetworkManager.Instance.Lobby.InviteUser(userName);
        }

        private void OnClickedExitButton()
        {
            CustomLog.LogError("방에서 나가는 처리 구현하기");
        }

        private void OnMatchMakingRoomInviteComplete(OnMatchMakingRoomInviteCompleteEvent args)
        {
            string message = "초대 전송에 성공했습니다.";
            Color color = Color.green;
            inputName.text = string.Empty;
            errorMessage.SetMessage(message, color, false);
        }

        private void OnMatchMakingRoomInviteFailed(OnMatchMakingRoomInviteFailedEvent args)
        {
            string message = $"초대 전송에 실패했습니다. ERROR : {args.ErrorCode}";
            Color color = Color.red;
            inputName.text = string.Empty;
            errorMessage.SetMessage(message, color, true);
        }
    }
}
