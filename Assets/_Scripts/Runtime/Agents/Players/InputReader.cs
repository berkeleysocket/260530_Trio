using GameRoomPacket;
using UnityEngine;

namespace Runtime.Agents
{
    public class InputReader : MonoBehaviour
    {
        [field : SerializeField] public InputSO inputData;

        private void Start()
        {
            inputData.OnMoved += (velocity) =>
            {
                C2S_MoveInputPacket packet = new C2S_MoveInputPacket();
                //C2S_MoveInputPacket.StartC2S_MoveInputPacket();
                //NetworkManager.Instance.Send(packet);
            };
        }
    }
}