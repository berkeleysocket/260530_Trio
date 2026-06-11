using UnityEngine;

namespace Runtime.UI
{
    public class LobbyPage : Page
    {
        [field: SerializeField] public RoomManageUI RoomManageUI { get; private set; }
        [field: SerializeField] public RoomUsersUI RoomUserUI { get; private set; }
        [field: SerializeField] public CreateRoomUI CreateRoomUI { get; private set; }
    }
}