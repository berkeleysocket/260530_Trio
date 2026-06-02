using Runtime.Shared.Core;
using System.Net.Sockets;

namespace Runtime.Clients.Core
{
    public class ClientSession : AbstractSession
    {
        public ClientSession(Socket connectedSocket) : base(connectedSocket)
        {
        }
    }
}