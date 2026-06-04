using Google.FlatBuffers;

namespace Runtime.Shared.Core
{
    public struct Packet
    {
        public IFlatbufferObject flatBuffer { get; private set; }

        public Packet(IFlatbufferObject buffer)
        {
            this.flatBuffer = buffer;
        }
    }
}