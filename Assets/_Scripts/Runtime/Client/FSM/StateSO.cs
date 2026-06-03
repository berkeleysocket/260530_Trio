using UnityEngine;

namespace Runtime.Clients.FSM
{
    [CreateAssetMenu(fileName = "State data", menuName = "KSY/SO/Agents/State data", order = 0)]
    public class StateSO : ScriptableObject
    {
        public string stateName;
        public string className;
        public int assetIndex;
        public AnimParamSO stateParam;
    }
}
