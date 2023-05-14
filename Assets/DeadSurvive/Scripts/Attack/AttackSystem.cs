using DeadSurvive.Common;
using DeadSurvive.Health;
using DeadSurvive.Unit;
using DeadSurvive.Unit.Enum;
using DeadSurvive.ZoneDetect;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.Attack
{
    public class AttackSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var filter = world.Filter<CombatComponent>().End();

            foreach (var entity in filter)
            {
                Attack(world, entity);
            }
        }

        private void Attack(EcsWorld ecsWorld, int entity)
        {
            var unitPool = ecsWorld.GetPool<UnitComponent>();
            var detectPool = ecsWorld.GetPool<DetectComponent>();
            var combatPool = ecsWorld.GetPool<CombatComponent>();
            var attackPool = ecsWorld.GetPool<AttackComponent>();
            var transformPool = ecsWorld.GetPool<UnityObject<Transform>>();
            var healthChangePool = ecsWorld.GetPool<HealthChangeComponent>();
            
            ref var combatComponent = ref combatPool.Get(entity);
            ref var unitComponent = ref unitPool.Get(entity);
            ref var transformComponent = ref transformPool.Get(entity);

            if (!combatComponent.EntityTarget.Unpack(ecsWorld, out var entityTarget))
            {
                ResetUnit(ecsWorld, entity);
                return;
            }
                
            ref var transformTargetComponent = ref transformPool.Get(entityTarget);

            ref var detectComponent = ref detectPool.Get(entity);
            ref var attackComponent = ref attackPool.Get(entity);

            var distance =  Vector2.Distance(transformComponent.Value.position, transformTargetComponent.Value.position);
            var canAttack = unitComponent.UnitState == UnitState.Attack &&
                            detectComponent.ContainsEntity(ecsWorld, entityTarget) &&
                            distance < attackComponent.AttackRange;

            if (!canAttack)
            {
                ResetUnit(ecsWorld, entity);
                return;
            }

            attackComponent.Delay -= Time.deltaTime;

            if (attackComponent.Delay > 0)
            {
                return;
            }
                
            ref var healthChangeComponent = ref healthChangePool.GetOrAdd(entityTarget);
            healthChangeComponent.Points += attackComponent.AttackDamage;
            attackComponent.RefreshDelay();
        }

        private void ResetUnit(EcsWorld ecsWorld, int entity)
        {
            var combatPool = ecsWorld.GetPool<CombatComponent>();
            ref var unitComponent = ref ecsWorld.GetPool<UnitComponent>().Get(entity);

            if (unitComponent.UnitState == UnitState.Attack)
            {
                unitComponent.UnitState = UnitState.Stay;
            }
            
            combatPool.Del(entity);
        }
        
    }
}
