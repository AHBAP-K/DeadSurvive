using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.ZoneDetect
{
    public class ZoneDetectSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var detectPool = world.GetPool<DetectComponent>();
            var filter = world.Filter<DetectComponent>().End();

            foreach (var entity in filter)
            {
                ref var detectComponent = ref detectPool.Get(entity);
                
                foreach (var detectedEntity in filter)
                {
                    if (entity == detectedEntity)
                    {
                        continue;
                    }
                    
                    ref var detected = ref detectPool.Get(detectedEntity);
                    var entityIsContains = detectComponent.ContainsEntity(detectedEntity);
                    var distance = Vector2.Distance(detectComponent.ObjectTransform.position, detected.ObjectTransform.position);

                    if (!detectPool.Has(detectedEntity) && entityIsContains)
                    {
                        detectComponent.RemoveDetectedEntity(detectedEntity);
                        continue;
                    }

                    if (!entityIsContains)
                    {
                        detectComponent.AddDetectedEntity(detectedEntity, distance);
                    }
                    else
                    {
                        detectComponent.UpdateDistanceDetectedEntity(detectedEntity, distance);
                    }
                }
            }
        }
    }
}