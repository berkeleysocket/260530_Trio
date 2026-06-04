using Utility.Debug;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Runtime.Shared.Core
{
    public class Session
    {
        public bool IsOpened { get; private set; }

        private Socket _connectedSocket;
        private SocketAsyncEventArgs _receiveArgs;
        private SocketAsyncEventArgs _sendArgs;
        private ReceiveBuffer _receiveBuffer;
        private SendQueue _sendQueue;

        public Session(Socket connectedSocket)
        {
            _connectedSocket = connectedSocket;

            _receiveBuffer = new ReceiveBuffer();
            _receiveBuffer.Initialize(4056);
            _sendQueue = new SendQueue();
            _sendQueue.Initialize();

            _receiveArgs = new SocketAsyncEventArgs();
            _receiveArgs.Completed += HandleReceived;
            _sendArgs = new SocketAsyncEventArgs();
            _sendArgs.Completed += HandleSent;
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

        public void Send(Packet packet)
        {
            if(IsOpened)
            {
                List<ArraySegment<byte>> bufferList = null;
                _sendQueue.Enqueue(packet);
                _sendQueue.TryFlush(out bufferList);
                _sendArgs.BufferList = bufferList;
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
                _receiveBuffer.Clean();
                _receiveArgs.SetBuffer(_receiveBuffer.WriteSegment());

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