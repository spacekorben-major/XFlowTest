using System.Numerics;
using Components;
using Leopotam.EcsLite;

namespace Systems
{
    public class RotationSystem : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world.Filter<RotationComponent>()
                .Inc<TargetRotationComponent>()
                .Exc<TargetPositionReachedTag>()
                .End();

            var poolRotation = world.GetPool<RotationComponent>();
            var poolTargetRotation = world.GetPool<TargetRotationComponent>();

            foreach (var entity in filter)
            {
                var targetRotation = poolTargetRotation.Get(entity).Rotation;
                if (targetRotation == Vector3.Zero) continue;

                poolRotation.Get(entity).Rotation = targetRotation;
            }
        }
    }
}