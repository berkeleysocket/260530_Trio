using Runtime.Utility.EventChannel;
using System;

namespace Runtime.UI
{
    public class TitlePage : Page
    {
        private void Start()
        {
            Action<AnyKeyInputEvent> onCompleted = (eventArgs)=> FadeOut();
            EventChannel.AddListener<AnyKeyInputEvent>(onCompleted);
        }
    }
}
