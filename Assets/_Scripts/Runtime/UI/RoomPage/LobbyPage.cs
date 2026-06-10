using UnityEngine;

namespace Runtime.UI
{
    public class LobbyPage : Page
    {
        [SerializeField] private RoomManageUI roomManageUI;
        [SerializeField] private RoomUsersUI roomUserUI;

        public override void Initialize()
        {
            base.Initialize();

            roomManageUI.Initialize();
            roomUserUI.Initialize();
        }
    }
}