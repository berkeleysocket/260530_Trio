using UnityEngine;
using TMPro;
using System.Collections;
using DG.Tweening;

namespace Runtime.UI
{
    public class UIEffect_TextBlink : MonoBehaviour, IUIEffect
    {
        [SerializeField] private TMP_Text textCompo;
        [SerializeField] private float blinkInterval = 1f;         
        [Range(0f, 1f)]
        [SerializeField] private float minAlpha = 0.5f;

        private Coroutine _effectRoutine;

        public void Initialize()
        {

        }

        public void ActiveEffect()
        {
            if(_effectRoutine == null)
                _effectRoutine = StartCoroutine(Blink());
        }

        public void InactiveEffect()
        {
            if (_effectRoutine != null)
                StopCoroutine(_effectRoutine);

            textCompo.alpha = 1f;
        }

        private IEnumerator Blink()
        {
            Color color = textCompo.color;
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
                
                textCompo.color = color;

                yield return new WaitForSeconds(blinkInterval);
            }
        }

    }
}
