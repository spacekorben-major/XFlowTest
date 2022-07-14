using System.Diagnostics;
using Core;
using Leopotam.EcsLite;

namespace Systems
{
    public class TimeMeasurementSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
    {
        private Stopwatch _stopwatch;

        public void Destroy(EcsSystems systems)
        {
            _stopwatch.Stop();
        }

        public void Init(EcsSystems systems)
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        public void Run(EcsSystems systems)
        {
            _stopwatch.Stop();
            systems.GetShared<GameTimer>().DeltaTime = _stopwatch.ElapsedMilliseconds / 1000f; // calculate seconds
            _stopwatch.Restart();
        }
    }
}