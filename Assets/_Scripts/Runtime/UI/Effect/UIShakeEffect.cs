using DG.Tweening;
using UnityEngine;

namespace Runtime.UI
{
    public class UIShakeEffect : MonoBehaviour, IUIEffect
    {
        [SerializeField] private RectTransform shackRTrm;

        [ContextMenu("Active Effect")]
        public void ActiveEffect()
        {
            ShakePasswordInput();
        }

        private void ShakePasswordInput()
        {
            shackRTrm.DOKill();
            shackRTrm.anchoredPosition = Vector2.zero;

            shackRTrm.DOShakePosition(0.5f, new Vector2(25f, 0f), 15);
        }
    }
}
