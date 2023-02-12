using DeadSurvive.Common;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.Health
{
    public static class HealthExtensions
    {
        public static void AddHealthComponent(this EcsWorld world, HealthData healthData, int entity, Transform parent)
        {
            var unitObject = Object.Instantiate(healthData.HealthBarPrefab, parent);
            var healthBarView = unitObject.GetComponent<HealthBarView>();
            
            var healthPool = world.GetPool<HealthComponent>();
            var healthViewPool = world.GetPool<UnityObject<HealthBarView>>();
            
            ref var healthComponent = ref healthPool.Add(entity);
            ref var healthBarViewComponent = ref healthViewPool.Add(entity);

            healthBarViewComponent.Value = healthBarView;

            unitObject.transform.localPosition = healthData.Position;
            
            healthComponent.CurrentHealth = healthData.MaxHealth;
            healthComponent.MaxHealth = healthData.MaxHealth;
        }
    }
}