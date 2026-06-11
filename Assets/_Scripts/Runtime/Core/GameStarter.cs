using Runtime.InputSystem;
using Runtime.Shared.Core;
using Runtime.UI;
using Runtime.Utility.EventChannel;
using UnityEngine;
using Utility.Debug;

namespace Runtime.Core
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] private TitlePage titlePage;
        [SerializeField] private AccountPage accountPage;
        [SerializeField] private LobbyPage lobbyPage;
        [SerializeField] private InvitationPopup invitationPopup;

        private void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            RegisterEvent();

            accountPage.Show();
        }

        private void Initialize()
        {
            UIManager.Instance.Initialize();
            NetworkManager.Instance.Initialize();
            InputManager.Instance.Initialize();
        }

        private void RegisterEvent()
        {
            EventChannel.AddListener<AnyKeyInputEvent>(OnPressedAnyKey);

            EventChannel.AddListener<OnCustomLoginCompleteEvent>(OnCustomLoginComplete);
            
            EventChannel.AddListener<OnJoinMatchMakingServerCompleteEvent>(OnJoinMatchMakingServerComplete);

            EventChannel.AddListener<OnMatchMakingRoomSomeoneInvitedEvent>(OnMatchMakingRoomSomeoneInvited);
            EventChannel.AddListener<OnRespondToRoomInvitationCompleteEvent>(OnRespondToRoomInvitationComplete);

            EventChannel.AddListener<OnCreateMatchRoomCompleteEvent>(OnCreateMatchRoomComplete);
            EventChannel.AddListener<OnCreateMatchRoomFailedEvent>(OnCreateMatchRoomFailed);
            EventChannel.AddListener<OnLeftMatchRoomCompleteEvent>(OnLeftMatchRoomComplete);
        }
        #region MatchMakingServerEvent
        private void OnJoinMatchMakingServerComplete(OnJoinMatchMakingServerCompleteEvent args)
        {

        }
        #endregion

        #region RoomEvent
        private void OnCreateMatchRoomComplete(OnCreateMatchRoomCompleteEvent args)
        {
            lobbyPage.CreateRoomUI.Hide();
            lobbyPage.RoomManageUI.Show();
            lobbyPage.RoomUserUI.Show();
        }
        private void OnLeftMatchRoomComplete(OnLeftMatchRoomCompleteEvent args)
        {
            lobbyPage.CreateRoomUI.Show();
            lobbyPage.RoomManageUI.Hide();
            lobbyPage.RoomUserUI.Hide();
        }
        private void OnCreateMatchRoomFailed(OnCreateMatchRoomFailedEvent args)
        {

        }
        #endregion

        #region LoginEvent
        private void OnCustomLoginComplete(OnCustomLoginCompleteEvent args)
        {
            accountPage.Hide();
            titlePage.Show();

            InputManager.Instance.EnableReader<UIInputReader>();

            NetworkManager.Instance.Lobby.JoinMatchMakingServer();
        }
        #endregion

        #region InputEvent
        private void OnPressedAnyKey(AnyKeyInputEvent args)
        {
            titlePage.Hide();
            lobbyPage.Show();
            lobbyPage.CreateRoomUI.Show();
            lobbyPage.RoomManageUI.Hide();
            lobbyPage.RoomUserUI.Hide();

            EventChannel.RemoveListener<AnyKeyInputEvent>(OnPressedAnyKey);
        }
        #endregion

        #region InvitationEvent
        private void OnMatchMakingRoomSomeoneInvited(OnMatchMakingRoomSomeoneInvitedEvent args)
        {
            invitationPopup.Show();
        }
        private void OnRespondToRoomInvitationComplete(OnRespondToRoomInvitationCompleteEvent args)
        {
            lobbyPage.CreateRoomUI.Hide();
            lobbyPage.RoomManageUI.Show();
            lobbyPage.RoomUserUI.Show();
        }
        #endregion
    }
}
