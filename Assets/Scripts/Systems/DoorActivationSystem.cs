using System;
using Components;
using Leopotam.EcsLite;

namespace Systems
{
    public class DoorActivationSystem : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();
            var filterButtons = world.Filter<DoorLinkComponent>().Inc<ProximityTriggerActiveComponent>().End();

            var poolDoorLink = world.GetPool<DoorLinkComponent>();
            var poolProximityTriggerActive = world.GetPool<ProximityTriggerActiveComponent>();

            var poolDoor = world.GetPool<DoorComponent>();
            var poolDoorUnlocked = world.GetPool<DoorUnlockedTag>();

            foreach (var buttonEntity in filterButtons)
            {
                var doorEntityPacked = poolDoorLink.Get(buttonEntity).LinkedEntity;
                if (!doorEntityPacked.Unpack(world, out var doorEntity))
                    throw new Exception("Door entity was destroyed!");

                if (poolDoorUnlocked.Has(doorEntity)) continue;

                if (!poolDoor.Has(doorEntity)) continue;

                poolDoor.Get(doorEntity).Unlocking = poolProximityTriggerActive.Get(buttonEntity).Active;
            }
        }
    }
}