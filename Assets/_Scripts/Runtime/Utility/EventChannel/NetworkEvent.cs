using BackEnd.Tcp;
using System.Collections.Generic;

namespace Runtime.Utility.EventChannel
{
    public class NetworkEvent { }

    #region OnCustomLogin
    public class OnCustomLoginCompleteEvent : GameEvent 
    { 
        public string Nickname { get; private set; }

        public OnCustomLoginCompleteEvent(string nickname)
        {
            this.Nickname = nickname;
        }
    }
    public class OnCustomLoginFailedEvent : GameEvent
    {
        public ushort StatusCode { get; private set; }
        public OnCustomLoginFailedEvent(ushort statusCode)
        {
            this.StatusCode = statusCode;
        }
    }
    #endregion

    #region OnCustomSignUp
    public class OnCustomSignUpCompleteEvent : GameEvent { }
    public class OnCustomSignUpFailedEvent : GameEvent 
    { 
        public ushort StatusCode { get; private set; }
        public OnCustomSignUpFailedEvent(ushort statusCode)
        {
            this.StatusCode = statusCode;
        }
    }
    #endregion

    #region OnJoinMatchMakingServer
    public class OnJoinMatchMakingServerCompleteEvent : GameEvent { }
    public class OnJoinMatchMakingServerFailedEvent : GameEvent 
    { 
        public ErrorInfo ErrorInfo { get; private set; }
        public OnJoinMatchMakingServerFailedEvent(ErrorInfo errorInfo)
        {
            this.ErrorInfo = errorInfo;
        }
    }
    #endregion

    #region OnCreateMatchRoom
    public class OnCreateMatchRoomCompleteEvent : GameEvent { }
    public class OnCreateMatchRoomFailedEvent : GameEvent
    {
        public ErrorCode ErrorCode { get; private set; }
        public OnCreateMatchRoomFailedEvent(ErrorCode errorCode)
        {
            this.ErrorCode = ErrorCode;
        }
    }
    #endregion

    #region OnValidateNickname
    public class OnValidateNicknameCompleteEvent : GameEvent { }
    public class OnValidateNicknameFailedEvent : GameEvent
    {
        public ushort StatusCode { get; private set; }
        public OnValidateNicknameFailedEvent(ushort statusCode)
        {
            this.StatusCode = statusCode;
        }
    }
    #endregion

    #region OnUpdateNickname
    public class OnUpdateNicknameCompleteEvent : GameEvent { }
    public class OnUpdateNicknameFailedEvent : GameEvent 
    { 
        public ushort StatusCode { get; private set; }
        public OnUpdateNicknameFailedEvent(ushort statusCode)
        {
            this.StatusCode = statusCode;
        }
    }
    #endregion

    #region OnMatchMakingRoomSomeoneInvited
    public class OnMatchMakingRoomSomeoneInvitedEvent : GameEvent 
    { 
        public string InviterNickname { get; private set; }

        public OnMatchMakingRoomSomeoneInvitedEvent(string inviterNickname)
        {
            this.InviterNickname = inviterNickname;
        }
    }
    #endregion

    #region OnMatchMakingRoomJoined
    public class OnMatchMakingRoomJoinedEvent : GameEvent
    {
        public string VisitorNickname { get; private set; }

        public OnMatchMakingRoomJoinedEvent(string visitorNickname)
        {
            this.VisitorNickname = visitorNickname;
        }
    }
    #endregion

    #region OnMatchMakingRoomLeave
    public class OnMatchMakingRoomLeftEvent : GameEvent
    {
        public MatchMakingUserInfo UserInfo { get; private set; }

        public OnMatchMakingRoomLeftEvent(MatchMakingUserInfo userInfo)
        {
            this.UserInfo = userInfo;
        }
    }
    #endregion

    #region OnMatchMakingRoomInvite
    public class OnMatchMakingRoomInviteCompleteEvent : GameEvent { }
    public class OnMatchMakingRoomInviteFailedEvent : GameEvent 
    { 
        public OnMatchMakingRoomInviteFailedEvent(ErrorCode errorCode)
        {
            this.ErrorCode = errorCode;
        }

        public ErrorCode ErrorCode { get; private set; }
    }
    #endregion

    #region OnRespondToRoomInvitation
    public class OnRespondToRoomInvitationCompleteEvent : GameEvent { }
    public class OnRespondToRoomInvitationFailedEvent : GameEvent 
    { 
        public ErrorCode ErrorCode { get; private set; } 
        public OnRespondToRoomInvitationFailedEvent(ErrorCode errorCode)
        {
            this.ErrorCode = errorCode;
        }
    }
    #endregion

    #region OnRemovedMail
    public class OnRemovedMailEvent : GameEvent { }
    #endregion

    #region OnLeftMatchRoom
    public class OnLeftMatchRoomCompleteEvent : GameEvent { }
    public class OnLeftMatchRoomFailedEvent : GameEvent 
    { 
        public ErrorCode ErrorCode { get; private set; }

        public OnLeftMatchRoomFailedEvent(ErrorCode errorCode)
        {
            this.ErrorCode = ErrorCode;
        }
    }
    #endregion

    #region OnHandleMatchMakingRoomUserList
    public class OnHandleMatchMakingRoomUserListCompleteEvent : GameEvent
    {
        public List<MatchMakingUserInfo> UserList { get; private set; }

        public OnHandleMatchMakingRoomUserListCompleteEvent(List<MatchMakingUserInfo> userList)
        {
            this.UserList = userList;
        }
    }

    public class OnHandleMatchMakingRoomUserListFailedEvent : GameEvent 
    { 

        public OnHandleMatchMakingRoomUserListFailedEvent()
        {

        }
    }
    #endregion
}
