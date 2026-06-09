using Runtime.Shared.Core;
using Runtime.InputSystem;
using UnityEngine;
using Runtime.UI;
using Runtime.Utility.EventChannel;

namespace Runtime.Core
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] private Page titlePage;
        [SerializeField] private Page accountPage;
        [SerializeField] private Page roomPage;

        private void Awake()
        {
            NetworkManager.Instance.Initialize();
            InputManager.Instance.Initialize();
        }

        private void Start()
        {
            EventChannel.AddListener<OnCustomLoginCompleteEvent>(OnCustomLoginComplete);
            EventChannel.AddListener<AnyKeyInputEvent>(OnPressedAnyKey);

            accountPage.Show(false);
        }

        private void OnCustomLoginComplete(OnCustomLoginCompleteEvent args)
        {
            accountPage.Hide(true);
            titlePage.Show(true);
            InputManager.Instance.EnableReader<UIInputReader>();
        }

        private void OnPressedAnyKey(AnyKeyInputEvent args)
        {
            EventChannel.RemoveListener<AnyKeyInputEvent>(OnPressedAnyKey);
            EventChannel.AddListener<OnJoinMatchMakingServerCompleteEvent>(OnJoinMatchMakingServerComplete);
            NetworkManager.Instance.Lobby.JoinMatchMakingServer();
        }

        private void OnJoinMatchMakingServerComplete(OnJoinMatchMakingServerCompleteEvent args)
        {
            titlePage.Hide(true);
            EventChannel.RemoveListener<OnJoinMatchMakingServerCompleteEvent>(OnJoinMatchMakingServerComplete);
            EventChannel.AddListener<OnCreateMatchRoomCompleteEvent>(OnCreateMatchRoomComplete);
            EventChannel.AddListener<OnCreateMatchRoomFailedEvent>(OnCreateMatchRoomFailed);
            NetworkManager.Instance.Lobby.CreateMatchRoom();
        }

        private void OnCreateMatchRoomComplete(OnCreateMatchRoomCompleteEvent args)
        {
            roomPage.Show(true);
        }

        private void OnCreateMatchRoomFailed(OnCreateMatchRoomFailedEvent args)
        {

        }
    }
}
