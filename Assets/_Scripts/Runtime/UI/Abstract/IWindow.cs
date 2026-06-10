using Runtime.Pattern;

namespace Runtime.UI
{
    public interface IWindow : IInitializable
    {
        public void Show();
        public void Hide();
    }
}
