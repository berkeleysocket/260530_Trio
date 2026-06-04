using Google.FlatBuffers;
using Runtime.Clients.Core;
using Runtime.Servers.Core;
using UnityEngine;

namespace Runtime.Shared.Core
{
    public class NetworkManager : MonoBehaviour
    {
        public static NetworkManager Instance => _instance;
        private static NetworkManager _instance;

        public NetworkRole Role { get; private set; }

        private RoomBootstrap _visitorBootstrap;

        public void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            Role = NetworkRole.None;
            _visitorBootstrap = new RoomBootstrap();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.C))
                StartVisitor();
            if(Input.GetKeyDown(KeyCode.H))
                StartHost();
        }

        [ContextMenu("Boot Visitor")]
        public void StartVisitor()
        {
            Role = NetworkRole.Visitor;
            _visitorBootstrap.Initialize();
            _visitorBootstrap.Boot();
        }

        [ContextMenu("Boot Host")]
        public void StartHost()
        {
            Role = NetworkRole.Host;
        }

        public void Send(IFlatbufferObject packet)
        {
        }
    }
}