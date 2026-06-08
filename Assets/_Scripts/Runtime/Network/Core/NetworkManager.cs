using BackEnd;
using Runtime.Networks;
using Runtime.Pattern;
using UnityEngine;

namespace Runtime.Shared.Core
{
    public class NetworkManager : MonoSingleton<NetworkManager>
    {
        public LoginService Login { get; private set; }
        public LobbyService Lobby { get; private set; }

        protected override void OnAwake()
        {
            Initialize();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                //StartVisitor();
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                //StartHost();
            }
        }

        private void Initialize() 
        {
            Backend.Initialize();
            Login = new LoginService();
            Lobby = new LobbyService();
            Lobby.Initialize();
        }
    }
}