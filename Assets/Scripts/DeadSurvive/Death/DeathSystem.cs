using DeadSurvive.Common.Data;
using DeadSurvive.Health;
using DeadSurvive.Unit;
using DeadSurvive.UnitButton;
using Leopotam.EcsLite;

namespace DeadSurvive.Death
{
    public class DeathSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var healthPool = world.GetPool<HealthComponent>();
            var unitPool = world.GetPool<UnitComponent>();
            var filterUnit = world.Filter<HealthComponent>().Inc<UnitComponent>().End();

            foreach (var entity in filterUnit)
            {
                ref var healthComponent = ref healthPool.Get(entity);

                if (healthComponent.CurrentHealth < 0f)
                {
                    var pool = systems.GetShared<GameData>().Pool;
                    ref var unitComponent = ref unitPool.Get(entity);

                    RemoveButton(world, entity, pool);
                    
                    var gameObject = unitComponent.UnitTransform.gameObject;
                    
                    world.DelEntity(entity);
                    pool.ReturnObject(gameObject);
                }
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