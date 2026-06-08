using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.InputSystem
{
    public abstract class InputReaderBaseSO : ScriptableObject
    {
        public abstract void Initialize(PlayerInputActions inputActions);
        public abstract void Release();
        public abstract InputActionMap GetInputActionMap();
    }
}