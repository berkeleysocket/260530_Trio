using GameRoomPacket;
using Google.FlatBuffers;

namespace Runtime.Shared.Core
{
    public class PacketSerializer
    {
        private FlatBufferBuilder _builder;

        public void Initialize()
        {
            this._builder = new FlatBufferBuilder(1024);
        }

        //public C2S_MoveInputPacket Serialize(float velocity, UnityEngine.Vector2 position)
        //{
        //    Offset<Vector2> positionOffset = Vector2.CreateVector2(_builder, position.x, position.y);
        //    C2S_MoveInputPacket.StartC2S_MoveInputPacket(_builder);
        //    C2S_MoveInputPacket.AddPosition(_builder, positionOffset);
        //    C2S_MoveInputPacket.AddVelocity(_builder, velocity);
        //    Offset<C2S_MoveInputPacket> packetOffset = C2S_MoveInputPacket.EndC2S_MoveInputPacket(_builder);
        //    _builder.Finish();
        //}
    }
}
