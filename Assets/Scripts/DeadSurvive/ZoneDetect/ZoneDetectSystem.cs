using DeadSurvive.Unit;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.ZoneDetect
{
    public class ZoneDetectSystem : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();
            var detectPool = world.GetPool<DetectComponent>();
            var unitPool = world.GetPool<UnitComponent>();
            var filter = world.Filter<DetectComponent>().Inc<UnitComponent>().End();

            foreach (var entity in filter)
            {
                ref var detectUnitComponent = ref detectPool.Get(entity);
                ref var unitComponent = ref unitPool.Get(entity);
                
                foreach (var entityDetect in filter)
                {
                    if (entity == entityDetect)
                    {
                        continue;
                    }
                    
                    ref var targetUnitComponent = ref unitPool.Get(entityDetect);
                    var entityIsContains = detectUnitComponent.DetectedEntities.Contains(entityDetect);
                    var distance = Vector2.Distance(unitComponent.UnitTransform.position, targetUnitComponent.UnitTransform.position);

                    if (detectUnitComponent.DetectDistance > distance && !entityIsContains)
                    {
                        detectUnitComponent.DetectedEntities.Add(entityDetect);
                    }
                    else if(detectUnitComponent.DetectDistance < distance && entityIsContains)
                    {
                        detectUnitComponent.DetectedEntities.Remove(entityDetect);
                    }
                }
            }
        }
    }
}