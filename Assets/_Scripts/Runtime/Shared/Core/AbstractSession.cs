using DebugingUtility;
using Runtime.Shared.Packet;
using System;
using System.Net.Sockets;

namespace Runtime.Shared.Core
{
    public abstract class AbstractSession
    {
        private Socket _connectedSocket;
        private SocketAsyncEventArgs _receiveArgs;
        private SocketAsyncEventArgs _sendArgs;
        private ReceiveBuffer _receiveBuffer;

        public AbstractSession(Socket connectedSocket)
        {
            _connectedSocket = connectedSocket;

            _receiveArgs = new SocketAsyncEventArgs();
            _receiveArgs.Completed += HandleReceived;
            _sendArgs = new SocketAsyncEventArgs();
            _receiveArgs.Completed += HandleSent;

            _receiveBuffer = new ReceiveBuffer(4096);
        }

        public void Receive()
        {
            bool pending = _connectedSocket.ReceiveAsync(_receiveArgs);
            if (!pending)
                HandleReceived(null, _receiveArgs);
        }

        public void Send(IPacket packet)
        {
            _sendArgs.SetBuffer(packet.GetBytes());
            _connectedSocket.SendAsync(_sendArgs);
        }

        private void HandleReceived(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success
                && args.BytesTransferred > 0)
            {
                int bytesTransferred = args.BytesTransferred;
                byte[] buffer = args.Buffer;
                ArraySegment<byte> segment = _receiveBuffer.WriteSegment();
                Array.Copy(buffer, 0, segment.Array, segment.Offset, bytesTransferred);
                _receiveBuffer.OnWrite(bytesTransferred);

                CustomLog.LogSuccess("HandleReceived");
            }
            else
            {
                CustomLog.LogError(args.SocketError.ToString());
            }
        }

        private void HandleSent(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success
                && args.BytesTransferred > 0)
            {
                CustomLog.LogSuccess("HandleSent");
            }
            else
            {
                CustomLog.LogError(args.SocketError.ToString());
            }
        }
    }
}

