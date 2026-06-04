using Runtime.Servers.Core;
using Runtime.Shared.Core;
using System.Collections.Generic;

namespace Runtime.Clients.Core
{
    public class RoomBootstrap
    {
        private Listener _listener;
        private Connector _connector;
        private List<Session> _sessions;

        public void Initialize()
        {
            if (_listener != null)
                _listener.Reset();
            else
            {
                _listener = new Listener();
                _listener.Initialize();
            }

            if (_connector != null)
                _connector.Reset();
            else
            {
                _connector = new Connector();
                _connector.Initialize();
            }

            _sessions.Clear();

            _listener.OnAccepted += HandleAccepted;
        }

        public void Boot()
        {
            _connector.Connect("127.0.0.1", 9797);
        }

        public void HandleAccepted(Session session) => _sessions.Add(session);
        public void HandleConnected()
        {

        }
    }
}