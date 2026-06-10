using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using Utility.Debug;

namespace Runtime.UI
{
    public abstract class TextUI : MonoBehaviour, IWindowElement
    {
        private TMP_Text _messageCompo;
        private Coroutine _fadeTimer;

        private void OnDisable()
        {
            if (_messageCompo != null)
                _messageCompo.text = string.Empty;
        }

        public virtual void Initialize()
        {
            this._messageCompo = GetComponent<TMP_Text>();

            CustomLog.Assert(_messageCompo != null, "_messageCompo is null");
        }

        public void SetMessage(string text, Color textColor = default)
        {
            this._messageCompo.color = textColor;
            this._messageCompo.text = text;

            OnSetMessage();
        }

        public abstract void OnSetMessage();

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
