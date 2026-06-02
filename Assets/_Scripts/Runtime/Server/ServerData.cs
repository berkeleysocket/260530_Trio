using UnityEngine;

[CreateAssetMenu(fileName = "ServerData", menuName = "KSY/SO/ServerData")]
public class ServerData : ScriptableObject
{
    [field : SerializeField] public string IpAddress { get; private set; }
    [field: SerializeField] public int Port { get; private set; }
}
