using Runtime.Shared.Core;
using Runtime.Utility.EventChannel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class InvitationPopup : Popup
    {
        [SerializeField] private TMP_Text txt_inviteMessage;
        [SerializeField] private Button btn_accept;
        [SerializeField] private Button btn_decline;

        private string _inviterNickname = null;

        public override void OnInitialize()
        {
            base.OnInitialize();

            EventChannel.AddListener<OnMatchMakingRoomSomeoneInvitedEvent>(OnMatchMakingRoomSomeoneInvited);
            EventChannel.AddListener<OnRespondToRoomInvitationCompleteEvent>(OnRespondToRoomInvitationComplete);
            btn_accept.onClick.AddListener(OnClickedAcceptButton);
            btn_decline.onClick.AddListener(OnClickedDeclineButton);
        }

        private void OnMatchMakingRoomSomeoneInvited(OnMatchMakingRoomSomeoneInvitedEvent args)
        {
            this._inviterNickname = args.InviterNickname;
            this.txt_inviteMessage.text = $"{args.InviterNickname}(이)가 당신에게 초대를 보냈습니다!";
        }

        private void OnRespondToRoomInvitationComplete(OnRespondToRoomInvitationCompleteEvent args)
        {
            Hide();
        }

        private void OnClickedAcceptButton()
        {
            NetworkManager.Instance.Lobby.RespondToRoomInvitation(_inviterNickname, true);
        }

        private void OnClickedDeclineButton()
        {
            NetworkManager.Instance.Lobby.RespondToRoomInvitation(_inviterNickname, false);
        }
    }
}