using System.Net.Sockets;

public class ServerBootstrap
{
    private Socket _socket;

    public void Initialize()
    {
        AddressFamily addressFamily = AddressFamily.InterNetwork;
        SocketType socketType = SocketType.Stream;
        ProtocolType protocol = ProtocolType.Tcp;
        _socket = new Socket(addressFamily, socketType, protocol);
    }
}
