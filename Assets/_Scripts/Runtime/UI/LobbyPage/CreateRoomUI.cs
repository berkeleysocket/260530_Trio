using Runtime.Shared.Core;
using Runtime.Utility.EventChannel;
using UnityEngine;
using UnityEngine.UI;
using Utility.Debug;

namespace Runtime.UI
{
    public class CreateRoomUI : WindowElementDirector
    {
        [SerializeField] private ErrorMessageUI errorMessage;
        [SerializeField] private Button btn_createRoom;

        private void OnDisable()
        {
            EventChannel.RemoveListener<OnCreateMatchRoomFailedEvent>(OnCreateMatchRoomFailed);
            btn_createRoom.onClick.RemoveListener(OnClickedCreateRoomButton);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            errorMessage.Initialize();

            EventChannel.AddListener<OnCreateMatchRoomFailedEvent>(OnCreateMatchRoomFailed);
            btn_createRoom.onClick.AddListener(OnClickedCreateRoomButton);
        }

        private void OnClickedCreateRoomButton()
        {
            CustomLog.Log("ClickCreateRoom");
            NetworkManager.Instance.Lobby.CreateMatchRoom();
        }

        private void OnCreateMatchRoomFailed(OnCreateMatchRoomFailedEvent args)
        {
            errorMessage.SetMessage($"방 생성에 실패했습니다. ERROR : {args.ErrorCode}");
        }
    }
}
