using UnityEngine;

namespace Runtime.Server.Core
{
    public class ServerBootstrap : MonoBehaviour
    {
        private Server _server;

        private void Awake()
        {
            _server = new Server();
            _server.Initialize();
        }

        [ContextMenu("ServerStart")]
        public void ServerStart()
        {
            _server.Bind("127.0.0.1", 8972);
            _server.Listen();
            _server.Accept();
        }
    }
}