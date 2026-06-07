using BackEnd;
using Runtime.Networks;
using UnityEngine;

namespace Runtime.Shared.Core
{
    public class NetworkManager : MonoBehaviour
    {
        public static NetworkManager Instance => _instance;
        private static NetworkManager _instance;

        public LoginService Login { get; private set; }
        public LobbyService Lobby { get; private set; }

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
        }
    }
}