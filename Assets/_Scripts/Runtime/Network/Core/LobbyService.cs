using BackEnd;
using BackEnd.Tcp;
using Runtime.Utility.EventChannel;
using System;
using System.Collections.Generic;
using Utility.Debug;

namespace Runtime.Networks
{
    public class LobbyService
    {
        private Action OnLeaveMatchMakingServerComplete;
        private Action<ErrorInfo> LeaveMatchMakingServerFailed;

        private Dictionary<string, (SessionId, string)> invitationDict;

        public void Initialize()
        {
            invitationDict = new Dictionary<string, (SessionId, string)>();

            Backend.Match.OnMatchMakingRoomSomeoneInvited = HandleMatchMakingRoomSomeoneInvited;
            Backend.Match.OnMatchMakingRoomInviteResponse = HandleRespondedToRoomInvitation;
            Backend.Match.OnMatchMakingRoomJoin = HandleMatchMakingRoomJoined;
            Backend.Match.OnMatchMakingRoomLeave = HandleMatchMakingRoomLeave;
            Backend.Match.OnMatchMakingRoomInvite = HandleInvitedUser;
        }

        public void JoinMatchMakingServer()
        {
            Backend.Match.OnJoinMatchMakingServer = HandleJoinedMatchMakingServer;
            Backend.Match.JoinMatchMakingServer(out ErrorInfo _);
        }

        private void HandleJoinedMatchMakingServer(JoinChannelEventArgs args)
        {
            if(args.ErrInfo == ErrorInfo.Success)
            {
                CustomLog.LogSuccess("매치메이킹 서버 접속에 성공했습니다.");
                EventChannel.InvokeEvent(new OnJoinMatchMakingServerCompleteEvent());
            }
            else
            {
                CustomLog.LogError("매치메이킹 서버 접속에 실패했습니다.");
                EventChannel.InvokeEvent(new OnJoinMatchMakingServerFailedEvent(args.ErrInfo));
            }
        }

        public void LeaveMatchMakingServer(Action onCompleted = null, Action<ErrorInfo> onFailed = null)
        {
            if (onCompleted != null)
                this.OnLeaveMatchMakingServerComplete = onCompleted;
            else if (onFailed != null)
                this.LeaveMatchMakingServerFailed = onFailed;

            Backend.Match.OnLeaveMatchMakingServer = HandleLeaveMatchMakingServer;
            Backend.Match.LeaveMatchMakingServer();
        }

        private void HandleLeaveMatchMakingServer(LeaveChannelEventArgs args)
        {
            if(args.ErrInfo == ErrorInfo.Success)
            {
                CustomLog.LogSuccess("매치메이킹 서버 접속 종료에 성공했습니다.");
                OnLeaveMatchMakingServerComplete?.Invoke();
                OnLeaveMatchMakingServerComplete = null;
            }
            else
            {
                CustomLog.LogError("매치메이킹 서버 접속 종료에 실패했습니다.");
                LeaveMatchMakingServerFailed?.Invoke(args.ErrInfo);
                LeaveMatchMakingServerFailed = null;
            }
        }

        public void CreateMatchRoom()
        {
            Backend.Match.OnMatchMakingRoomCreate = HandleCreateMatchRoom;
            Backend.Match.CreateMatchRoom();
        }

        private void HandleCreateMatchRoom(MatchMakingInteractionEventArgs args)
        {
            if (args.ErrInfo == ErrorCode.Success)
            {
                CustomLog.LogSuccess("매칭 룸 생성에 성공했습니다.");
                EventChannel.InvokeEvent(new OnCreateMatchRoomCompleteEvent());
            }
            else
            {
                CustomLog.LogError("매칭 룸 생성에 실패했습니다.");
                EventChannel.InvokeEvent(new OnCreateMatchRoomFailedEvent(args.ErrInfo));
            }
        }

        public void InviteUser(string nickname)
        {
            Backend.Match.InviteUser(nickname);
        }

        private void HandleInvitedUser(MatchMakingInteractionEventArgs args)
        {
            ErrorCode errorCode = args.ErrInfo;
            if (errorCode == ErrorCode.Success)
            {
                CustomLog.LogSuccess("유저 초대에 성공했습니다.");
                EventChannel.InvokeEvent(new OnMatchMakingRoomInviteCompleteEvent());
            }
            else
            {
                CustomLog.LogError("유저 초대에 실패했습니다.");
                EventChannel.InvokeEvent(new OnMatchMakingRoomInviteFailedEvent(errorCode));
            }
        }

        private void HandleMatchMakingRoomSomeoneInvited(MatchMakingInvitedRoomEventArgs args)
        {
            if (args.ErrInfo == ErrorCode.Success)
            {
                string inviterNickname = args.InviteUserInfo.m_nickName;
                SessionId roomId = args.RoomId;
                string roomToken = args.RoomToken;
                invitationDict[inviterNickname] = (roomId, roomToken);

                CustomLog.LogSuccess("초대 수신에 성공했습니다.");
                CustomLog.LogSuccess($"Inviter : {inviterNickname}, Room Id : {roomId}, Room Token : {roomToken}");
                
                EventChannel.InvokeEvent(new OnMatchMakingRoomSomeoneInvitedEvent(inviterNickname));
            }
        }

        private void HandleMatchMakingRoomJoined(MatchMakingGamerInfoInRoomEventArgs args)
        {
            if (args.ErrInfo == ErrorCode.Success)
            {
                string visitorNickname = args.UserInfo.m_nickName;
                CustomLog.LogSuccess($"{visitorNickname} 유저가 입장에 성공했습니다.");
                EventChannel.InvokeEvent(new OnMatchMakingRoomJoinedEvent(visitorNickname));
            }
        }

        public void RespondToRoomInvitation(string inviterNickname, bool isAccept)
        {
            if(invitationDict.TryGetValue(inviterNickname, out (SessionId, string) invitation))
            {
                var roomId = invitation.Item1;
                var roomToken = invitation.Item2;

                if (isAccept)
                    Backend.Match.AcceptInvitation(roomId, roomToken);
                else
                    Backend.Match.DeclineInvitation(roomId, roomToken);
            }
            else
                EventChannel.InvokeEvent(new OnRespondToRoomInvitationFailed(ErrorCode.AuthenticationFailed));
        }

        private void HandleRespondedToRoomInvitation(MatchMakingInteractionEventArgs args)
        {
            ErrorCode errorCode = args.ErrInfo;
            if (errorCode == ErrorCode.Success)
            {
                CustomLog.LogSuccess("초대에 대한 수락/거절 응답을 성공했습니다");
                EventChannel.InvokeEvent(new OnRespondToRoomInvitationComplete());
            }
            else
            {
                CustomLog.LogSuccess("초대에 대한 수락/거절 응답을 실패했습니다");
                EventChannel.InvokeEvent(new OnRespondToRoomInvitationFailed(errorCode));
            }
        }

        private void HandleMatchMakingRoomLeave(MatchMakingGamerInfoInRoomEventArgs args)
        {
            if(args.ErrInfo == ErrorCode.Success)
            {
                MatchMakingUserInfo user = args.UserInfo;
                CustomLog.LogSuccess($"{user.m_nickName}(이)가 퇴장에 성공했습니다.");
                EventChannel.InvokeEvent(new OnMatchMakingRoomLeftEvent(user));
            }
        }
    }
}