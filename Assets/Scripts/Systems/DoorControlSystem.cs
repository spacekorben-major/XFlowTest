using Components;
using Leopotam.EcsLite;

namespace Systems
{
    public class DoorControlSystem : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world.Filter<DoorComponent>()
                .Inc<TargetPositionComponent>()
                .Inc<PositionComponent>()
                .Exc<DoorUnlockedTag>().End();

            var poolDoor = world.GetPool<DoorComponent>();
            var poolUnlocked = world.GetPool<DoorUnlockedTag>();
            var poolMovementDisabled = world.GetPool<MovementDisabledTag>();
            var poolTargetPositionReached = world.GetPool<TargetPositionReachedTag>();

            foreach (var entity in filter)
            {
                if (poolTargetPositionReached.Has(entity))
                {
                    poolUnlocked.Add(entity);
                    continue;
                }

                if (poolDoor.Get(entity).Unlocking)
                {
                    if (poolMovementDisabled.Has(entity)) poolMovementDisabled.Del(entity);
                }
                else
                {
                    if (!poolMovementDisabled.Has(entity)) poolMovementDisabled.Add(entity);
                }
            }
        }
    }
}