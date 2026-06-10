using BackEnd;
using Codice.CM.Client.Differences.Graphic;
using Runtime.Networks;
using Runtime.Pattern;
using Runtime.Utility.EventChannel;

namespace Runtime.Shared.Core
{
    public class NetworkManager : MonoSingleton<NetworkManager>
    {
        public LoginService Login { get; private set; }
        public LobbyService Lobby { get; private set; }
        public Account MyAccount { get; private set; }

        private bool _initialized = false;

        public void Initialize()
        {
            if (_initialized || Instance == null) return;

            Backend.Initialize();

            Login = new LoginService();
            Lobby = new LobbyService();

            Lobby.Initialize();

            EventChannel.AddListener<OnCustomLoginCompleteEvent>((args)=> this.MyAccount = new Account(args.Nickname));

            _initialized = true;
        }

        private void Update()
        {
            Backend.Match.Poll();
        }
    }
}