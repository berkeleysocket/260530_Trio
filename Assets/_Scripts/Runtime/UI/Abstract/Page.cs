using DG.Tweening;
using System;
using UnityEngine;
namespace Runtime.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Page : MonoBehaviour, IWindow
    {
        [SerializeField] private float fadeInSpeed = 1f;
        [SerializeField] private float fadeOutSpeed = 1f;

        private CanvasGroup _canvasGroup;

        public virtual void Initialize()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            FadeIn();
        }

        public void Hide()
        {
            gameObject.SetActive(true);
            FadeOut();
        }

        private void FadeIn()
        {
            _canvasGroup.DOKill();
            _canvasGroup.DOFade(1, fadeInSpeed);
        }

        private void FadeOut()
        {
            _canvasGroup.DOKill();
            _canvasGroup.DOFade(0, fadeOutSpeed);
        }
    }
}