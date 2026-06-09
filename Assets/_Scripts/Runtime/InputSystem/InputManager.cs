using UnityEngine;
using System.Collections.Generic;
using Runtime.Pattern;
using System;
using Utility.Debug;
using Runtime.Utility.EventChannel;

namespace Runtime.InputSystem
{
    public class InputManager : MonoSingleton<InputManager>
    {
        [SerializeField] private List<InputReaderBaseSO> readerRegistry;
        private Dictionary<Type, InputReaderBaseSO> _readers;
        private Dictionary<Type, bool> _isReaderRegistered;
        private PlayerInputActions _inputActions;

        private void OnDisable()
        {
            DisableAllReader();
        }

        private void OnApplicationQuit()
        {
            DisableAllReader();
        }

        public void Initialize()
        {
            _inputActions = new PlayerInputActions();
            _readers = new Dictionary<Type, InputReaderBaseSO>();
            _isReaderRegistered = new Dictionary<Type, bool>();

            foreach (InputReaderBaseSO reader in readerRegistry)
            {
                Type key = reader.GetType();
                _readers[key] = reader;
                _isReaderRegistered[key] = false;

                reader.Initialize(_inputActions);
            }

            #region 테스트 코드
            EventChannel.AddListener<JumpInputEvent>((evtArgs) => CustomLog.LogSuccess("Input Jump Key"));
            EventChannel.AddListener<MoveInputEvent>((evtArgs) => CustomLog.LogSuccess("Input Move Key"));

            EventChannel.AddListener<AnyKeyInputEvent>((evtArgs) => CustomLog.LogSuccess("Input Any Key"));
            #endregion
        }

        public void EnableReader<T>() where T : InputReaderBaseSO
        {
            if (_readers != null && _readers.Count != 0)
            {
                Type key = typeof(T);
                _readers.TryGetValue(key, out InputReaderBaseSO reader);
                _isReaderRegistered.TryGetValue(key, out bool isRegistered);

                if (!isRegistered)
                {
                    reader.Enable();
                    _isReaderRegistered[key] = true;
                }
            }
        }

        public void DisableReader<T>() where T : InputReaderBaseSO
        {
            if (_readers != null && _readers.Count != 0)
            {
                Type key = typeof(T);
                _readers.TryGetValue(key, out InputReaderBaseSO reader);
                _isReaderRegistered.TryGetValue(key, out bool isRegistered);

                if(isRegistered)
                {
                    reader.Disable();
                    _isReaderRegistered[key] = false;
                }
            }
        }

        public void EnableAllReader()
        {
            if (_readers != null && _readers.Count != 0)
            {
                foreach (InputReaderBaseSO reader in _readers.Values)
                {
                    Type key = reader.GetType();
                    reader.Enable();
                    _isReaderRegistered[key] = true;
                }
            }
        }

        public void DisableAllReader()
        {
            if (_readers != null && _readers.Count != 0)
            {
                foreach (InputReaderBaseSO reader in _readers.Values)
                {
                    Type key = reader.GetType();
                    reader.Disable();
                    _isReaderRegistered[key] = false;
                }
            }
        }
    }
}