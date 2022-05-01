using DeadSurvive.Unit;
using DeadSurvive.Unit.Enum;
using DeadSurvive.ZoneDetect;
using Leopotam.EcsLite;

namespace DeadSurvive.Attack
{
    public class AttackSystem : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();
            
            var attackPool = world.GetPool<AttackComponent>();
            var unitPool = world.GetPool<UnitComponent>();
            var detectPool = world.GetPool<DetectUnitComponent>();
            
            var filter = world.Filter<AttackComponent>().Inc<DetectUnitComponent>().Inc<UnitComponent>().End();

            foreach (var entity in filter)
            {
                ref var unitComponent = ref unitPool.Get(entity);
                ref var detectComponent = ref detectPool.Get(entity);
                ref var attackComponent = ref attackPool.Get(entity);
                
                if (unitComponent.UnitState == UnitState.Move || unitComponent.UnitState == UnitState.Attack) 
                {
                    continue;
                }

                for (int i = 0; i < detectComponent.DetectedUnitEntities.Count; i++)
                {
                    var detectedEntity = detectComponent.DetectedUnitEntities[i];
                    var detectedUnitComponent = unitPool.Get(detectedEntity);

                    if (unitComponent.UnitType == detectedUnitComponent.UnitType)
                    {
                        continue;
                    }
                    
                    attackComponent.AttackUnit(world , ref unitComponent,  ref detectedUnitComponent);
                }
            }
        }
        
        
    }
}