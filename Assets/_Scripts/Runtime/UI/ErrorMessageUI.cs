using UnityEngine;
using System.Collections.Generic;

namespace Runtime.UI
{
    [RequireComponent(typeof(UIShakeEffect))]
    public class ErrorMessageUI : TextUI
    {
        [SerializeField] private List<IUIEffect> effects;
        [SerializeField] private float displayDuration = 5f;
        [SerializeField] private float fadeInSpeed = 1f;
        [SerializeField] private float fadeOutSpeed = 1f;

        private void OnDisable()
        {
            if(effects != null && effects.Count != 0)
            {
                foreach (var effect in effects)
                    effect.InactiveEffect();
            }
        }

        public override void Initialize() { }

        public override void OnSetMessage()
        {
            Fade(displayDuration, fadeInSpeed, fadeOutSpeed);

            foreach (var effect in effects)
                effect.ActiveEffect();
        }
    }
}