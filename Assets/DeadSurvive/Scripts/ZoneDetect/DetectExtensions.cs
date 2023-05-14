using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.ZoneDetect
{
    public static class DetectExtensions
    {
        public static List<int> TryGetDetectEntities(this ref DetectComponent detectComponent, EcsWorld ecsWorld, float distance)
        {
            var detectedEntities = new List<int>();
            var removableIndexItem = -1;

            for (var i = 0; i < detectComponent.DetectedEntities.Count; i++)
            {
                var currentDetectedEntity = detectComponent.DetectedEntities[i];
                
                if (!currentDetectedEntity.PackedEntity.Unpack(ecsWorld, out var entity))
                {
                    removableIndexItem = i;
                    continue;
                }
                
                if (currentDetectedEntity.Distance < distance)
                {
                    detectedEntities.Add(entity);
                }
            }

            if (removableIndexItem > -1)
            {
                detectComponent.DetectedEntities.RemoveAt(removableIndexItem);
            }

            return detectedEntities;
        }
        
        public static void AddDetectComponent(this EcsWorld world, Transform transform, int entity)
        {
            var detectPool = world.GetPool<DetectComponent>();
            ref var detectComponent = ref detectPool.Add(entity);
            detectComponent.ObjectTransform = transform;
            detectComponent.DetectedEntities = new List<DetectedEntity>(5);
        }
        
        public static bool ContainsEntity(this ref DetectComponent detectComponent, EcsWorld ecsWorld, int targetEntity)
        {
            foreach (var detectedEntity in detectComponent.DetectedEntities)
            {
                if (detectedEntity.PackedEntity.Unpack(ecsWorld, out var entity) && entity == targetEntity)
                {
                    return true;
                }
            }

            return false;
        }
        
        public static void AddDetectedEntity(this ref DetectComponent detectComponent, EcsPackedEntity packedEntity, float distance)
        {
            detectComponent.DetectedEntities.Add(new DetectedEntity(packedEntity, distance));
        }

        public static void UpdateDistanceDetectedEntity(this ref DetectComponent detectComponent, EcsWorld ecsWorld, int targetEntity, float distance)
        {
            for (var i = 0; i < detectComponent.DetectedEntities.Count; i++)
            {
                if (!detectComponent.DetectedEntities[i].PackedEntity.Unpack(ecsWorld, out var entity) || entity != targetEntity)
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