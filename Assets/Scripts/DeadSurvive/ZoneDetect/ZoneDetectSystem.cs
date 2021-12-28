using Leopotam.EcsLite;

namespace DeadSurvive.ZoneDetect
{
    public class ZoneDetectSystem : IEcsInitSystem, IEcsRunSystem
    {
        public void Init(EcsSystems systems)
        {
            throw new System.NotImplementedException();
        }

        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();
        }
    }
}