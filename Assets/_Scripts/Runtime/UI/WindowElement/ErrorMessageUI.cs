using UnityEngine;

namespace Runtime.UI
{
    public class ErrorMessageUI : WindowText
    {
        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

        protected sealed override void OnSetMessage(bool useEffect)
        {
            base.OnSetMessage();

            ShowAndHide();
        }
    }
}