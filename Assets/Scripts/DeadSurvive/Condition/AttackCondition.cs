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
        
        private readonly int _entityUnit;
        private readonly int _entityTarget;
        
        public AttackCondition(EcsWorld ecsWorld, int entityUnit, int entityTarget)
        {
            _ecsWorld = ecsWorld;
            _entityUnit = entityUnit;
            _entityTarget = entityTarget;
        }
        
        public bool Check()
        {
            ref var detectComponent = ref _ecsWorld.GetPool<DetectComponent>().Get(_entityUnit);
            ref var unitComponent = ref _ecsWorld.GetPool<UnitComponent>().Get(_entityUnit);
            ref var attackComponent = ref _ecsWorld.GetPool<AttackComponent>().Get(_entityUnit);
            ref var unitTarget = ref _ecsWorld.GetPool<UnitComponent>().Get(_entityTarget);

            var distance =  Vector2.Distance(unitComponent.UnitTransform.position, unitTarget.UnitTransform.position);

            return unitComponent.UnitState == UnitState.Attack &&
                   detectComponent.ContainsEntity(_entityTarget) &&
                   distance < attackComponent.AttackRange;
        }

    }
}