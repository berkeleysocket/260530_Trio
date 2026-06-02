using DebugingUtility;
using Runtime.Clients.Core;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Runtime.Servers.Core
{
    public class Listener
    {
        private Socket _listenSocket;
        private List<ClientSession> _clients;
        private SocketAsyncEventArgs _acceptArgs;

        public bool IsBound { get; private set; }
        public bool IsListening { get; private set; }

        public void Initialize()
        {
            AddressFamily addressFamily = AddressFamily.InterNetwork;
            SocketType socketType = SocketType.Stream;
            ProtocolType protocol = ProtocolType.Tcp;
            _listenSocket = new Socket(addressFamily, socketType, protocol);

            _acceptArgs = new SocketAsyncEventArgs();
            _acceptArgs.Completed += HandleAccept;
        }

        public List<ClientSession> GetClients() => new List<ClientSession>(_clients);

        public void Bind(string address, int port)
        {
            IPAddress ipAddress = IPAddress.Parse(address);
            IPEndPoint endPoint = new IPEndPoint(ipAddress, port);
            _listenSocket.Bind(endPoint);
            IsBound = true;
        }

        public void Listen()
        {
            if (!IsBound)
            {
                CustomLog.LogError("server is not bound");
                return;
            }

            _listenSocket.Listen(10);
            IsListening = true;
        }

        public void Accept()
        {
            if (!IsListening)
            {
                CustomLog.LogError("server is not listening");
                return;
            }
            bool pending = _listenSocket.AcceptAsync(_acceptArgs);
            if (!pending)
                HandleAccept(null, _acceptArgs);
        }

        private void HandleAccept(object sender, SocketAsyncEventArgs args)
        {
            Accept();
            ClientSession client = new ClientSession(args.AcceptSocket);
            _clients.Add(client);

            client.Receive();
        }
    }
}