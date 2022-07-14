using Components;
using Core.UnityDependent;
using Leopotam.EcsLite;
using UnityEngine;
using Views;
using Zenject;

namespace Systems.UnityDependentSystems
{
    /// <summary>
    ///     This system reads player input. It is Unity dependent and won't be used by the server
    /// </summary>
    public class PlayerInputSystem : IInitializable, ITickable
    {
        private readonly MainCameraView _mainCameraView;

        private Plane _mainPlane;

        private readonly EcsWorld _world;

        public PlayerInputSystem(SceneData sceneData, EcsWorld world)
        {
            _mainCameraView = sceneData.CameraView;

            _world = world;
        }

        public void Initialize()
        {
            _mainPlane = new Plane(new Vector3(0, 1, 0), 0);
        }

        public void Tick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = _mainCameraView.MainCamera.ScreenPointToRay(Input.mousePosition);
                _mainPlane.Raycast(ray, out var dist);

                var point = ray.GetPoint(dist);

                var filter = _world.Filter<TargetPositionComponent>().Inc<MouseControlledTag>().End();
                var poolTargetPosition = _world.GetPool<TargetPositionComponent>();
                var poolTargetPositionReached = _world.GetPool<TargetPositionReachedTag>();

                var poolPosition = _world.GetPool<PositionComponent>();
                var poolTargetRotation = _world.GetPool<TargetRotationComponent>();

                foreach (var entity in filter)
                {
                    var targetPosition = new System.Numerics.Vector3 {X = point.x, Y = point.y, Z = point.z};
                    poolTargetPosition.Get(entity).Position = targetPosition;

                    if (poolTargetPositionReached.Has(entity)) poolTargetPositionReached.Del(entity);

                    if (poolTargetRotation.Has(entity) && poolPosition.Has(entity))
                    {
                        var directionVector = targetPosition - poolPosition.Get(entity).Position;
                        poolTargetRotation.Get(entity).Rotation = System.Numerics.Vector3.Normalize(directionVector);
                    }
                }
            }
        }
    }
}