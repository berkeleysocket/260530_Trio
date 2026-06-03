using Runtime.Clients.ModuleSystem;

namespace Runtime.Clients.Agents
{
    public interface IModule
    {
        public void Initialize(ModuleOwner owner);
    }
}