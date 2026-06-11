using DG.Tweening;
using UnityEngine;
using Utility.Debug;

namespace Runtime.UI
{
    public class UIEffect_Shake : UIEffect
    {
        [SerializeField] private float duration;
        [SerializeField] private float strength;
        [SerializeField] private int vibrancy;

        private RectTransform _targetRectTransform;
        private Vector2 _initAnchorPos;

        private void Awake()
        {
            Debug.LogWarning("임시 초기화");
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
            this._targetRectTransform = GetComponent<RectTransform>();
            this._initAnchorPos = _targetRectTransform.anchoredPosition;

            CustomLog.Assert(_targetRectTransform != null, "_targetRectTransform is null");
        }  

        public override void ActiveEffect()
        {
            Shake();
        }

        public override void InactiveEffect()
        {
            _targetRectTransform.DOKill();
        }

        private void Shake()
        {
            _targetRectTransform.DOKill();
            _targetRectTransform.anchoredPosition = _initAnchorPos;
            _targetRectTransform.DOShakePosition(duration, new Vector2(strength, 0), vibrancy);
        }
    }
}
