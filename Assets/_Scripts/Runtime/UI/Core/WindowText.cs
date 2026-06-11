using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utility.Debug;

namespace Runtime.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class WindowText : WindowElement
    {
        [SerializeField] private List<UIEffect> effects;
        private TMP_Text _textUI;
        private Coroutine _fadeTimer;

        private void OnDisable()
        {
            if (_textUI != null)
                _textUI.text = string.Empty;

            if (effects != null && effects.Count != 0)
            {
                foreach (var effect in effects)
                    effect.InactiveEffect();
            }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            this._textUI = GetComponent<TMP_Text>();

            CustomLog.Assert(_textUI != null, "_textUI is null");
        }

        public void SetMessage(string text, Color textColor = default, bool useEffect = true)
        {
            this._textUI.color = textColor;
            this._textUI.text = text;

            OnSetMessage(useEffect);
        }

        protected virtual void OnSetMessage(bool useEffect = true)
        {
            if (useEffect)
                foreach (var effect in effects)
                    effect.ActiveEffect();
        }

        //public void Fade(float endValue, float transitionDuration)
        //{
        //    _messageCompo.DOKill();
        //    _messageCompo.DOFade(endValue, transitionDuration)
        //        .onComplete += ()=> 
        //        {
        //            if (endValue == 0)
        //                gameObject.SetActive(false);
        //        };
        //}

        //public void FadeInAndOut(float displayDuration, float inTransitionDuration, float outTransitionDuration)
        //{
        //    _messageCompo.DOKill();
        //    _messageCompo.DOFade(1f, inTransitionDuration);

        //    if (_fadeTimer != null)
        //    {
        //        StopCoroutine(_fadeTimer);
        //        _fadeTimer = null;
        //    }

        //    _fadeTimer = StartCoroutine(FadeTimer(displayDuration, outTransitionDuration));
        //}

        //private IEnumerator FadeTimer(float displayDuration, float outTransitionDuration)
        //{
        //    yield return new WaitForSeconds(displayDuration);
        //    _messageCompo.DOFade(0f, outTransitionDuration)
        //        .onComplete += ()=> gameObject.SetActive(false);
        //    _fadeTimer = null;
        //}
    }
}
