using System.Collections.Generic;
using System.Net.Sockets;

public class ServerBootstrap
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

        _socket.Listen(10);

        _acceptArgs = new SocketAsyncEventArgs();
        _acceptArgs.Completed += HandleAccept;
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
