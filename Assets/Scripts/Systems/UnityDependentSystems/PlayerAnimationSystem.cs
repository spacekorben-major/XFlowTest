using Components;
using Components.UnityDependent;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Systems.UnityDependentSystems
{
    public class PlayerAnimationSystem : ITickable
    {
        private readonly int _walking = Animator.StringToHash("Walking");
        private readonly EcsWorld _world;

        public PlayerAnimationSystem(EcsWorld world)
        {
            _world = world;
        }

        public void Tick()
        {
            var poolAnimator = _world.GetPool<AnimatorComponent>();
            var poolTargetPositionReached = _world.GetPool<TargetPositionReachedTag>();

            var filter = _world.Filter<AnimatorComponent>().End();
            foreach (var entity in filter)
                poolAnimator.Get(entity).Animator.SetBool(_walking, !poolTargetPositionReached.Has(entity));
        }
    }
}