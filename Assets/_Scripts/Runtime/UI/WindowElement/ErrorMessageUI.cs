using UnityEngine;

namespace Runtime.UI
{
    public class ErrorMessageUI : TextUI
    {
        [SerializeField] private float displayDuration = 5f;
        [SerializeField] private float fadeInSpeed = 1f;
        [SerializeField] private float fadeOutSpeed = 1f;

        public sealed override void Initialize() 
        {
            base.Initialize();
        }

        protected sealed override void OnSetMessage(bool useEffect)
        {
            base.OnSetMessage();

            Fade(displayDuration, fadeInSpeed, fadeOutSpeed);
        }
    }
}