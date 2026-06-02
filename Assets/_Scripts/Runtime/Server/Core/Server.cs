using Runtime.Shared.Packet;

namespace Runtime.Servers.Core
{
    public class Server
    {
        private Listener _listener;
        public void BroadcastAll(IPacket packet)
        {
            for (int i = _clients.Count - 1; i >= 0; i--)
            {
                _clients[i].Send(packet);
            }
        }
    }
}