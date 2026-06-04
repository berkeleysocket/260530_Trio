using Utility.Debug;
using Runtime.Shared.Core;
using System;
using System.Net;
using System.Net.Sockets;

namespace Runtime.Clients.Core
{
    public class Connector
    {
        public event Action<Session> OnConnected;

        private Socket _connectSocket;
        private SocketAsyncEventArgs _connectedArgs;

        public void Initialize()
        {
            AddressFamily addressFamily = AddressFamily.InterNetwork;
            SocketType socketType = SocketType.Stream;
            ProtocolType protocol = ProtocolType.Tcp;
            _connectSocket = new Socket(addressFamily, socketType, protocol);

            _connectedArgs = new SocketAsyncEventArgs();
            _connectedArgs.Completed += HandleConnected;
        }

        public void Connect(string address, int port)
        {
            //AddressFamily addressFamily = AddressFamily.InterNetwork;
            //SocketType socketType = SocketType.Stream;
            //ProtocolType protocol = ProtocolType.Tcp;
            //Socket socket = new Socket(addressFamily, socketType, protocol);

            IPAddress ipAddress = IPAddress.Parse(address);
            IPEndPoint endPoint = new IPEndPoint(ipAddress, port);
            _connectedArgs.RemoteEndPoint = endPoint;

            //bool pending = socket.ConnectAsync(_connectedArgs);
            bool pending = _connectSocket.ConnectAsync(_connectedArgs);
            if (!pending)
                HandleConnected(null, _connectedArgs);
        }

        private void HandleConnected(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                CustomLog.LogSuccess("HandleConnected");
                Session session = new Session(args.ConnectSocket);
                OnConnected?.Invoke(session);
            }
            else
            {
                CustomLog.LogError(args.SocketError.ToString());
            }
        }

        public void Reset()
        {
            OnConnected = null;
            _connectedArgs.RemoteEndPoint = null;
            _connectSocket.Disconnect(true);
        }
    }
}