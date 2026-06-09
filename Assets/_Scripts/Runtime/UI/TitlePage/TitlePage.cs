using Runtime.InputSystem;
using Runtime.Utility.EventChannel;

namespace Runtime.UI
{
    public class TitlePage : Page
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
            InputManager.Instance.RegisterReader<UIInputReader>();
            EventChannel.AddListener<AnyKeyInputEvent>(HandlePressedAnyKey);
        }

        private void OnDisable()
        {
            EventChannel.RemoveListener<AnyKeyInputEvent>(HandlePressedAnyKey);
        }

        private void HandlePressedAnyKey(AnyKeyInputEvent args)
        {
            FadeOut();
        }
    }
}
