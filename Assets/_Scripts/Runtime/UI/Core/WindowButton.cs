using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class WindowButton : WindowElement
    {
        //[SerializeField] private List<ButtonEffect> buttonEffectList
        private Button _button;

        protected override void OnInitialize()
        {
            _button = GetComponent<Button>();

            //_button.onClick 버튼 이펙트 실행 메서드 WindowButton을 인자값으로 넘겨주고 구현하기.
        }
    }
}
