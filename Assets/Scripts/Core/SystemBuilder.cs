using System;
using Leopotam.EcsLite;
using Systems;
using Zenject;

namespace Core
{
    public class SystemBuilder : IInitializable, ITickable, IDisposable
    {
        private EcsSystems _systems;
        private EcsWorld _world;

        public SystemBuilder(EcsWorld world)
        {
            _world = world;
        }

        public void Dispose()
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems = null;
            }

            if (_world != null)
            {
                _world.Destroy();
                _world = null;
            }
        }

        public void Initialize()
        {
            _systems = new EcsSystems(_world, new GameTimer())
                .Add(new TimeMeasurementSystem())
                .Add(new MovementSystem())
                .Add(new RotationSystem())
                .Add(new DoorActivationSystem())
                .Add(new DoorControlSystem())
                .Add(new ProximitySystem());
            _systems.Init();
        }

        public void Tick()
        {
            _systems?.Run();
        }
    }

    public class GameTimer
    {
        public float DeltaTime;
    }
}