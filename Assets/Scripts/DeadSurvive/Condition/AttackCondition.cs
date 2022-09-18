using DeadSurvive.Condition.Interfaces;
using DeadSurvive.Unit;
using DeadSurvive.Unit.Enum;
using DeadSurvive.ZoneDetect;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.Attack.Data
{
    public class AttackCondition : ICondition
    {
        private readonly EcsWorld _ecsWorld;
        private readonly EcsPool<UnitComponent> _unitPool;
        
        private readonly EcsPackedEntity _entityUnit;
        private readonly EcsPackedEntity _entityTarget;
        
        public AttackCondition(EcsWorld ecsWorld, int entityUnit, int entityTarget)
        {
            _ecsWorld = ecsWorld;
            _entityUnit = ecsWorld.PackEntity(entityUnit);
            _entityTarget = ecsWorld.PackEntity(entityTarget);
            _unitPool = ecsWorld.GetPool<UnitComponent>();
        }
        
        public bool Check()
        {
            if (!_entityUnit.Unpack(_ecsWorld, out var entityUnit) || !_entityTarget.Unpack(_ecsWorld, out var entityTarget))
            {
                return false;
            }

            ref var unitComponent = ref _unitPool.Get(entityUnit);
            ref var detectComponent = ref _ecsWorld.GetPool<DetectComponent>().Get(entityUnit);
            ref var attackComponent = ref _ecsWorld.GetPool<AttackComponent>().Get(entityUnit);
            ref var unitTarget = ref _ecsWorld.GetPool<UnitComponent>().Get(entityTarget);

            var distance =  Vector2.Distance(unitComponent.UnitTransform.position, unitTarget.UnitTransform.position);

            return unitComponent.UnitState == UnitState.Attack &&
                   detectComponent.ContainsEntity(entityTarget) &&
                   distance < attackComponent.AttackRange;
        }

    }
}