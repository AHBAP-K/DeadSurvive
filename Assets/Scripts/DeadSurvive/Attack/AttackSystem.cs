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

            var unitPool = world.GetPool<UnitComponent>();
            var detectPool = world.GetPool<DetectComponent>();
            var combatPool = world.GetPool<CombatComponent>();
            var attackPool = world.GetPool<AttackComponent>();

            var filter = world.Filter<CombatComponent>().End();

            foreach (var entity in filter)
            {
                ref var combatComponent = ref combatPool.Get(entity);
                ref var unitComponent = ref unitPool.Get(entity);

                if (!combatComponent.EntityTarget.Unpack(world, out var entityTarget))
                {
                    ResetUnit(world, entity);
                    continue;
                }
                
                ref var unitTargetComponent = ref unitPool.Get(entityTarget);
                ref var detectComponent = ref detectPool.Get(entity);
                ref var attackComponent = ref attackPool.Get(entity);

                var distance =  Vector2.Distance(unitComponent.UnitTransform.position, unitTargetComponent.UnitTransform.position);
                var canAttack = unitComponent.UnitState == UnitState.Attack &&
                                detectComponent.ContainsEntity(entityTarget) &&
                                distance < attackComponent.AttackRange;

                if (!canAttack)
                {
                    ResetUnit(world, entity);
                    continue;
                }

                attackComponent.Delay -= Time.deltaTime;

                if (attackComponent.Delay < 0)
                {
                    ref var healthChangeComponent = ref world.GetPool<HealthChangeComponent>().Add(entityTarget);
                    healthChangeComponent.Points = attackComponent.AttackDamage;
                    attackComponent.RefreshDelay();
                }
            }
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
