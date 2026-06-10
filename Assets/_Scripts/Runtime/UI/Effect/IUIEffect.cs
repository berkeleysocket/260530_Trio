using Runtime.Pattern;

namespace Runtime.UI
{
    public interface IUIEffect : IInitializable
    {
        public void ActiveEffect();
        public void InactiveEffect();
    }
}
