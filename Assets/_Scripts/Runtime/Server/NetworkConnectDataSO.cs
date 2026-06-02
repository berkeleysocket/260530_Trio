using UnityEngine;

[CreateAssetMenu(fileName = "NetworkConnectDataSO", menuName = "KSY/SO/NetworkConnectDataSO")]
public class NetworkConnectDataSO : ScriptableObject
{
    [field : SerializeField] public string IpAddress { get; private set; }
    [field: SerializeField] public int Port { get; private set; }
}
