using Runtime.Utility.EventChannel;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.InputSystem
{
    [CreateAssetMenu(fileName = "PlayerInputReader", menuName = "KSY/SO/InputReader/PlayerInputReader")]
    public class PlayerInputReader : InputReaderBaseSO, PlayerInputActions.IPlayerActions
    {
        private PlayerInputActions _inputActions;

        public override void Initialize(PlayerInputActions inputActions)
        {
            this._inputActions = inputActions;

            _inputActions.Player.AddCallbacks(this);
            _inputActions.Enable();
        }

        public override void Release()
        {
            if(_inputActions != null)
            {
                _inputActions.Player.RemoveCallbacks(this);
                _inputActions.Disable();
            }
        }

        public override InputActionMap GetInputActionMap() => _inputActions?.Player;
        
        public void OnJump(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                EventChannel.InvokeEvent(new JumpInputEvent());
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if(context.started || context.canceled)
            {
                float input = context.ReadValue<float>();
                EventChannel.InvokeEvent(new MoveInputEvent(input));
            }
        }
    }
}