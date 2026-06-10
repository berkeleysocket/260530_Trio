using Runtime.Shared.Core;
using Runtime.Utility.EventChannel;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class RoomPage : Page
    {
        [SerializeField] private VisitorElementUI userElementPrefab;
        [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;

        [SerializeField] private ErrorMessageUI errorMessage;
        [SerializeField] private TMP_InputField inputName;
        [SerializeField] private Button btn_invite;

        private List<VisitorElementUI> _visitors;

        public override void Initialize()
        {
            base.Initialize();

            _visitors = new List<VisitorElementUI>();

            EventChannel.AddListener<OnMatchMakingRoomInviteCompleteEvent>(OnMatchMakingRoomInviteComplete);
            EventChannel.AddListener<OnMatchMakingRoomInviteFailedEvent>(OnMatchMakingRoomInviteFailed);
            EventChannel.AddListener<OnMatchMakingRoomJoinedEvent>(OnMatchMakingRoomJoined);
            EventChannel.AddListener<OnMatchMakingRoomLeftEvent>(OnMatchMakingRoomLeft);

            btn_invite.onClick.AddListener(OnClickedInviteButton);

            errorMessage.Initialize();
        }

        private void OnClickedInviteButton()
        {
            string userName = inputName.text.Trim();

            NetworkManager.Instance.Lobby.InviteUser(userName);  
        }

        private void OnMatchMakingRoomJoined(OnMatchMakingRoomJoinedEvent args)
        {
            VisitorElementUI visitor = Instantiate(userElementPrefab, verticalLayoutGroup.transform);
            visitor.Initialize(args.VisitorNickname);
            _visitors.Add(visitor);
        }

        private void OnMatchMakingRoomLeft(OnMatchMakingRoomLeftEvent args)
        {
            string leaverNickname = args.UserInfo.m_nickName;
            int index = _visitors.FindIndex((visitor) => visitor.VisitorNickname == leaverNickname);
            var leaver = _visitors[index];
            Destroy(leaver);
            _visitors.RemoveAt(index);
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