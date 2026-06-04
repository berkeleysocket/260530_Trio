namespace Runtime.ModuleSystem
{
    public interface IModule
    {
        public void Initialize(ModuleOwner owner);
    }
}