using Runtime.Shared.Core;
using System.Net.Sockets;

namespace Runtime.Clients.Core
{
    public class ServerSession : AbstractSession
    {
        public ServerSession(Socket connectedSocket) : base(connectedSocket)
        {
        }
    }
}