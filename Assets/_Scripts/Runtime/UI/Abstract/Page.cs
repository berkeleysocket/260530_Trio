using System;
using System.Collections;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.UI
{
    public abstract class Page : MonoBehaviour
    {
        //[field: SerializeField] public UnityEvent OnFadeOutComplete { get; private set; }
        //[field: SerializeField] public UnityEvent OnFadeInComplete { get; private set; }

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

            OnInitialized();
        }

        protected virtual void OnInitialized() { }

        [ContextMenu("Fade In")]
        public void FadeIn()
        {
            if (_currentFadeCoroutine == null)
            {
                _canvasGroup.interactable = false;
                _currentFadeCoroutine = StartCoroutine(FadeInCoroutine());
            }
        }

        [ContextMenu("Fade Out")]
        public void FadeOut()
        {
            if(_currentFadeCoroutine == null)
            {
                _canvasGroup.interactable = false;
                _currentFadeCoroutine = StartCoroutine(FadeOutCoroutine());
            }
        }

        private IEnumerator FadeInCoroutine()
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
            //OnFadeInComplete?.Invoke();
        }

        private IEnumerator FadeOutCoroutine()
        {
            float a = 1;
            while(a > 0)
            {
                a = _canvasGroup.alpha - 0.01f * fadeOutSpeed;
                _canvasGroup.alpha = a;

                yield return null;
            }

            _currentFadeCoroutine = null;
            //OnFadeOutComplete?.Invoke();
        }
    }
}