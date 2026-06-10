using Runtime.Pattern;
using UnityEngine;

namespace Runtime.UI
{
    public abstract class UIEffect : MonoBehaviour, IInitializable
    {
        public virtual void Initialize() { }

        public abstract void ActiveEffect();
        public abstract void InactiveEffect();
    }
}