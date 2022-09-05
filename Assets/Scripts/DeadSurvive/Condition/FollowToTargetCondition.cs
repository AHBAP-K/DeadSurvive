using DeadSurvive.Attack;
using DeadSurvive.Condition.Interfaces;
using DeadSurvive.Unit;
using DeadSurvive.ZoneDetect;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.Condition
{
    public class FollowToTargetCondition : ICondition
    {
        private readonly EcsWorld _ecsWorld;

        private readonly int _entity;
        private readonly int _target;

        public FollowToTargetCondition(EcsWorld ecsWorld, int entity, int target)
        {
            _ecsWorld = ecsWorld;
            _entity = entity;
            _target = target;
        }
        
        public bool Check()
        {
            var unitPool = _ecsWorld.GetPool<UnitComponent>();
            var detectPool = _ecsWorld.GetPool<DetectComponent>();
            var attackPool = _ecsWorld.GetPool<AttackComponent>();
            
            ref var unitComponent = ref unitPool.Get(_entity);
            ref var attackComponent = ref attackPool.Get(_entity);
            ref var unitDetectComponent = ref detectPool.Get(_entity);
            ref var targetUnitComponent = ref unitPool.Get(_target);

            var distance = Vector2.Distance(unitComponent.UnitTransform.position, targetUnitComponent.UnitTransform.position);

            return attackComponent.AttackRange < distance && 3f > distance;
        }
    }
}