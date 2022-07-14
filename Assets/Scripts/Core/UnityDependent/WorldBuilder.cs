using System;
using Components;
using Components.UnityDependentComponents;
using Leopotam.EcsLite;
using UnityEngine;
using Views;
using Zenject;
using Vector3 = System.Numerics.Vector3;

namespace Core.UnityDependent
{
    public class WorldBuilder : IFactory<EcsWorld>
    {
        private readonly SceneData _sceneData;

        public WorldBuilder(SceneData sceneData)
        {
            _sceneData = sceneData;
        }

        public EcsWorld Create()
        {
            var world = new EcsWorld();
            CreatePlayer(world);

            CreateButtonAndWall(world, _sceneData.Button1Transform, _sceneData.Door1Transform);
            CreateButtonAndWall(world, _sceneData.Button2Transform, _sceneData.Door2Transform);

            return world;
        }

        private void CreatePlayer(EcsWorld world)
        {
            var playerEntity = world.NewEntity();

            var poolTransform = world.GetPool<TransformComponent>();
            var poolAnimator = world.GetPool<AnimatorComponent>();
            var poolPosition = world.GetPool<PositionComponent>();
            var poolMovementInput = world.GetPool<TargetPositionComponent>();
            var poolMouseControl = world.GetPool<MouseControlledTag>();
            var poolVelocity = world.GetPool<VelocityComponent>();
            var poolAlertingEntity = world.GetPool<AlertingEntityTag>();
            var poolRotation = world.GetPool<RotationComponent>();
            var poolTargetRotation = world.GetPool<TargetRotationComponent>();

            poolTransform.Add(playerEntity).Transform = _sceneData.PlayerView.transform;
            poolAnimator.Add(playerEntity).Animator = _sceneData.PlayerView.Animator;
            poolPosition.Add(playerEntity);
            poolMovementInput.Add(playerEntity);
            poolMouseControl.Add(playerEntity);
            poolVelocity.Add(playerEntity).Velocity = 3f;
            poolAlertingEntity.Add(playerEntity);
            poolRotation.Add(playerEntity);
            poolTargetRotation.Add(playerEntity);
        }

        private void CreateButtonAndWall(EcsWorld world, Transform buttonTransform, Transform doorTransform)
        {
            var buttonEntity = world.NewEntity();

            var poolTransform = world.GetPool<TransformComponent>();
            var poolPosition = world.GetPool<PositionComponent>();
            var poolWallLink = world.GetPool<DoorLinkComponent>();
            var poolProximityTrigger = world.GetPool<ProximityTriggerComponent>();
            var poolProximityTriggerActive = world.GetPool<ProximityTriggerActiveComponent>();

            poolTransform.Add(buttonEntity).Transform = buttonTransform;
            var buttonPosition = buttonTransform.position;
            poolPosition.Add(buttonEntity).Position = new Vector3(buttonPosition.x, buttonPosition.y, buttonPosition.z);

            poolProximityTrigger.Add(buttonEntity).DetectRadius = 0.8f;
            poolProximityTriggerActive.Add(buttonEntity);

            var doorEntity = world.NewEntity();

            poolWallLink.Add(buttonEntity).LinkedEntity = world.PackEntity(doorEntity);

            var poolVelocity = world.GetPool<VelocityComponent>();
            var poolDoor = world.GetPool<DoorComponent>();
            var poolMovement = world.GetPool<TargetPositionComponent>();
            var poolMovementDisabled = world.GetPool<MovementDisabledTag>();

            poolTransform.Add(doorEntity).Transform = doorTransform;
            var doorPosition = doorTransform.position;
            poolPosition.Add(doorEntity).Position = new Vector3(doorPosition.x, doorPosition.y, doorPosition.z);

            poolMovement.Add(doorEntity).Position = new Vector3(doorPosition.x, doorPosition.y - 4f, doorPosition.z);
            poolVelocity.Add(doorEntity).Velocity = 0.7f;
            poolMovementDisabled.Add(doorEntity);
            poolDoor.Add(doorEntity);
        }
    }

    /// <summary>
    ///     This is a placeholder, obviously.
    ///     In future we will need to come up with to come up with a way to serialize
    ///     unity scene to a server-readable data type. After that we will be able to merge WorldBuilder
    ///     and WorldBuilderNoUnity into single file.
    /// </summary>
    [Serializable]
    public class SceneData
    {
        public MainCameraView CameraView;

        public PlayerView PlayerView;

        public Transform Button1Transform;

        public Transform Button2Transform;

        public Transform Door1Transform;

        public Transform Door2Transform;
    }
}