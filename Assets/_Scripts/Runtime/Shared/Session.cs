using System.Net.Sockets;

public class Session
{
    private Socket _connectedSocket;
    private SocketAsyncEventArgs _receiveArgs;

    public Session(Socket connectedSocket)
    {
        this._connectedSocket = connectedSocket;

        _receiveArgs = new SocketAsyncEventArgs();
        _receiveArgs.Completed += HandleReceive;
    }

    public void Receive()
    {
        bool pending = _connectedSocket.ReceiveAsync(_receiveArgs);
        if (!pending)
            HandleReceive(null, _receiveArgs);
    }

    private void HandleReceive(object sender, SocketAsyncEventArgs args)
    {
        args.
    }
}
