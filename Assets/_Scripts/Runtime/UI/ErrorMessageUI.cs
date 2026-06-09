using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Runtime.UI
{
    public class ErrorMessageUI : MonoBehaviour
    {
        [SerializeField] private float displayDuration = 5f;
        private UIShakeEffect _shakeEffect;
        private TMP_Text _messageUI;
        private Coroutine _fadeOutTimer;

        private void OnDisable()
        {
            if (_fadeOutTimer != null)
            {
                StopCoroutine(_fadeOutTimer);
                _fadeOutTimer = null;
            }

            if (_shakeEffect != null)
                _shakeEffect.DOKill();

            if (_messageUI != null)
                _messageUI.text = string.Empty;
        }

        public void Initialize()
        {
            this._shakeEffect = GetComponent<UIShakeEffect>();
            this._messageUI = GetComponent<TMP_Text>();

            this._messageUI.alpha = 0;
        }

        public void SetMessage(string message, Color color)
        {
            if (_fadeOutTimer != null)
            {
                _messageUI.DOKill();
                StopCoroutine(_fadeOutTimer);
                _fadeOutTimer = null;
            }

            _messageUI.text = message;
            _messageUI.color = color;
            _messageUI.alpha = 1;

            _shakeEffect.ActiveEffect();
            _fadeOutTimer = StartCoroutine(FadeOutTimer());
        }

        private IEnumerator FadeOutTimer()
        {
            yield return new WaitForSeconds(displayDuration);
            _messageUI.DOFade(0, 1f);
        }
    }
}