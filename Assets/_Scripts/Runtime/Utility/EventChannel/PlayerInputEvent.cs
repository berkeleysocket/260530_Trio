namespace Runtime.Utility.EventChannel
{ 
    public class PlayerInputEvent { }

    public class MoveInputEvent : GameEvent
    {
        public MoveInputEvent(float velocity)
        {
            this.Velocity = velocity;
        }

        public float Velocity { get; private set; }
    }

    public class JumpInputEvent : GameEvent { }
}