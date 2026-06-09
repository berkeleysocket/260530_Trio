using System;
using System.Collections;
using UnityEngine;
namespace Runtime.UI
{
    public abstract class Page : MonoBehaviour
    {
        [SerializeField] private float fadeInSpeed = 1f;
        [SerializeField] private float fadeOutSpeed = 1f;

        private event Action onEnable;
        private Coroutine _currentFadeCoroutine;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            Initialize();
        }

        private void OnEnable()
        {
            onEnable?.Invoke();
        }
        public void Initialize()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            OnInitialized();
        }

        protected virtual void OnInitialized() { }

        public void Show(bool useFade)
        {
            gameObject.SetActive(true);

            if (useFade)
                onEnable += FadeIn;
        }

        public void Hide(bool useFade)
        {
            if (!gameObject.activeSelf) return;
            if (useFade)
                FadeOut();
            else
                gameObject.SetActive(false);
        }

        private void FadeIn()
        {
            onEnable -= FadeIn;
            if (_currentFadeCoroutine != null)
            {
                StopCoroutine(_currentFadeCoroutine);
                _currentFadeCoroutine = null;
                _canvasGroup.interactable = false;
            }
            _currentFadeCoroutine = StartCoroutine(FadeInCoroutine());
        }

        private void FadeOut()
        {
            onEnable -= FadeOut;
            if (_currentFadeCoroutine != null)
            {
                StopCoroutine(_currentFadeCoroutine);
                _currentFadeCoroutine = null;
                _canvasGroup.interactable = false;
            }
            _currentFadeCoroutine = StartCoroutine(FadeOutCoroutine());
        }

        private IEnumerator FadeInCoroutine()
        {
            float a = _canvasGroup.alpha;
            while (a < 1)
            {
                a = _canvasGroup.alpha + 0.01f * fadeInSpeed;
                _canvasGroup.alpha = a;

                yield return null;
            }

            _canvasGroup.interactable = true;
            _currentFadeCoroutine = null;
        }

        private IEnumerator FadeOutCoroutine()
        {
            float a = _canvasGroup.alpha;
            while (a > 0)
            {
                a = _canvasGroup.alpha - 0.01f * fadeOutSpeed;
                _canvasGroup.alpha = a;

                yield return null;
            }

            _currentFadeCoroutine = null;
            gameObject.SetActive(false);
        }
    }
}