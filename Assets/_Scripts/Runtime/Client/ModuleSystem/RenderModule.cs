using Runtime.Clients.Agents;
using UnityEngine;

namespace Runtime.Clients.ModuleSystem
{
    [RequireComponent(typeof(Animator))]
    public class RenderModule : MonoBehaviour, IModule
    {
        private Animator _animator;
        public void Initialize(ModuleOwner owner)
        {
            _animator = GetComponent<Animator>();
        }

        public void PlayClip(int clipHash, float normalizedTime, float crossFadeDuration, int layerIndex = 0)
        {
            _animator.CrossFadeInFixedTime(clipHash, crossFadeDuration, layerIndex, normalizedTime);
        }
    }
}

