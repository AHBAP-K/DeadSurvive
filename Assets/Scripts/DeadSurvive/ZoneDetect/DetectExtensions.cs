using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.ZoneDetect
{
    public static class DetectExtensions
    {
        public static void AddDetectComponent(this EcsWorld world, Transform transform, int entity)
        {
            var detectPool = world.GetPool<DetectComponent>();
            ref var detectComponent = ref detectPool.Add(entity);
            detectComponent.ObjectTransform = transform;
            detectComponent.DetectedEntities = new List<DetectedEntity>(5);
        }
        
        public static bool ContainsEntity(this ref DetectComponent detectComponent, int entity)
        {
            return detectComponent.DetectedEntities.Any(detectedEntity => detectedEntity.Entity == entity);
        }
        
        public static void AddDetectedEntity(this ref DetectComponent detectComponent, int entity, float distance)
        {
            detectComponent.DetectedEntities.Add(new DetectedEntity(entity, distance));
        }
        
        public static void RemoveDetectedEntity(this ref DetectComponent detectComponent, int entity)
        {
            for (var index = 0; index < detectComponent.DetectedEntities.Count; index++)
            {
                if (detectComponent.DetectedEntities[index].Entity == entity)
                {
                    detectComponent.DetectedEntities.Remove(detectComponent.DetectedEntities[index]);
                    return;
                }
            }
        }
        
        public static void UpdateDistanceDetectedEntity(this ref DetectComponent detectComponent, int entity, float distance)
        {
            for (var i = 0; i < detectComponent.DetectedEntities.Count; i++)
            {
                if (detectComponent.DetectedEntities[i].Entity != entity)
                {
                    continue;
                }

                var detectedEntity = detectComponent.DetectedEntities[i];
                detectedEntity.Distance = distance;
                detectComponent.DetectedEntities[i] = detectedEntity;
                
                return;
            }
        }
    }
}