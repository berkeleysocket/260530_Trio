using Runtime.Shared.Packet;
using UnityEngine;

public abstract class NetworkObject : MonoBehaviour
{
    protected abstract void Sync(IPacket packet);
}
