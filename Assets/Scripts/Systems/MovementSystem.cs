using System.Numerics;
using Components;
using Core;
using Leopotam.EcsLite;

namespace Systems
{
    public class MovementSystem : IEcsRunSystem
    {
        private const float KDistanceDelta = 0.01f;

        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();

            var filter = world.Filter<TargetPositionComponent>()
                .Inc<PositionComponent>()
                .Inc<VelocityComponent>()
                .Exc<TargetPositionReachedTag>()
                .Exc<MovementDisabledTag>()
                .End();

            var poolMovementInput = world.GetPool<TargetPositionComponent>();
            var poolPosition = world.GetPool<PositionComponent>();
            var poolVelocity = world.GetPool<VelocityComponent>();
            var poolTargetPositionReached = world.GetPool<TargetPositionReachedTag>();

            var deltaTime = systems.GetShared<GameTimer>().DeltaTime;

            foreach (var entity in filter)
            {
                var targetPosition = poolMovementInput.Get(entity).Position;
                ref var currentPosition = ref poolPosition.Get(entity).Position;
                if (Vector3.DistanceSquared(targetPosition, currentPosition) < KDistanceDelta)
                {
                    poolTargetPositionReached.Add(entity);
                    continue;
                }

                currentPosition += Vector3.Normalize(targetPosition - currentPosition) *
                                   poolVelocity.Get(entity).Velocity * deltaTime;
            }
        }
    }
}