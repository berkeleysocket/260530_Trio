using Runtime.Shared.Core;
using Runtime.InputSystem;
using UnityEngine;

namespace Runtime.Core
{
    public class GameBootstrap : MonoBehaviour
    {
        private void Awake()
        {
            NetworkManager.Instance.Initialize();
            InputManager.Instance.Initialize();
        }
    }
}
