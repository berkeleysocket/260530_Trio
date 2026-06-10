using Runtime.Shared.Core;
using Runtime.InputSystem;
using UnityEngine;
using Runtime.UI;
using Runtime.Utility.EventChannel;
using Runtime.Pattern;
using Utility.Debug;

namespace Runtime.Core
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] Canvas canvas;
        [SerializeField] private Page titlePage;
        [SerializeField] private Page accountPage;
        [SerializeField] private Page roomPage;
        [SerializeField] private Popup invitationPopup;

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
            IInitializable[] InitializableUIs = canvas.GetComponentsInChildren<IInitializable>(true);
            foreach (IInitializable element in InitializableUIs)
                element.Initialize();

            NetworkManager.Instance.Initialize();
            InputManager.Instance.Initialize();

            CustomLog.LogSuccess("GameStarter Initialize");
        }

        private void RegisterEvent()
        {
            EventChannel.AddListener<OnCustomLoginCompleteEvent>(OnCustomLoginComplete);
            EventChannel.AddListener<OnMatchMakingRoomSomeoneInvitedEvent>(OnMatchMakingRoomSomeoneInvited);
            EventChannel.AddListener<AnyKeyInputEvent>(OnPressedAnyKey);
        }

        private void OnCustomLoginComplete(OnCustomLoginCompleteEvent args)
        {
            accountPage.Hide();
            titlePage.Show();

            InputManager.Instance.EnableReader<UIInputReader>();

            EventChannel.AddListener<OnJoinMatchMakingServerCompleteEvent>(OnJoinMatchMakingServerComplete);

            NetworkManager.Instance.Lobby.JoinMatchMakingServer();
        }

        private void OnPressedAnyKey(AnyKeyInputEvent args)
        {
            titlePage.Hide();

            EventChannel.RemoveListener<AnyKeyInputEvent>(OnPressedAnyKey);
            EventChannel.RemoveListener<OnJoinMatchMakingServerCompleteEvent>(OnJoinMatchMakingServerComplete);

            EventChannel.AddListener<OnCreateMatchRoomCompleteEvent>(OnCreateMatchRoomComplete);
            EventChannel.AddListener<OnCreateMatchRoomFailedEvent>(OnCreateMatchRoomFailed);

            NetworkManager.Instance.Lobby.CreateMatchRoom();
        }

        private void OnJoinMatchMakingServerComplete(OnJoinMatchMakingServerCompleteEvent args)
        {
            EventChannel.RemoveListener<OnJoinMatchMakingServerCompleteEvent>(OnJoinMatchMakingServerComplete);
        }

        private void OnCreateMatchRoomComplete(OnCreateMatchRoomCompleteEvent args)
        {
            EventChannel.RemoveListener<OnCreateMatchRoomCompleteEvent>(OnCreateMatchRoomComplete);
            EventChannel.RemoveListener<OnCreateMatchRoomFailedEvent>(OnCreateMatchRoomFailed);

            roomPage.Show();
        }

        private void OnCreateMatchRoomFailed(OnCreateMatchRoomFailedEvent args)
        {

        }

        private void OnMatchMakingRoomSomeoneInvited(OnMatchMakingRoomSomeoneInvitedEvent args)
        {
            invitationPopup.Show();
        }
    }
}
