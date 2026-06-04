using UnityEngine;

namespace Runtime.Shared.Core
{
    public class NetworkManager : MonoBehaviour
    {
        public static NetworkManager Instance => _instance;
        private static NetworkManager _instance;

        public NetworkRole Role { get; private set; }

        public void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            Role = NetworkRole.None;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                //StartVisitor();
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                //StartHost();
            }
        }
    }
}