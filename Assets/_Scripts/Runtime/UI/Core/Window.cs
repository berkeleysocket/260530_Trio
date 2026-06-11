using DG.Tweening;
using Runtime.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility.Debug;

namespace Runtime.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Window : MonoBehaviour, IInitializable
    {
        [SerializeField] private List<WindowElementDirector> windowElementList;
        [SerializeField] private UIVisibilityDataSO uiVisibilityData;
        private float _fadeInDuration;
        private float _fadeOutDuration;
        private CanvasGroup _group;
        private Dictionary<Type, WindowElementDirector> windowElements;
        private Sequence _currentSequence;
        private Sequence _fadeInSequence;
        private Sequence _fadeOutSequence;

        public void Initialize()
        {
            this._group = GetComponent<CanvasGroup>();
            this._fadeInDuration = uiVisibilityData.FadeInDuration;
            this._fadeOutDuration = uiVisibilityData.FadeOutDuration;

            this._fadeInSequence = DOTween.Sequence()
                .Append(_group.DOFade(1, _fadeInDuration))
                .SetAutoKill(false)
                .Pause();

            this._fadeOutSequence = DOTween.Sequence()
                .Append(_group.DOFade(0, _fadeOutDuration))
                .OnComplete(() => gameObject.SetActive(false))
                .SetAutoKill(false)
                .Pause();

            windowElements = windowElementList.ToDictionary(
                (element) => element.GetType(),
                (element) => element);

            foreach (var element in windowElements.Values)
                element.Initialize();


            OnInitialize();
            CustomLog.Assert(_group != null, $"{gameObject.name} : _group is null");
        }

        public virtual void OnInitialize() { }

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
    }
}
