using Runtime.Utility.EventChannel;
using System.Collections.Generic;
using BackEnd.Tcp;
using DG.Tweening;

namespace Runtime.Networks
{
    public struct Invitation
    {
        public SessionId RoomId { get; private set; }
        public string RoomToken { get; private set; }

        public Invitation(SessionId roomId, string roomToken)
        {
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
            return invitation.RoomId != SessionId.None && string.IsNullOrEmpty(invitation.RoomToken);
        }

        public void AddMail(string inviterNickname, SessionId roomId, string roomToken)
        {
            _invitationBox[inviterNickname] = new Invitation(roomId, roomToken);

            TweenCallback onInvitationExpired = () =>
            {
                RemoveMail(inviterNickname);
            };

            DOVirtual.DelayedCall(5f, onInvitationExpired);
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
