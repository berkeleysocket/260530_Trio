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
        [SerializeField] private bool InitializeAll = false;
        private Dictionary<Type, InputReaderBaseSO> _readers;
        private PlayerInputActions _inputActions;

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
            _readers = new Dictionary<Type, InputReaderBaseSO>();

            foreach (InputReaderBaseSO reader in readerRegistry)
            {
                _readers[reader.GetType()] = reader;
            }

            #region 테스트 코드
            //if (InitializeAll)  
            //    InitializeAllReader();

            EventChannel.AddListener<JumpInputEvent>((evtArgs) => CustomLog.LogSuccess("Input Jump Key"));
            EventChannel.AddListener<MoveInputEvent>((evtArgs) => CustomLog.LogSuccess("Input Move Key"));

            EventChannel.AddListener<AnyKeyInputEvent>((evtArgs) => CustomLog.LogSuccess("Input Any Key"));
            #endregion
        }

        public void ReleaseReader<T>() where T : InputReaderBaseSO
        {
            if (_readers != null && _readers.Count != 0)
            {
                _readers.TryGetValue(typeof(T), out InputReaderBaseSO reader);
                reader.Release();
            }
        }

        public void InitializeAllReader()
        {
            if (_readers != null && _readers.Count != 0)    
                foreach (InputReaderBaseSO reader in _readers.Values)
                    reader.Initialize(_inputActions);
        }

        public void ReleaseAllReader()
        {
            if(_readers != null && _readers.Count != 0)
                foreach (InputReaderBaseSO reader in _readers.Values)
                    reader.Release();
        }
    }
}