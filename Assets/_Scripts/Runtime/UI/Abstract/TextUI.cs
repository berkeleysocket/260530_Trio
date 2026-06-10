using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utility.Debug;

namespace Runtime.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public abstract class TextUI : MonoBehaviour, IWindowElement
    {
        [SerializeField] private List<UIEffect> effects;
        private TMP_Text _messageCompo;
        private Coroutine _fadeTimer;

        private void OnDisable()
        {
            if (_messageCompo != null)
                _messageCompo.text = string.Empty;

            if (effects != null && effects.Count != 0)
            {
                foreach (var effect in effects)
                    effect.InactiveEffect();
            }
        }

        public virtual void Initialize()
        {
            CustomLog.Log("Initialize");
            this._messageCompo = GetComponent<TMP_Text>();

            CustomLog.Assert(_messageCompo != null, "_messageCompo is null");
        }

        public void SetMessage(string text, Color textColor = default, bool useEffect = true)
        {
            this._messageCompo.color = textColor;
            this._messageCompo.text = text;

            OnSetMessage(useEffect);
        }

        protected virtual void OnSetMessage(bool useEffect = true)
        {
            if(useEffect)
                foreach (var effect in effects)
                    effect.ActiveEffect();
        }

        public void Fade(float endValue, float transitionDuration)
        {
            _messageCompo.DOKill();
            _messageCompo.DOFade(endValue, transitionDuration);
        }

        public void Fade(float displayDuration, float inTransitionDuration, float outTransitionDuration)
        {
            _messageCompo.DOKill();
            _messageCompo.DOFade(1f, inTransitionDuration);

            if (_fadeTimer != null)
            {
                StopCoroutine(_fadeTimer);
                _fadeTimer = null;
            }

            _fadeTimer = StartCoroutine(FadeTimer(displayDuration, outTransitionDuration));
        }

        private IEnumerator FadeTimer(float displayDuration, float outTransitionDuration)
        {
            yield return new WaitForSeconds(displayDuration);
            _messageCompo.DOFade(0f, outTransitionDuration);
        }
    }
}
