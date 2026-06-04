using System;
using System.Collections.Generic;

namespace Runtime.Shared.Core
{
    public class SendQueue
    {
        private Queue<Packet> _queue;
        private bool isEmpty => _queue.Count <= 0;

        public void Initialize()
        {
            _queue = new Queue<Packet>();
        }

        public void Enqueue(Packet packet)
        {
            _queue.Enqueue(packet);
        }

        public void TryFlush(out List<ArraySegment<byte>> bufferList)
        {
            bufferList = null;

            if (isEmpty) return;

            bufferList = new List<ArraySegment<byte>>();

            while (!isEmpty)
            {
                Packet packet = _queue.Dequeue();
                int pos = packet.flatBuffer.ByteBuffer.Position;
                int len = packet.flatBuffer.ByteBuffer.Length;
                bufferList.Add(packet.flatBuffer.ByteBuffer.ToArraySegment(pos, len));
            }
        }
    }
}

