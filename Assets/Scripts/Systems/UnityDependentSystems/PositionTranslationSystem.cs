using Components;
using Components.UnityDependent;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Systems.UnityDependentSystems
{
    public class PositionTranslationSystem : ITickable
    {
        private readonly EcsWorld _world;

        public PositionTranslationSystem(EcsWorld world)
        {
            _world = world;
        }

        public void Tick()
        {
            var filter = _world.Filter<PositionComponent>().Inc<TransformComponent>().End();
            var poolPosition = _world.GetPool<PositionComponent>();
            var poolTransform = _world.GetPool<TransformComponent>();

            foreach (var entity in filter)
            {
                var position = poolPosition.Get(entity).Position;
                poolTransform.Get(entity).Transform.position = new Vector3(position.X, position.Y, position.Z);
            }
        }
    }
}