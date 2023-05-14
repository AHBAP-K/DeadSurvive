using DeadSurvive.Common;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.Health
{
    public class HealthSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var healthPool = world.GetPool<HealthComponent>();
            var healthViewPool = world.GetPool<UnityObject<HealthBarView>>();

            var healthChangePool = world.GetPool<HealthChangeComponent>();
            
            var filterUnit = world.Filter<HealthComponent>().Inc<HealthChangeComponent>().End();

            foreach (var entity in filterUnit)
            {
                ref var healthComponent = ref healthPool.Get(entity);
                ref var healthChangeComponent = ref healthChangePool.Get(entity);
                ref var healthViewComponent = ref healthViewPool.Get(entity);
                
                healthComponent.CurrentHealth = Mathf.Clamp(healthComponent.CurrentHealth + healthChangeComponent.Points, -0.1f, healthComponent.MaxHealth);
                
                healthChangePool.Del(entity);
                
                healthViewComponent.Value.MoveBar(healthComponent.CurrentHealth / healthComponent.MaxHealth);
            }
        }
    }
}