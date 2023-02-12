using DeadSurvive.Common;
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
            var attackPool = world.GetPool<AttackComponent>();

            var filter = world.Filter<DetectComponent>().Inc<UnitComponent>().End();

            foreach (var entity in filter)
            {
                ref var unitComponent = ref unitPool.Get(entity);
                ref var detectComponent = ref detectPool.Get(entity);
                ref var attackComponent = ref attackPool.Get(entity);

                if (unitComponent.UnitState is UnitState.Move or UnitState.Attack or UnitState.Dead)
                {
                    continue;
                }

                var detectedEntities = detectComponent.TryGetDetectEntities(world, attackComponent.AttackDetectRange);

                foreach (var target in detectedEntities)
                {
                    var detectedUnitComponent = unitPool.Get(target);

                    if (unitComponent.UnitType != detectedUnitComponent.UnitType)
                    {
                        AttackUnit(world, entity, target);
                    }
                }
            }
        }

        private void AttackUnit(EcsWorld ecsWorld, int entityUnit, int entityTarget)
        {
            Debug.Log($"[{nameof(AttackSystem)}] {nameof(AttackUnit)} Entity: {entityUnit}, Target: {entityTarget}");
            
            var positionPool = ecsWorld.GetPool<MoveDestinationComponent>();
            var transformPool = ecsWorld.GetPool<UnityObject<Transform>>();

            ref var targetPositionComponent = ref positionPool.GetOrAdd(entityUnit);
            ref var transformComponent = ref transformPool.Get(entityTarget);
            
            var transformPositionHolder = new TransformPositionHolder(transformComponent.Value);
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