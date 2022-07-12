using System;
using Leopotam.EcsLite;
using Zenject;

namespace Core
{
    public class WorldBuilder : IInitializable, ITickable, IDisposable
    {
        EcsWorld _world;
        EcsSystems _systems;

        public WorldBuilder()
        {
        }

        public void Initialize() {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
                //.Add (new WeaponSystem ());
            _systems.Init();
        }
    
        public void Tick() {
            _systems?.Run();
        }

        public void Dispose() {
            if (_systems != null) {
                _systems.Destroy();
                _systems = null;
            }

            if (_world != null) {
                _world.Destroy();
                _world = null;
            }
        }
    }
}