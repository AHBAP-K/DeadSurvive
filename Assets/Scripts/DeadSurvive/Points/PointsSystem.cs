using DeadSurvive.Common;
using DeadSurvive.Health;
using DeadSurvive.Unit;
using DeadSurvive.Unit.Enum;
using Leopotam.EcsLite;

namespace DeadSurvive.Points
{
    public class PointsSystem : IEcsInitSystem, IEcsRunSystem
    {
        public void Init(IEcsSystems systems)
        {
            throw new System.NotImplementedException();
        }
        
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var healthPool = world.GetPool<HealthComponent>();
            var unitPool = world.GetPool<UnitComponent>();
            
            var filterUnit = world.Filter<HealthComponent>().Inc<UnitComponent>().End();
            
            foreach (var entity in filterUnit)
            {
                ref var healthComponent = ref healthPool.Get(entity);
                ref var unitComponent = ref unitPool.Get(entity);
                
                if (unitComponent.UnitType != UnitType.Enemy || healthComponent.CurrentHealth > 0f)
                {
                    continue;
                }
            }
        }
    }
}