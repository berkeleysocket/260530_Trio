using DG.Tweening;
using Runtime.Shared.Core;
using Runtime.Utility.EventChannel;
using System;
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

        public override void Initialize()
        {
            base.Initialize();

            EventChannel.AddListener<OnMatchMakingRoomSomeoneInvitedEvent>(HandleMatchMakingRoomSomeoneInvited);
            btn_accept.onClick.AddListener(() => NetworkManager.Instance.Lobby.RespondToRoomInvitation(_inviterNickname, true));
            btn_decline.onClick.AddListener(() => NetworkManager.Instance.Lobby.RespondToRoomInvitation(_inviterNickname, false));
        }

        public void HandleMatchMakingRoomSomeoneInvited(OnMatchMakingRoomSomeoneInvitedEvent args)
        {
            this._inviterNickname = args.InviterNickname;
            this.txt_inviteMessage.text = $"{args.InviterNickname}(이)가 당신에게 초대를 보냈습니다!";
        }
    }
}