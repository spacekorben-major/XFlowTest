using System.Numerics;
using Components;
using Leopotam.EcsLite;

namespace Systems
{
    public class ProximitySystem : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();
            var filterProximityTriggers = world.Filter<ProximityTriggerComponent>()
                .Inc<ProximityTriggerActiveComponent>()
                .Inc<PositionComponent>()
                .End();

            var filterAlertingEntities = world.Filter<AlertingEntityTag>()
                .Inc<PositionComponent>()
                .End();

            var poolPosition = world.GetPool<PositionComponent>();
            var poolProximityTrigger = world.GetPool<ProximityTriggerComponent>();
            var poolProximityTriggerActive = world.GetPool<ProximityTriggerActiveComponent>();

            foreach (var triggerEntity in filterProximityTriggers)
            {
                var triggerMagnitude = poolProximityTrigger.Get(triggerEntity).DetectRadius;
                var triggerPosition = poolPosition.Get(triggerEntity).Position;
                ref var proximityTriggerActive = ref poolProximityTriggerActive.Get(triggerEntity);

                var active = false;
                foreach (var alertingEntity in filterAlertingEntities)
                {
                    var magnitude = Vector3.DistanceSquared(poolPosition.Get(alertingEntity).Position, triggerPosition);
                    if (magnitude < triggerMagnitude * triggerMagnitude)
                    {
                        active = true;
                        break;
                    }
                }

                proximityTriggerActive.Active = active;
            }
        }
    }
}