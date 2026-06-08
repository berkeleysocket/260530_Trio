using UnityEngine;
using System.Collections.Generic;

namespace Runtime.InputSystem
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private List<InputReaderBaseSO> _readers;
        private PlayerInputActions _inputActions;

        private void Awake()
        {
            Initialize();
        }

        private void OnDisable()
        {
            Release();
        }

        private void OnApplicationQuit()
        {
            Release();
        }

        public void Initialize()
        {
            _inputActions = new PlayerInputActions();

            foreach(var reader in _readers)
            {
                reader.Initialize(_inputActions);
            }
        }

        public void Release()
        {
            foreach (var reader in _readers)
                reader.Release();
        }
    }
}