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

        public bool IsOpened { get; private set; }

        public AbstractSession(Socket connectedSocket)
        {
            _connectedSocket = connectedSocket;

            _receiveArgs = new SocketAsyncEventArgs();
            _receiveArgs.Completed += HandleReceived;
            _sendArgs = new SocketAsyncEventArgs();
            _receiveArgs.Completed += HandleSent;

            _receiveBuffer = new ReceiveBuffer(4096);
        }

        public void Open()
        {
            IsOpened = true;
            Receive();
        }

        public void Close()
        {
            IsOpened = false;
            _receiveArgs.Completed -= HandleReceived;
            _sendArgs.Completed -= HandleSent;

            _connectedSocket.Disconnect(false);
            _connectedSocket.Close();
            _connectedSocket.Dispose();
            _receiveArgs.Dispose();
            _sendArgs.Dispose();
            _receiveBuffer = null;
        }

        public void Send(IPacket packet)
        {
            if(IsOpened)
            {
                _sendArgs.SetBuffer(packet.GetBytes());
                _connectedSocket.SendAsync(_sendArgs);
            }
            else
            {
                CustomLog.LogError("Session is not opened");
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

        private void Receive()
        {
            if(IsOpened)
            {
                bool pending = _connectedSocket.ReceiveAsync(_receiveArgs);
                if (!pending)
                    HandleReceived(null, _receiveArgs);
            }
            else
            {
                CustomLog.LogError("Session is not opened");
            }
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
                Receive();
            }
            else
            {
                CustomLog.LogError(args.SocketError.ToString());
            }
        }
    }
}