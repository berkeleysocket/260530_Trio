namespace Runtime.Agents
{
    public class Player : Agent
    {
        protected override void OnAwake()
        {
            base.OnAwake();

            //_inputReader = GetComponent<InputReader>();

            //_inputReader.InputData.OnMoved += GetModule<MovementModule>().SetHorizontalInput;
            //_inputReader.InputData.OnJumped += GetModule<MovementModule>().Jump;
        }
    }
}

