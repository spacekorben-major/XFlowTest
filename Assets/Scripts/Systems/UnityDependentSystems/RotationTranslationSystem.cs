using System.Numerics;
using Components;
using Components.UnityDependent;
using Leopotam.EcsLite;
using Zenject;
using Quaternion = UnityEngine.Quaternion;

namespace Systems.UnityDependentSystems
{
    public class RotationTranslationSystem : ITickable
    {
        private readonly EcsWorld _world;

        public RotationTranslationSystem(EcsWorld world)
        {
            _world = world;
        }

        public void Tick()
        {
            var filter = _world.Filter<RotationComponent>().Inc<TransformComponent>().End();
            var poolRotation = _world.GetPool<RotationComponent>();
            var poolTransform = _world.GetPool<TransformComponent>();

            foreach (var entity in filter)
            {
                var rotation = poolRotation.Get(entity).Rotation;
                if (rotation == Vector3.Zero) continue;

                poolTransform.Get(entity).Transform.rotation =
                    Quaternion.LookRotation(new UnityEngine.Vector3(rotation.X, rotation.Y, rotation.Z),
                        UnityEngine.Vector3.up);
            }
        }
    }
}