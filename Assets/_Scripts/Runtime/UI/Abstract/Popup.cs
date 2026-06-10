using DG.Tweening;
using UnityEngine;

namespace Runtime.UI
{
    public abstract class Popup : MonoBehaviour, IWindow
    {
        private RectTransform _popupRect;
        private CanvasGroup _popupGroup;
        private Sequence _mySequence;
        private float _duration = 1f;
        private Vector2 _shownPos;
        private Vector2 _hiddenPos;

        public virtual void Initialize()
        {
            _popupRect = GetComponent<RectTransform>();
            _popupGroup = GetComponent<CanvasGroup>();
            _mySequence = DOTween.Sequence();

            _popupRect.anchoredPosition = _hiddenPos;
        }

        public void Show()
        {
            ShowPopup();
        }

        public void Hide()
        {
            HidePopup();
        }

        private void ShowPopup()
        {
            _popupRect.DOKill();

            _mySequence
                .Join(_popupGroup.DOFade(1f, 1))
                .Join(_popupRect.DOAnchorPos(_shownPos, _duration)
                .SetEase(Ease.OutBack)
                .OnComplete(() => _popupGroup.interactable = true));
        }

        private void HidePopup()
        {
            _popupGroup.interactable = false;
            _popupRect.DOKill();

            _mySequence.Join(_popupGroup.DOFade(0f, 1))
                .Join(_popupRect.DOAnchorPos(_hiddenPos, _duration)
                .SetEase(Ease.InBack))
                .OnComplete(() => gameObject.SetActive(false));
        }

        #region Editor
#if UNITY_EDITOR
        [ContextMenu("Set Current Position As Show Position")]
        private void SetShownPosition() => _shownPos = GetComponent<RectTransform>().position;
        [ContextMenu("Set Current Position As Hide Position")]
        private void SetHiddenPosition() => _hiddenPos = GetComponent<RectTransform>().position;
#endif
        #endregion
    }
}