using Runtime.Clients.Agents;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using UnityEngine;

namespace Runtime.Clients.ModuleSystem
{
    public abstract class ModuleOwner : MonoBehaviour
    {
        private Dictionary<Type, IModule> _modules; 

        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            _modules = new Dictionary<Type, IModule>();
            _modules = GetComponentsInChildren<IModule>().ToDictionary(key => key.GetType(), module => module);

            foreach (IModule module in _modules.Values)
                module.Initialize(this);
        }

        protected T GetModule<T>() where T : class, IModule
        {
            if(_modules.TryGetValue(typeof(T), out IModule module))
            {
                return module as T;
            }

            return null;
        }
    }
}