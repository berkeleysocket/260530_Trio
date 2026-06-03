using Runtime.Clients.Agents;
using Runtime.Clients.ModuleSystem;

namespace Runtime.Clients.FSM
{
    public abstract class AgentState
    {
        protected readonly Agent _agent;
        protected readonly int _stateClipHash; //해당 상태의 애니메이션 클립 해시
        protected readonly RenderModule _renderer;

        public AgentState(Agent agent, int stateClipHash)
        {
            _agent = agent;
            _stateClipHash = stateClipHash;
            _renderer = agent.GetModule<RenderModule>();
        }

        public virtual void Enter(float transitionDuration, int layerIndex = 0)
        {
            _renderer.PlayClip(_stateClipHash, 0f, transitionDuration, layerIndex);
        }

        public virtual void Update() { }
        public virtual void Exit() { }
    }
}