using Runtime.Shared.Core;
using Runtime.Utility.EventChannel;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class RoomUsersUI : MonoBehaviour, IWindowElement
    {
        [SerializeField] private RoomUserElementUI userElementPrefab;
        [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;

        private List<RoomUserElementUI> _roomUserList;

        public void Initialize()
        {
            _roomUserList = new List<RoomUserElementUI>();

            EventChannel.AddListener<OnCreateMatchRoomCompleteEvent>(OnCreateMatchRoomComplete);
            EventChannel.AddListener<OnMatchMakingRoomJoinedEvent>(OnMatchMakingRoomJoined);
            EventChannel.AddListener<OnMatchMakingRoomLeftEvent>(OnMatchMakingRoomLeft);
        }

        private void OnCreateMatchRoomComplete(OnCreateMatchRoomCompleteEvent args)
        {
            RoomUserElementUI my = Instantiate(userElementPrefab, verticalLayoutGroup.transform);
            my.Initialize(NetworkManager.Instance.MyAccount.Nickname);
            _roomUserList.Add(my);
        }

        private void OnMatchMakingRoomJoined(OnMatchMakingRoomJoinedEvent args)
        {
            RoomUserElementUI visitor = Instantiate(userElementPrefab, verticalLayoutGroup.transform);
            visitor.Initialize(args.VisitorNickname);
            _roomUserList.Add(visitor);
        }

        private void OnMatchMakingRoomLeft(OnMatchMakingRoomLeftEvent args)
        {
            string leaverNickname = args.UserInfo.m_nickName;
            int index = _roomUserList.FindIndex((roomUser) => roomUser.VisitorNickname == leaverNickname);
            var leaver = _roomUserList[index];
            Destroy(leaver);
            _roomUserList.RemoveAt(index);
        }
    }
}
