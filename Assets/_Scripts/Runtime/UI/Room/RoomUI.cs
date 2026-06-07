using Runtime.Networks;
using Utility.Debug;
using Runtime.Shared.Core;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Runtime.UI
{
    public class RoomUI : MonoBehaviour
    {
        [SerializeField] private GameObject userElementPrefab;
        [SerializeField] private VerticalLayoutGroup visitors;
        [SerializeField] private Button btn_createRoom;
        [SerializeField] private Button btn_invite;
        [SerializeField] private TMP_InputField inputName;

        private void Awake()
        {
            btn_createRoom.onClick.AddListener(OnClickedCreateRoomButton);
            btn_invite.onClick.AddListener(OnClickedInviteButton);
        }

        private void OnClickedCreateRoomButton()
        {
            NetworkManager.Instance.Lobby.CreateMatchRoom();
        }

        private void OnClickedInviteButton()
        {
            string userName = inputName.text.Trim();
            NetworkManager.Instance.Lobby.InviteUser(userName);  
        }
    }
}