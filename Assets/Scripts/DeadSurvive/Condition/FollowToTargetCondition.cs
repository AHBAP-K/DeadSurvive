using DeadSurvive.Attack;
using DeadSurvive.Common;
using DeadSurvive.Condition.Interfaces;
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
            if (!_entityUnit.Unpack(_ecsWorld, out var entityUnit) ||
                !_entityTarget.Unpack(_ecsWorld, out var entityTarget))
            {
                return false;
            }
            
            var transformPool = _ecsWorld.GetPool<UnityObject<Transform>>();
            var attackPool = _ecsWorld.GetPool<AttackComponent>();
            
            ref var entityTransform = ref transformPool.Get(entityUnit);
            ref var attackComponent = ref attackPool.Get(entityUnit);
            ref var targetTransform = ref transformPool.Get(entityTarget);

            var distance = Vector2.Distance(entityTransform.Value.position, targetTransform.Value.position);

            return attackComponent.AttackRange < distance && attackComponent.AttackDetectRange > distance;
        }
    }
}