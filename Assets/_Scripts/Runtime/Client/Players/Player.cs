using Runtime.Clients.Agents;

namespace Runtime.Clients.Players
{
    public class Player : Agent
    {
        private InputReader _inputReader;

        protected override void OnAwake()
        {
            base.OnAwake();

            _inputReader = GetComponent<InputReader>();

            _inputReader.InputData.OnMoved += GetModule<MovementModule>().SetHorizontalInput;
            _inputReader.InputData.OnJumped += GetModule<MovementModule>().Jump;
        }
    }
}

