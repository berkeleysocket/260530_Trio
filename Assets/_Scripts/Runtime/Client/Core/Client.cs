using Runtime.Shared.Core;
using System.Net.Sockets;
using UnityEditor;

namespace Runtime.Client.Core
{ 
    public class Client
    {
        private Session _server;

        public void Initialize()
        {
            AddressFamily addressFamily = AddressFamily.InterNetwork;
            SocketType socketType = SocketType.Stream;
            ProtocolType protocol = ProtocolType.Tcp;

            _server = new Session(new Socket(addressFamily, socketType, protocol));
        }

        public void Connect()
        {
            _server.Connect();
        }
    }
}

