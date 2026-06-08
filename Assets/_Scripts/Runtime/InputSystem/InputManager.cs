using UnityEngine;
using System.Collections.Generic;

using Runtime.Pattern;
using System;

namespace Runtime.InputSystem
{
    public class InputManager : MonoSingleton<InputManager>
    {
        [SerializeField] private List<InputReaderBaseSO> readerRegistry;
        private Dictionary<Type, InputReaderBaseSO> _readers;
        private PlayerInputActions _inputActions;

        private void Awake()
        {
            Initialize();
        }

        private void OnDisable()
        {
            ReleaseAllReader();
        }

        private void OnApplicationQuit()
        {
            ReleaseAllReader();
        }

        public void Initialize()
        {
            _inputActions = new PlayerInputActions();

            foreach(InputReaderBaseSO reader in readerRegistry)
            {
                Type t = reader.GetType();
                _readers[t] = reader;
            }
        }

        public void ReleaseReader<T>() where T : InputReaderBaseSO
        {
            Type t = typeof(T);
            _readers.TryGetValue(t, out InputReaderBaseSO reader);
            reader?.Release();
        }

        public void InitializeAllReader()
        {
            foreach (InputReaderBaseSO reader in _readers.Values)
                reader.Initialize(_inputActions);
        }

        public void ReleaseAllReader()
        {
            foreach (InputReaderBaseSO reader in _readers.Values)
                reader.Release();
        }
    }
}