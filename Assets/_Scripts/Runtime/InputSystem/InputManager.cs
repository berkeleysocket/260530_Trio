using UnityEngine;
using System.Collections.Generic;

using Runtime.Pattern;

namespace Runtime.InputSystem
{
    public class InputManager : MonoSingleton<InputManager>
    {
        [SerializeField] private List<InputReaderBaseSO> readers;
        private PlayerInputActions _inputActions;

        private void Awake()
        {
            Initialize();
        }

        private void OnDisable()
        {
            ReleaseReaders();
        }

        private void OnApplicationQuit()
        {
            ReleaseReaders();
        }

        public void Initialize()
        {
            _inputActions = new PlayerInputActions();
        }

        public void InitializeReaders()
        {
            foreach (var reader in readers)
                reader.Initialize(_inputActions);
        }

        public void ReleaseReaders()
        {
            foreach (var reader in readers)
                reader.Release();
        }
    }
}