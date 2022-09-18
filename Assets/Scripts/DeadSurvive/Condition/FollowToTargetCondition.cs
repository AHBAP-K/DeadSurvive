using DeadSurvive.Attack;
using DeadSurvive.Condition.Interfaces;
using DeadSurvive.Unit;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.Condition
{
    public class FollowToTargetCondition : ICondition
    {
        private readonly EcsWorld _ecsWorld;

        private readonly EcsPackedEntity _entityUnit;
        private readonly EcsPackedEntity _entityTarget;
        
        public FollowToTargetCondition(EcsWorld ecsWorld, int entity, int target)
        {
            _ecsWorld = ecsWorld;
            _entityUnit = ecsWorld.PackEntity(entity);
            _entityTarget = ecsWorld.PackEntity(target);
        }
        
        public bool Check()
        {
            if (!_entityUnit.Unpack(_ecsWorld, out var entityUnit) || !_entityTarget.Unpack(_ecsWorld, out var entityTarget))
            {
                return false;
            }
            
            var unitPool = _ecsWorld.GetPool<UnitComponent>();
            var attackPool = _ecsWorld.GetPool<AttackComponent>();
            
            ref var unitComponent = ref unitPool.Get(entityUnit);
            ref var attackComponent = ref attackPool.Get(entityUnit);
            ref var targetUnitComponent = ref unitPool.Get(entityTarget);

            var distance = Vector2.Distance(unitComponent.UnitTransform.position, targetUnitComponent.UnitTransform.position);

            return attackComponent.AttackRange < distance && 3f > distance;
        }
    }
}