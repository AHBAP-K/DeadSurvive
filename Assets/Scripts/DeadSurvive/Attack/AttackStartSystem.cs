using DeadSurvive.Condition;
using DeadSurvive.Moving;
using DeadSurvive.Moving.Data;
using DeadSurvive.Unit;
using DeadSurvive.Unit.Enum;
using DeadSurvive.ZoneDetect;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.Attack
{
    public class AttackStartSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var unitPool = world.GetPool<UnitComponent>();
            var detectPool = world.GetPool<DetectComponent>();

            var filter = world.Filter<DetectComponent>().Inc<UnitComponent>().End();

            foreach (var entity in filter)
            {
                ref var unitComponent = ref unitPool.Get(entity);
                ref var detectComponent = ref detectPool.Get(entity);

                if (unitComponent.UnitState is UnitState.Move or UnitState.Attack or UnitState.Dead)
                {
                    continue;
                }

                foreach (var detectedEntity in detectComponent.DetectedEntities)
                {
                    var detectedUnitComponent = unitPool.Get(detectedEntity.Entity);

                    if (unitComponent.UnitType != detectedUnitComponent.UnitType && detectedEntity.Distance < 3f)
                    {
                        AttackUnit(world, entity, detectedEntity.Entity);
                    }
                }
            }
        }

        private void AttackUnit(EcsWorld ecsWorld, int entityUnit, int entityTarget)
        {
            Debug.Log($"[{nameof(AttackSystem)}] {nameof(AttackUnit)} Entity: {entityUnit}, Target: {entityTarget}");
            
            var positionPool = ecsWorld.GetPool<MoveDestinationComponent>();
            var unitPool = ecsWorld.GetPool<UnitComponent>();
            
            if (positionPool.Has(entityUnit))
            {
                positionPool.Del(entityUnit);
            }

            ref var targetPositionComponent = ref positionPool.Add(entityUnit);
            ref var targetUnit = ref unitPool.Get(entityTarget);
            
            var transformPositionHolder = new TransformPositionHolder(targetUnit.UnitTransform);
            var followCondition = new FollowToTargetCondition(ecsWorld, entityUnit, entityTarget);
            
            targetPositionComponent.Configure(transformPositionHolder, followCondition);
            targetPositionComponent.ReachedTarget += () => { ReachedTarget(ecsWorld, entityUnit, entityTarget);};
        }

        private void ReachedTarget(EcsWorld ecsWorld, int entityUnit, int entityTarget)
        {
            var combatPool = ecsWorld.GetPool<CombatComponent>();
            var unitPool = ecsWorld.GetPool<UnitComponent>();
            ref var combatComponent = ref combatPool.Add(entityUnit);
            ref var unitComponent = ref unitPool.Get(entityUnit);
            var entityTargetPacked = ecsWorld.PackEntity(entityTarget);

            unitComponent.UnitState = UnitState.Attack;
            combatComponent.Setup(entityTargetPacked);
        }
    }
}