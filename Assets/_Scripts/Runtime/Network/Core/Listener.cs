using Utility.Debug;
using Runtime.Shared.Core;
using System;
using System.Net;
using System.Net.Sockets;

namespace Runtime.Servers.Core
{
    public class Listener
    {
        public event Action<Session> OnAccepted;

        private Socket _listenSocket;
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
            if(args.SocketError == SocketError.Success)
            {
                CustomLog.LogSuccess("HandleAccept");
                Session client = new Session(args.AcceptSocket);
                OnAccepted?.Invoke(client);
                client.Open();
                Accept();
            }
            else
            {
                CustomLog.LogError(args.SocketError.ToString());
            }
        }

        public void Reset()
        {
            OnAccepted = null;
            IsBound = false;
            IsListening = false;
            _listenSocket.Disconnect(true);
            _acceptArgs.AcceptSocket = null;
        }
    }
}