using DeadSurvive.Common;
using DeadSurvive.Health;
using DeadSurvive.Unit;
using DeadSurvive.Unit.Enum;
using DeadSurvive.ZoneDetect;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace DeadSurvive.Attack
{
    public class AttackSystem : IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;

        private readonly EcsPoolInject<UnitComponent> _unitPool = default;
        private readonly EcsPoolInject<DetectComponent> _detectPool = default;
        private readonly EcsPoolInject<CombatComponent> _combatPool = default;
        private readonly EcsPoolInject<AttackComponent> _attackPool = default;
        private readonly EcsPoolInject<UnityObject<Transform>> _transformPool = default;
        private readonly EcsPoolInject<HealthChangeComponent> _healthChangePool = default;

        private readonly EcsFilterInject<Inc<CombatComponent>> _filterCombat;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filterCombat.Value)
            {
                Attack(_world.Value, entity);
            }
        }

        private void Attack(EcsWorld ecsWorld, int entity)
        {
            ref var combatComponent = ref _combatPool.Value.Get(entity);
            ref var unitComponent = ref _unitPool.Value.Get(entity);
            ref var transformComponent = ref _transformPool.Value.Get(entity);

            if (!combatComponent.EntityTarget.Unpack(ecsWorld, out var entityTarget))
            {
                ResetUnit(ecsWorld, entity);
                return;
            }
                
            ref var transformTargetComponent = ref _transformPool.Value.Get(entityTarget);

            ref var detectComponent = ref _detectPool.Value.Get(entity);
            ref var attackComponent = ref _attackPool.Value.Get(entity);

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
                
            ref var healthChangeComponent = ref _healthChangePool.Value.GetOrAdd(entityTarget);
            healthChangeComponent.Points += attackComponent.AttackDamage;
            attackComponent.RefreshDelay();
        }

        private void ResetUnit(EcsWorld ecsWorld, int entity)
        {
            ref var unitComponent = ref ecsWorld.GetPool<UnitComponent>().Get(entity);

            if (unitComponent.UnitState == UnitState.Attack)
            {
                unitComponent.UnitState = UnitState.Stay;
            }
            
            _combatPool.Value.Del(entity);
        }
    }
}
