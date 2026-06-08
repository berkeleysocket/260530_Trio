using System;
using System.Collections;
using UnityEditor.Build;
using UnityEngine;

namespace Runtime.UI
{
    public abstract class Page : MonoBehaviour
    {
        [SerializeField] private float fadeInSpeed = 1f;
        [SerializeField] private float fadeOutSpeed = 1f;

        private Coroutine _currentFadeCoroutine;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        [ContextMenu("Fade In")]
        public void FadeIn(Action onCompleted = null)
        {
            if (_currentFadeCoroutine == null)
            {
                _canvasGroup.interactable = false;
                _currentFadeCoroutine = StartCoroutine(FadeInCoroutine(onCompleted));
            }
        }

        [ContextMenu("Fade Out")]
        public void FadeOut(Action onCompleted = null)
        {
            if(_currentFadeCoroutine == null)
            {
                _canvasGroup.interactable = false;
                _currentFadeCoroutine = StartCoroutine(FadeOutCoroutine(onCompleted));
            }
        }

        private IEnumerator FadeInCoroutine(Action onCompleted)
        {
            float a = 0;
            while (a < 1)
            {
                a = _canvasGroup.alpha + 0.01f * fadeInSpeed;
                _canvasGroup.alpha = a;

                yield return null;
            }

            _canvasGroup.interactable = true;
            _currentFadeCoroutine = null;
            onCompleted?.Invoke();
        }

        private IEnumerator FadeOutCoroutine(Action onCompleted)
        {
            float a = 1;
            while(a > 0)
            {
                a = _canvasGroup.alpha - 0.01f * fadeOutSpeed;
                _canvasGroup.alpha = a;

                yield return null;
            }

            _currentFadeCoroutine = null;
            onCompleted?.Invoke();
        }
    }
}
