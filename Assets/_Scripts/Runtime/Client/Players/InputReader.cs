using UnityEngine;

namespace Runtime.Clients.Players
{
    public class InputReader : MonoBehaviour
    {
        [field: SerializeField] public InputSO InputData { get; private set; }
    }
}