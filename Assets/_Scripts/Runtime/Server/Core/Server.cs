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

        public void Open(NetworkConnectDataSO data)
        {
            _listener.Bind(data.IpAddress, data.Port);
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