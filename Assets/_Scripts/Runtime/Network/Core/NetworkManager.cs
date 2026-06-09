using BackEnd;
using Runtime.Networks;
using Runtime.Pattern;

namespace Runtime.Shared.Core
{
    public class NetworkManager : MonoSingleton<NetworkManager>
    {
        public LoginService Login { get; private set; }
        public LobbyService Lobby { get; private set; }

        private bool _initialized = false;

        public void Initialize()
        {
            if (_initialized || Instance == null) return;

            Backend.Initialize();

            Login = new LoginService();
            Lobby = new LobbyService();

            Lobby.Initialize();

            _initialized = true;
        }

        private void Update()
        {
            Backend.Match.Poll();
        }
    }
}