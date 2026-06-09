using Runtime.Shared.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd.Tcp;

namespace Runtime.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class RoomPage : Page
    {
        [SerializeField] private MatchMakingUserElementUI userElementPrefab;
        [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;
        [SerializeField] private Button btn_invite;
        [SerializeField] private TMP_InputField inputName;

        private List<MatchMakingUserElementUI> _visitors;

        private void Awake()
        {
            Initialize();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            
            _visitors = new List<MatchMakingUserElementUI>();

            btn_invite.onClick.AddListener(OnClickedInviteButton);
        }

        private void OnClickedInviteButton()
        {
            string userName = inputName.text.Trim();

            NetworkManager.Instance.Lobby.MatchMakingRoomJoined += HandleMatchMakingRoomJoined;
            NetworkManager.Instance.Lobby.InviteUser(userName);  
        }

        private void HandleMatchMakingRoomJoined(MatchMakingUserInfo user)
        {
            var visitor = Instantiate(userElementPrefab, verticalLayoutGroup.transform);

            _visitors.Add(visitor);
        }
    }
}