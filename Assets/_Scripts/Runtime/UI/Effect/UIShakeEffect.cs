using DG.Tweening;
using UnityEngine;

namespace Runtime.UI
{
    public class UIShakeEffect : MonoBehaviour, IUIEffect
    {
        [SerializeField] private float duration;
        [SerializeField] private float strength;
        [SerializeField] private int vibrancy;

        private RectTransform _shackRTrm;
        private Vector2 _initAnchorPos;

        private void Awake()
        {
            this._shackRTrm = GetComponent<RectTransform>();
            this._initAnchorPos = _shackRTrm.anchoredPosition;
        }

        [ContextMenu("Active Effect")]
        public void ActiveEffect()
        {
            ShakePasswordInput();
        }

        private void ShakePasswordInput()
        {
            _shackRTrm.DOKill();
            _shackRTrm.anchoredPosition = _initAnchorPos;

            _shackRTrm.DOShakePosition(duration, new Vector2(strength, 0), vibrancy);
        }
    }
}
