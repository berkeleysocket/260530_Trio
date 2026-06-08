using BackEnd;
using BackEnd.Tcp;
using System;
using System.Collections.Generic;
using Utility.Debug;

namespace Runtime.Networks
{
    public class LobbyService
    {
        public event Action MatchMakingRoomSomeoneInvited;
        public event Action<MatchMakingUserInfo> MatchMakingRoomJoined;

        private Action JoinMatchMakingServerCompleted;
        private Action<ErrorInfo> JoinMatchMakingServerFailed;

        private Action LeaveMatchMakingServerCompleted;
        private Action<ErrorInfo> LeaveMatchMakingServerFailed;

        private Action CreateMatchRoomCompleted;
        private Action<ErrorCode> CreateMatchRoomFailed;

        private Action InviteUserCompleted;
        private Action<ErrorCode> InviteUserFailed;

        private Action RespondToRoomInvitationCompleted;
        private Action<ErrorCode> RespondToRoomInvitationFailed;
        private Dictionary<string, (SessionId, string)> invitationDict = new Dictionary<string, (SessionId, string)>();

        public void JoinMatchMakingServer(Action onCompleted = null, Action<ErrorInfo> onFailed = null)
        {
            if (onCompleted != null)
                this.JoinMatchMakingServerCompleted = onCompleted;
            if (onFailed != null)
                this.JoinMatchMakingServerFailed = onFailed;

            Backend.Match.OnMatchMakingRoomSomeoneInvited = HandleMatchMakingRoomSomeoneInvited;
            Backend.Match.OnMatchMakingRoomInviteResponse = HandleRespondedToRoomInvitation;
            Backend.Match.OnMatchMakingRoomJoin = HandleOnMatchMakingRoomJoined;

            Backend.Match.OnJoinMatchMakingServer = HandleJoinedMatchMakingServer;
            Backend.Match.JoinMatchMakingServer(out ErrorInfo _);
        }

        private void HandleJoinedMatchMakingServer(JoinChannelEventArgs args)
        {
            if(args.ErrInfo == ErrorInfo.Success)
            {
                CustomLog.LogSuccess("매치메이킹 서버 접속에 성공했습니다.");
                JoinMatchMakingServerCompleted?.Invoke();
                JoinMatchMakingServerCompleted = null;
            }
            else
            {
                CustomLog.LogError("매치메이킹 서버 접속에 실패했습니다.");
                JoinMatchMakingServerFailed?.Invoke(args.ErrInfo);
                JoinMatchMakingServerFailed = null;
            }
        }

        public void LeaveMatchMakingServer(Action onCompleted = null, Action<ErrorInfo> onFailed = null)
        {
            if (onCompleted != null)
                this.LeaveMatchMakingServerCompleted = onCompleted;
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
                LeaveMatchMakingServerCompleted?.Invoke();
                LeaveMatchMakingServerCompleted = null;
            }
            else
            {
                CustomLog.LogError("매치메이킹 서버 접속 종료에 실패했습니다.");
                LeaveMatchMakingServerFailed?.Invoke(args.ErrInfo);
                LeaveMatchMakingServerFailed = null;
            }
        }

        public void CreateMatchRoom(Action onCompleted = null, Action<ErrorCode> onFailed = null)
        {
            if (onCompleted != null)
                this.CreateMatchRoomCompleted = onCompleted;
            if (onFailed != null)
                this.CreateMatchRoomFailed = onFailed;

            Backend.Match.OnMatchMakingRoomCreate = HandleCreateMatchRoom;
            Backend.Match.CreateMatchRoom();
        }

        private void HandleCreateMatchRoom(MatchMakingInteractionEventArgs args)
        {
            if (args.ErrInfo == ErrorCode.Success)
            {
                CustomLog.LogSuccess("매칭 룸 생성에 성공했습니다.");
                CreateMatchRoomCompleted?.Invoke();
                CreateMatchRoomCompleted = null;
            }
            else
            {
                CustomLog.LogError("매칭 룸 생성에 실패했습니다.");
                CreateMatchRoomFailed?.Invoke(args.ErrInfo);
                CreateMatchRoomFailed = null;
            }
        }

        public void InviteUser(string nickname, Action onCompleted = null,
            Action<ErrorCode> onFailed = null)
        {
            if (onCompleted != null)
                InviteUserCompleted = onCompleted;
            if (onFailed != null)
                InviteUserFailed = onFailed;

            Backend.Match.OnMatchMakingRoomInvite = HandleInvitedUser;
            Backend.Match.InviteUser(nickname);
        }

        private void HandleInvitedUser(MatchMakingInteractionEventArgs args)
        {
            if (args.ErrInfo == ErrorCode.Success)
            {
                CustomLog.LogSuccess("유저 초대에 성공했습니다.");
                InviteUserCompleted?.Invoke();
                InviteUserCompleted = null;
            }
            else
            {
                CustomLog.LogError("유저 초대에 실패했습니다.");
                InviteUserFailed?.Invoke(args.ErrInfo);
                InviteUserFailed = null;
            }
        }

        private void HandleMatchMakingRoomSomeoneInvited(MatchMakingInvitedRoomEventArgs args)
        {
            if (args.ErrInfo == ErrorCode.Success)
            {
                string inviter = args.InviteUserInfo.m_nickName;
                SessionId roomId = args.RoomId;
                string roomToken = args.RoomToken;
                invitationDict[inviter] = (roomId, roomToken);

                CustomLog.LogSuccess("초대 수신에 성공했습니다.");
                CustomLog.LogSuccess($"Inviter : {inviter}, Room Id : {roomId}, Room Token : {roomToken}");
                MatchMakingRoomSomeoneInvited?.Invoke();
            }
        }

        private void HandleOnMatchMakingRoomJoined(MatchMakingGamerInfoInRoomEventArgs args)
        {
            if (args.ErrInfo == ErrorCode.Success)
            {
                MatchMakingUserInfo user = args.UserInfo;
                CustomLog.LogSuccess($"{user.m_nickName} 유저가 입장에 성공했습니다.");
                MatchMakingRoomJoined?.Invoke(user);
            }
        }

        public void RespondToRoomInvitation(string inviterNickname, bool isAccept,
            Action onCompleted = null, Action<ErrorCode> onFailed = null)
        {
            var roomId = invitationDict[inviterNickname].Item1;
            var roomToken = invitationDict[inviterNickname].Item2;

            if (RespondToRoomInvitationCompleted != null)
                this.RespondToRoomInvitationCompleted = onCompleted;
            if (RespondToRoomInvitationFailed != null)
                this.RespondToRoomInvitationFailed = onFailed;
            
            if(isAccept)
                Backend.Match.AcceptInvitation(roomId, roomToken);
            else
                Backend.Match.DeclineInvitation(roomId, roomToken);
        }

        private void HandleRespondedToRoomInvitation(MatchMakingInteractionEventArgs args)
        {
            if(args.ErrInfo == ErrorCode.Success)
            {
                CustomLog.LogSuccess("초대에 대한 수락/거절 응답을 성공했습니다");
                RespondToRoomInvitationCompleted?.Invoke();
                RespondToRoomInvitationCompleted = null;
            }
            else
            {
                CustomLog.LogSuccess("초대에 대한 수락/거절 응답을 실패했습니다");
                RespondToRoomInvitationFailed?.Invoke(args.ErrInfo);
                RespondToRoomInvitationFailed = null;
            }
        }
    }
}