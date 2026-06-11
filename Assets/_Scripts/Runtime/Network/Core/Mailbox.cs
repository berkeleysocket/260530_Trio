using Runtime.Utility.EventChannel;
using System.Collections.Generic;
using BackEnd.Tcp;
using DG.Tweening;
using Utility.Debug;

namespace Runtime.Networks
{
    public struct Invitation
    {
        public bool Initialized { get; private set; }
        public SessionId RoomId { get; private set; }
        public string RoomToken { get; private set; }

        public Invitation(SessionId roomId, string roomToken)
        {
            Initialized = true;

            this.RoomId = roomId;
            this.RoomToken = roomToken;
        }
    }

    public class Mailbox
    {
        private Dictionary<string, Invitation> _invitationBox;

        public void Initialize()
        {
            this._invitationBox = new Dictionary<string, Invitation>();
        }

        public bool TryGetMail(string inviterNickname, out Invitation invitation)
        {
            _invitationBox.TryGetValue(inviterNickname, out invitation);
            return invitation.Initialized;
        }

        public void AddMail(string inviterNickname, SessionId roomId, string roomToken)
        {
            _invitationBox[inviterNickname] = new Invitation(roomId, roomToken);

            CustomLog.LogWarning("皋老 昏力 贸府 秦具 窃");
            //TweenCallback onInvitationExpired = () =>
            //{
            //    RemoveMail(inviterNickname);
            //};

            //DOVirtual.DelayedCall(5f, onInvitationExpired);
        }

        private void RemoveMail(string inviterNickname)
        {
            if (_invitationBox.ContainsKey(inviterNickname))
            {
                _invitationBox.Remove(inviterNickname);
                EventChannel.InvokeEvent(new OnRemovedMailEvent());
            }
        }
    }
}
