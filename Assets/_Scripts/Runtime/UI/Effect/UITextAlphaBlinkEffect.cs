using UnityEngine;
using TMPro;
using System.Collections;

namespace Runtime.UI
{
    public class UITextAlphaBlinkEffect : MonoBehaviour, IUIEffect
    {
        [SerializeField] private float blinkInterval = 1f;         
        [Range(0f, 1f)]
        [SerializeField] private float minAlpha = 0.5f;         

        private TMP_Text _textComponent;

        #region 임시 코드
        private void Awake()
        {
            _textComponent = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            ActiveEffect();
        }
        #endregion

        [ContextMenu("Active Effect")]
        public void ActiveEffect()
        {
            StartCoroutine(AnimateAlpha());
        }

        private IEnumerator AnimateAlpha()
        {
            Color color = _textComponent.color;
            bool flip = false;

            while(true)
            {
                if(color.a <= minAlpha)
                    flip = true;
                else if(color.a >= 1f)
                    flip = false;

                if (flip)
                    color.a += 0.1f;
                else
                    color.a -= 0.1f;
                
                _textComponent.color = color;

                yield return new WaitForSeconds(blinkInterval);
            }
        }

    }
}
