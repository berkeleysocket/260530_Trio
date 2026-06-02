using Runtime.Shared.Packet;

namespace Runtime.Servers.Core
{
    public class Server
    {
        private Listener _listener;

        public void Initialize()
        {
            _listener = new Listener();
            _listener.Initialize();
        }

        public void Open()
        {
            _listener.Bind("127.0.0.1", 8976);
            _listener.Listen();
            _listener.Accept();
        }

        public void BroadcastAll(IPacket packet)
        {
            var clients = _listener.GetClients();
            for (int i = clients.Count - 1; i >= 0; i--)
            {
                clients[i].Send(packet);
            }
        }
    }
}