using DeadSurvive.Common;
using DeadSurvive.Common.Data;
using DeadSurvive.Health;
using DeadSurvive.Unit;
using DeadSurvive.UnitButton;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.Death
{
    public class DeathSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var healthPool = world.GetPool<HealthComponent>();
            var transformPool = world.GetPool<UnityObject<Transform>>();
            var filterUnit = world.Filter<HealthComponent>().Inc<UnitComponent>().End();

            foreach (var entity in filterUnit)
            {
                ref var healthComponent = ref healthPool.Get(entity);

                if (healthComponent.CurrentHealth > 0f)
                {
                    continue;
                }
                
                var pool = systems.GetShared<GameData>().Pool;
                ref var transformComponent = ref transformPool.Get(entity);

                RemoveButton(world, entity, pool);
                    
                var gameObject = transformComponent.Value.gameObject;
                    
                world.DelEntity(entity);
                pool.ReturnObject(gameObject);
            }
        }

        private void RemoveButton(EcsWorld ecsWorld, int entity, Pool.Pool pool)
        {
            var buttonPool = ecsWorld.GetPool<ButtonComponent>();

            if (!buttonPool.Has(entity))
            {
                return;
            }
            
            ref var buttonComponent = ref buttonPool.Get(entity);
            var buttonGameObject = buttonComponent.ButtonView.gameObject;
            pool.ReturnObject(buttonGameObject);
        }
        
    }
}