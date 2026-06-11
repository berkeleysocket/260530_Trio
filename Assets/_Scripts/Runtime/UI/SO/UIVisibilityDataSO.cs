using UnityEngine;

namespace Runtime.UI
{
    [CreateAssetMenu(fileName = "UIVisibilityDataSO", menuName = "KSY/SO/UI/UIVisibilityDataSO")]
    public class UIVisibilityDataSO : ScriptableObject
    {
        [field: SerializeField] public float FadeInDuration { get; private set; }
        [field: SerializeField] public float FadeOutDuration { get; private set; }
        [field: SerializeField] public float DisplayTime { get; private set; }
    }
}
