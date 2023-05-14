using DeadSurvive.Common;
using DeadSurvive.Condition;
using DeadSurvive.Moving;
using DeadSurvive.Moving.Data;
using DeadSurvive.Unit;
using DeadSurvive.Unit.Enum;
using DeadSurvive.ZoneDetect;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace DeadSurvive.Attack
{
    public class AttackStartSystem : IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsPoolInject<UnitComponent> _unitPool = default;
        private readonly EcsPoolInject<DetectComponent> _detectPool = default;
        private readonly EcsPoolInject<AttackComponent> _attackPool = default;
        private readonly EcsPoolInject<MoveDestinationComponent> _moveDestinationPool = default;
        private readonly EcsPoolInject<UnityObject<Transform>> _transformPool = default;
        private readonly EcsPoolInject<CombatComponent> _combatPool = default;
        
        private readonly EcsFilterInject<Inc<DetectComponent, UnitComponent>> _filter = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var unitComponent = ref _unitPool.Value.Get(entity);
                ref var detectComponent = ref _detectPool.Value.Get(entity);
                ref var attackComponent = ref _attackPool.Value.Get(entity);

                if (unitComponent.UnitState is UnitState.Move or UnitState.Attack or UnitState.Dead)
                {
                    continue;
                }

                var detectedEntities = detectComponent.TryGetDetectEntities(_world.Value, attackComponent.AttackDetectRange);

                foreach (var target in detectedEntities)
                {
                    var detectedUnitComponent = _unitPool.Value.Get(target);

                    if (unitComponent.UnitType != detectedUnitComponent.UnitType)
                    {
                        AttackUnit(entity, target);
                    }
                }
            }
        }

        private void AttackUnit(int entityUnit, int entityTarget)
        {
            Debug.Log($"[{nameof(AttackSystem)}] {nameof(AttackUnit)} Entity: {entityUnit}, Target: {entityTarget}");
            
            ref var targetPositionComponent = ref _moveDestinationPool.Value.GetOrAdd(entityUnit);
            ref var transformComponent = ref _transformPool.Value.Get(entityTarget);
            
            var transformPositionHolder = new TransformPositionHolder(transformComponent.Value);
            var followCondition = new FollowToTargetCondition(_world.Value, entityUnit, entityTarget);
            
            targetPositionComponent.Configure(transformPositionHolder, followCondition);
            targetPositionComponent.ReachedTarget += () => { ReachedTarget(entityUnit, entityTarget);};
        }

        private void ReachedTarget(int entityUnit, int entityTarget)
        {
            ref var combatComponent = ref _combatPool.Value.Add(entityUnit);
            ref var unitComponent = ref _unitPool.Value.Get(entityUnit);
            var entityTargetPacked = _world.Value.PackEntity(entityTarget);

            unitComponent.UnitState = UnitState.Attack;
            combatComponent.Setup(entityTargetPacked);
        }
    }
}