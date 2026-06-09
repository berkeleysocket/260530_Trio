using DG.Tweening;
using UnityEngine;

namespace Runtime.UI
{
    public class UIShakeEffect : MonoBehaviour
    {
        [SerializeField] private RectTransform passwordInput;

        private void ShakePasswordInput()
        {
            passwordInput.DOKill();
            passwordInput.anchoredPosition = Vector2.zero;

            // 부르르 연출
            passwordInput.DOShakePosition(0.5f, new Vector2(25f, 0f), 15);
        }
    }
}
