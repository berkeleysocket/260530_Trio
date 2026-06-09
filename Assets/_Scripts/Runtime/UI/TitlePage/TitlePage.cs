using Runtime.Utility.EventChannel;

namespace Runtime.UI
{
    public class TitlePage : Page
    {
        private void Start()
        {
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
