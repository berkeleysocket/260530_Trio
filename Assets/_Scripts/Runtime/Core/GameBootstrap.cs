using Runtime.Shared.Core;
using Runtime.InputSystem;
using UnityEngine;
using Runtime.UI;
using Runtime.Utility.EventChannel;

namespace Runtime.Core
{
    public class GameBootstrap : MonoBehaviour
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
        }

        private void OnCustomLoginComplete(OnCustomLoginCompleteEvent args)
        {
            accountPage.FadeOut();
            InputManager.Instance.EnableReader<UIInputReader>();
        }

        private void OnPressedAnyKey(AnyKeyInputEvent args)
        {
            titlePage.FadeOut();
            EventChannel.RemoveListener<AnyKeyInputEvent>(OnPressedAnyKey);
            EventChannel.AddListener<OnJoinMatchMakingServerCompleteEvent>(OnJoinMatchMakingServerComplete);
            NetworkManager.Instance.Lobby.JoinMatchMakingServer();
        }

        private void OnJoinMatchMakingServerComplete(OnJoinMatchMakingServerCompleteEvent args)
        {
            EventChannel.RemoveListener<OnJoinMatchMakingServerCompleteEvent>(OnJoinMatchMakingServerComplete);
            EventChannel.AddListener<OnCreateMatchRoomCompleteEvent>(OnCreateMatchRoomComplete);
            EventChannel.AddListener<OnCreateMatchRoomFailedEvent>(OnCreateMatchRoomFailed);
            NetworkManager.Instance.Lobby.CreateMatchRoom();
        }

        private void OnCreateMatchRoomComplete(OnCreateMatchRoomCompleteEvent args)
        {
            roomPage.FadeIn();
        }

        private void OnCreateMatchRoomFailed(OnCreateMatchRoomFailedEvent args)
        {

        }
    }
}
