using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.InputSystem
{
    [CreateAssetMenu(fileName = "PlayerInputReader", menuName = "KSY/SO/InputReader/PlayerInputReader")]
    public class PlayerInputReader : InputReaderBaseSO, PlayerInputActions.IPlayerActions
    {
        public event Action<float> OnMoved;
        public event Action OnJumped;

        private PlayerInputActions _inputActions;

        public override void Initialize(PlayerInputActions inputActions)
        {
            this._inputActions = inputActions;

            _inputActions.Player.AddCallbacks(this);
            _inputActions.Enable();
        }

        public override void Release()
        {
            _inputActions.Player.RemoveCallbacks(this);
            _inputActions.Disable();
        }

        public override InputActionMap GetInputActionMap() => _inputActions?.Player;
        
        public void OnJump(InputAction.CallbackContext context)
        {
            OnJumped?.Invoke();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            float input = context.ReadValue<float>();
            OnMoved?.Invoke(input);
        }
    }
}