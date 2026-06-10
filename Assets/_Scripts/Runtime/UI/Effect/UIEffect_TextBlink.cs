using UnityEngine;
using TMPro;
using System.Collections;
using Utility.Debug;

namespace Runtime.UI
{
    public class UIEffect_TextBlink : UIEffect
    {
        [SerializeField] private float blinkInterval = 1f;         
        [Range(0f, 1f)]
        [SerializeField] private float minAlpha = 0.5f;

        private TMP_Text _targetText;
        private Coroutine _effectRoutine;

        public override void Initialize()
        {
            base.Initialize();

            _targetText = GetComponent<TMP_Text>();

            CustomLog.Assert(_targetText != null, "_targetText is null");
        }

        public override void ActiveEffect()
        {
            if(_effectRoutine == null)
                _effectRoutine = StartCoroutine(Blink());
        }

        public override void InactiveEffect()
        {
            if (_effectRoutine != null)
                StopCoroutine(_effectRoutine);

            _targetText.alpha = 1f;
        }

        private IEnumerator Blink()
        {
            Color color = _targetText.color;
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
                
                _targetText.color = color;

                yield return new WaitForSeconds(blinkInterval);
            }
        }

    }
}
