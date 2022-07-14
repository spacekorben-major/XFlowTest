using System.Numerics;
using Components;
using Leopotam.EcsLite;
using Zenject;

namespace Core
{
    public class WorldBuilderNoUnity : IFactory<EcsWorld>
    {
        private readonly StartingPositionData mapData;

        public WorldBuilderNoUnity(StartingPositionData mapData)
        {
            this.mapData = mapData;
        }

        public EcsWorld Create()
        {
            var world = new EcsWorld();
            CreatePlayer(world);

            CreateButtonAndWall(world, mapData.Button1, mapData.Door1);
            CreateButtonAndWall(world, mapData.Button2, mapData.Door2);

            return world;
        }

        private void CreatePlayer(EcsWorld world)
        {
            var playerEntity = world.NewEntity();

            var poolPosition = world.GetPool<PositionComponent>();
            var poolMovementInput = world.GetPool<TargetPositionComponent>();
            var poolMouseControl = world.GetPool<MouseControlledTag>();
            var poolVelocity = world.GetPool<VelocityComponent>();
            var poolAlertingEntity = world.GetPool<AlertingEntityTag>();
            var poolRotation = world.GetPool<RotationComponent>();
            var poolTargetRotation = world.GetPool<TargetRotationComponent>();

            poolPosition.Add(playerEntity).Position = mapData.Player;
            poolMovementInput.Add(playerEntity);
            poolMouseControl.Add(playerEntity);
            poolVelocity.Add(playerEntity).Velocity = 3f;
            poolAlertingEntity.Add(playerEntity);
            poolRotation.Add(playerEntity);
            poolTargetRotation.Add(playerEntity);
        }

        private void CreateButtonAndWall(EcsWorld world, Vector3 buttonPosition, Vector3 doorPosition)
        {
            var buttonEntity = world.NewEntity();

            var poolPosition = world.GetPool<PositionComponent>();
            var poolWallLink = world.GetPool<DoorLinkComponent>();
            var poolProximityTrigger = world.GetPool<ProximityTriggerComponent>();
            var poolProximityTriggerActive = world.GetPool<ProximityTriggerActiveComponent>();

            poolPosition.Add(buttonEntity).Position = buttonPosition;

            poolProximityTrigger.Add(buttonEntity).DetectRadius = 0.8f;
            poolProximityTriggerActive.Add(buttonEntity);

            var doorEntity = world.NewEntity();

            poolWallLink.Add(buttonEntity).LinkedEntity = world.PackEntity(doorEntity);

            var poolVelocity = world.GetPool<VelocityComponent>();
            var poolDoor = world.GetPool<DoorComponent>();
            var poolMovement = world.GetPool<TargetPositionComponent>();
            var poolMovementDisabled = world.GetPool<MovementDisabledTag>();

            poolPosition.Add(doorEntity).Position = doorPosition;

            poolMovement.Add(doorEntity).Position = doorPosition + new Vector3(0, -4, 0);
            poolVelocity.Add(doorEntity).Velocity = 0.7f;
            poolMovementDisabled.Add(doorEntity);
            poolDoor.Add(doorEntity);
        }
    }

    public class StartingPositionData
    {
        public Vector3 Button1;

        public Vector3 Button2;

        public Vector3 Door1;

        public Vector3 Door2;
        public Vector3 Player;
    }
}