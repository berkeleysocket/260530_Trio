using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Clients.Players
{
    [CreateAssetMenu(fileName = "InputSO", menuName = "KSY/SO/InputSO")]
    public class InputSO : ScriptableObject, @InputActions.IPlayerActions
    {
        public event Action<float> OnMoved;
        public event Action OnJumped;

        private @InputActions _inputActions;

        private void OnEnable()
        {
            _inputActions = new @InputActions();

            _inputActions.Player.AddCallbacks(this);
            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }

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