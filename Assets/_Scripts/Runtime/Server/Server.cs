using System.Collections.Generic;
using System.Net.Sockets;

public class Server
{
    private Socket _socket;
    private List<Session> _clients;
    private SocketAsyncEventArgs _acceptArgs;

    public void Initialize()
    {
        AddressFamily addressFamily = AddressFamily.InterNetwork;
        SocketType socketType = SocketType.Stream;
        ProtocolType protocol = ProtocolType.Tcp;
        _socket = new Socket(addressFamily, socketType, protocol);

        _acceptArgs = new SocketAsyncEventArgs();
        _acceptArgs.Completed += HandleAccept;
    }

    public void Listen()
    {
        _socket.Listen(10);
    }

    public void Accept()
    {
        bool pending = _socket.AcceptAsync(_acceptArgs);
        if (!pending)
            HandleAccept(null, _acceptArgs);
    }

    private void HandleAccept(object sender, SocketAsyncEventArgs args)
    {
        _socket.AcceptAsync(_acceptArgs);
        Session client = new Session(args.AcceptSocket);
        _clients.Add(client);

        client.Receive();
    }
}
