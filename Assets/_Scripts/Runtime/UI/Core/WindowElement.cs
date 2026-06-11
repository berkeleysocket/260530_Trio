using DG.Tweening;
using Runtime.Pattern;
using System;
using UnityEngine;
using Utility.Debug;

namespace Runtime.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class WindowElement : MonoBehaviour, IInitializable
    {
        [SerializeField] private UIVisibilityDataSO uiVisibilityData;
        private float _fadeInDuration;
        private float _fadeOutDuration;
        private float _displayDuration;
        //private Action onShown;
        private CanvasGroup _group;
        private Sequence _currentSequence;
        private Sequence _fadeInSequence;
        private Sequence _fadeOutSequence;
        private Sequence _fadeInAndOutSequence;

        public void Initialize()
        {
            this._group = GetComponent<CanvasGroup>();
            this._fadeInDuration = uiVisibilityData.FadeInDuration;
            this._fadeOutDuration = uiVisibilityData.FadeOutDuration;
            this._displayDuration = uiVisibilityData.DisplayTime;

            this._fadeInSequence = DOTween.Sequence()
                .Append(_group.DOFade(1, _fadeInDuration))
                .SetAutoKill(false)
                .Pause();

            this._fadeOutSequence = DOTween.Sequence()
                .Append(_group.DOFade(0, _fadeOutDuration))
                .OnComplete(() => gameObject.SetActive(false))
                .SetAutoKill(false)
                .Pause();

            this._fadeInAndOutSequence = DOTween.Sequence()
                .Append(_group.DOFade(1, _fadeInDuration))
                .AppendInterval(_displayDuration)
                .Append(_group.DOFade(0, _fadeOutDuration))
                .OnComplete(() => gameObject.SetActive(false))
                .SetAutoKill(false)
                .Pause();

            OnInitialize();

            CustomLog.Assert(_group != null, $"{gameObject.name} : _group is null");
        }

        protected virtual void OnInitialize() { }

        public void Show()
        {
            if (gameObject.activeSelf)
                return;

            gameObject.SetActive(true);

            _currentSequence.Pause();
            _currentSequence = _fadeInSequence.Play();
        }

        public void Hide()
        {
            if (!gameObject.activeSelf)
                return;

            _currentSequence.Pause();
            _currentSequence = _fadeOutSequence.Play();
        }

        public void ShowAndHide()
        {
            if (gameObject.activeSelf)
                return;

            gameObject.SetActive(true);

            _currentSequence.Pause();
            _currentSequence = _fadeInAndOutSequence.Play();
        }
    }
}
