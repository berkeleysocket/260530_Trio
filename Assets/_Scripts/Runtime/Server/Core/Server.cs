using DebugingUtility;
using Runtime.Shared.Core;
using Runtime.Shared.Packet;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Runtime.Server.Core
{
    public class Server
    {
        private Socket _socket;
        private List<Session> _clients;
        private SocketAsyncEventArgs _acceptArgs;

        public bool IsBound { get; private set; }
        public bool IsListening { get; private set; }

        public void Initialize()
        {
            AddressFamily addressFamily = AddressFamily.InterNetwork;
            SocketType socketType = SocketType.Stream;
            ProtocolType protocol = ProtocolType.Tcp;
            _socket = new Socket(addressFamily, socketType, protocol);

            _acceptArgs = new SocketAsyncEventArgs();
            _acceptArgs.Completed += HandleAccept;
        }

        public void Bind(string address, int port)
        {
            IPAddress ipAddress = IPAddress.Parse(address);
            IPEndPoint endPoint = new IPEndPoint(ipAddress, port);
            _socket.Bind(endPoint);
            IsBound = true;
        }

        public void Listen()
        {
            if (!IsBound)
            {
                CustomLog.LogError("server is not bound");
                return;
            }

            _socket.Listen(10);
            IsListening = true;
        }

        public void Accept()
        {
            if(!IsListening)
            {
                CustomLog.LogError("server is not listening");
                return;
            }
            bool pending = _socket.AcceptAsync(_acceptArgs);
            if (!pending)
                HandleAccept(null, _acceptArgs);
        }

        public void BroadcastAll(IPacket packet)
        {
            for(int i = _clients.Count - 1; i >= 0; i--)
            {
                _clients[i].Send(packet);
            }
        }

        private void HandleAccept(object sender, SocketAsyncEventArgs args)
        {
            _socket.AcceptAsync(_acceptArgs);
            Session client = new Session(args.AcceptSocket);
            _clients.Add(client);

            client.Receive();
        }
    }

}