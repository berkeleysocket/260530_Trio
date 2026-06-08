using Runtime.Utility.EventChannel;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.InputSystem
{
    [CreateAssetMenu(fileName = "UIInputReader", menuName = "KSY/SO/InputReader/UIInputReader")]
    public class UIInputReader : InputReaderBaseSO, PlayerInputActions.IUIActions
    {
        private PlayerInputActions _inputActions;

        public override void Initialize(PlayerInputActions inputActions)
        {
            this._inputActions = inputActions;

            _inputActions.UI.AddCallbacks(this);
            _inputActions.Enable();
        }

        public override void Release()
        {
            _inputActions.UI.RemoveCallbacks(this);
            _inputActions.UI.Disable();
        }

        public override InputActionMap GetInputActionMap() => _inputActions.UI;

        public void OnPressAnyKey(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                EventChannel.InvokeEvent(new AnyKeyInputEvent());
            }
        }
    }
}
