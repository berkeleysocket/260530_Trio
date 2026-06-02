using UnityEngine;

namespace Runtime.Servers.Core
{
    public class ServerBootstrap : MonoBehaviour
    {
        [SerializeField] private NetworkConnectDataSO data;
        private Server _server;

        private void Awake()
        {
            _server = new Server();
            _server.Initialize();
        }

        [ContextMenu("ServerStart")]
        public void ServerStart()
        {
            _server.Open(data);
        }
    }
}