using BackEnd.Tcp;
using Codice.Client.Common;
using Runtime.Shared.Core;
using Runtime.Utility.EventChannel;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class RoomUsersUI : WindowElementDirector
    {
        [SerializeField] private RoomUserElementUI userElementPrefab;
        [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;

        private List<RoomUserElementUI> _roomUserList;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _roomUserList = new List<RoomUserElementUI>();

            EventChannel.AddListener<OnCreateMatchRoomCompleteEvent>(OnCreateMatchRoomComplete);
            EventChannel.AddListener<OnHandleMatchMakingRoomUserListCompleteEvent>(OnHandleMatchMakingRoomUserListComplete);
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

        private void OnHandleMatchMakingRoomUserListComplete(OnHandleMatchMakingRoomUserListCompleteEvent args)
        {
            List<MatchMakingUserInfo> userList = args.UserList;

            foreach(var userInfo in userList)
            {
                string userNickname = userInfo.m_nickName;
                if (userNickname == NetworkManager.Instance.MyAccount.Nickname) continue;
                RoomUserElementUI visitor = Instantiate(userElementPrefab, verticalLayoutGroup.transform);
                visitor.Initialize(userNickname);
                _roomUserList.Add(visitor);
            }
        }
    }
}
