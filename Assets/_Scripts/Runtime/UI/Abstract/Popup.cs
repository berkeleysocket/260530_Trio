using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Runtime.UI
{
    public abstract class Popup : MonoBehaviour, IWindow
    {
        [SerializeField] private Vector2 shownPos;
        [SerializeField] private Vector2 hiddenPos;
        [SerializeField] private RectTransform popupRect;
        [SerializeField] private float duration;
        private CanvasGroup _popupGroup;
        private Sequence _showSequence;
        private Sequence _hideSequence;
        private Coroutine _hideTimer;
        private float _duration = 1f;

        public virtual void Initialize()
        {
            _popupGroup = GetComponent<CanvasGroup>();

            _showSequence = DOTween.Sequence()
                .Append(_popupGroup.DOFade(1f, 1))
                .Join(popupRect.DOAnchorPos(shownPos, _duration)
                .SetEase(Ease.OutBack)
                .OnComplete(OnShowedComplete))
                .SetAutoKill(false)
                .Pause();

            _hideSequence = DOTween.Sequence()
                .Append(_popupGroup.DOFade(0f, 1))
                .Join(popupRect.DOAnchorPos(hiddenPos, _duration)
                .SetEase(Ease.InBack))
                .OnComplete(() => gameObject.SetActive(false))
                .SetAutoKill(false)
                .Pause();

            popupRect.anchoredPosition = hiddenPos;
            _popupGroup.alpha = 0;
            _popupGroup.interactable = false;
        }

        public void Show()
        {
            _hideSequence.Pause();
            _showSequence.Restart();
        }

        private void OnShowedComplete()
        {
            _popupGroup.interactable = true;

            if (_hideTimer != null)
            {
                StopCoroutine(_hideTimer);
                _hideTimer = null;
            }

            _hideTimer = StartCoroutine(HideTimer());
        }

        public void Hide()
        {
            if (_hideTimer != null) return;

            _popupGroup.interactable = false;
            _showSequence.Pause();
            _hideSequence.Restart();
        }

        private IEnumerator HideTimer()
        {
            yield return new WaitForSeconds(duration);

            _hideTimer = null;
            Hide();
        }
    }
}