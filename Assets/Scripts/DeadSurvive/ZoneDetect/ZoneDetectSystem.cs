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
            var detectPool = world.GetPool<DetectUnitComponent>();
            var unitPool = world.GetPool<UnitComponent>();
            var filter = world.Filter<DetectUnitComponent>().Inc<UnitComponent>().End();

            foreach (var entity in filter)
            {
                var detectUnitComponent = detectPool.Get(entity);
                var unitComponent = unitPool.Get(entity);
                
                foreach (var entityDetect in filter)
                {
                    if (entity == entityDetect)
                    {
                        continue;
                    }
                    
                    var targetUnitComponent = unitPool.Get(entityDetect);
                    var entityIsContains = detectUnitComponent.DetectedUnitEntities.Contains(entityDetect);
                    var distance = Vector2.Distance(unitComponent.UnitTransform.position, targetUnitComponent.UnitTransform.position);

                    if (detectUnitComponent.DetectDistance > distance && !entityIsContains)
                    {
                        detectUnitComponent.DetectedUnitEntities.Add(entityDetect);
                    }
                    else if(detectUnitComponent.DetectDistance < distance && entityIsContains)
                    {
                        detectUnitComponent.DetectedUnitEntities.Remove(entityDetect);
                    }
                }
            }
        }
    }
}