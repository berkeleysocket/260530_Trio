using System;

namespace Runtime.Shared.Packet
{ 
    public interface IPacket
    {
        public ArraySegment<byte> GetBytes();
    }
}