using System;

namespace Runtime.Shared.Core
{
    public class ReceiveBuffer
    {
        private byte[] _buffer;
        private int _readCursor = 0;
        private int _writeCursor = 0;

        public void Initialize(int capacity)
        {
            _buffer = new byte[capacity];
        }

        public ArraySegment<byte> WriteSegment() => new ArraySegment<byte>(_buffer, _writeCursor, _buffer.Length - _writeCursor);
        public ArraySegment<byte> ReadSegment() => new ArraySegment<byte>(_buffer, _readCursor, _writeCursor - _readCursor);

        public void OnWrite(int usedCapacity) => _writeCursor = Math.Min(_writeCursor + usedCapacity, _buffer.Length);
        public void OnRead(int readCapacity) => _readCursor = Math.Min(_readCursor + readCapacity, _writeCursor);

        public void Clean()
        {
            int usedCount = _writeCursor - _readCursor;
            if (usedCount != 0)
            {
                Array.Copy(_buffer, _readCursor, _buffer, 0, usedCount);
                _readCursor = 0;
                _writeCursor = usedCount;
            }
        }
    }
}