using KSY.Utility;
using System;
using System.Net.Sockets;

public class Session
{
    private Socket _connectedSocket;
    private SocketAsyncEventArgs _receiveArgs;
    private ReceiveBuffer _receiveBuffer;

    public Session(Socket connectedSocket)
    {
        this._connectedSocket = connectedSocket;

        _receiveArgs = new SocketAsyncEventArgs();
        _receiveArgs.Completed += HandleReceive;

        _receiveBuffer = new ReceiveBuffer(4096);
    }

    public void Receive()
    {
        bool pending = _connectedSocket.ReceiveAsync(_receiveArgs);
        if (!pending)
            HandleReceive(null, _receiveArgs);
    }

    public void Send()
    {

    }

    private void HandleReceive(object sender, SocketAsyncEventArgs args)
    {
        if(args.SocketError == SocketError.Success
            && args.BytesTransferred > 0)
        {
            int bytesTransferred = args.BytesTransferred;
            byte[] buffer = args.Buffer;
            ArraySegment<byte> segment = _receiveBuffer.WriteSegment();
            Array.Copy(buffer, 0, segment.Array, segment.Offset, bytesTransferred);
            _receiveBuffer.OnWrite(bytesTransferred);
        }
        else
        {
            CustomLog.LogError(args.SocketError.ToString());
        }
    }

}
