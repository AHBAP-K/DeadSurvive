using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DeadSurvive.Attack.Data;
using DeadSurvive.Condition;
using DeadSurvive.Health;
using DeadSurvive.Moving;
using DeadSurvive.Moving.Data;
using DeadSurvive.Unit;
using DeadSurvive.Unit.Enum;
using DeadSurvive.ZoneDetect;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.Attack
{
    public class AttackSystem : IEcsRunSystem
    {
        private readonly Dictionary<int, CancellationTokenSource> _attackCancellationTokens = new Dictionary<int, CancellationTokenSource>();

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
                
                if (unitComponent.UnitState is UnitState.Move)
                {
                    DisposeAttack(entity);
                }

                if (unitComponent.UnitState is UnitState.Move or UnitState.Attack or UnitState.Dead)
                {
                    continue;
                }

                for (int i = 0; i < detectComponent.DetectedEntities.Count; i++)
                {
                    var detectedEntity = detectComponent.DetectedEntities[i];
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

            DisposeAttack(entityUnit);

            var positionPool = ecsWorld.GetPool<MoveDestinationComponent>();
            var unitPool = ecsWorld.GetPool<UnitComponent>();
            
            if (positionPool.Has(entityUnit))
            {
                positionPool.Del(entityUnit);
            }

            ref var targetPositionComponent = ref positionPool.Add(entityUnit);
            var targetUnit = unitPool.Get(entityTarget);
            var transformPositionHolder = new TransformPositionHolder(targetUnit.UnitTransform);
            var followCondition = new FollowToTargetCondition(ecsWorld, entityUnit, entityTarget);
            
            targetPositionComponent.Configure(transformPositionHolder, followCondition);

            targetPositionComponent.ReachedTarget += () =>
            {
                var cancellationToken = new CancellationTokenSource();
                _attackCancellationTokens.Add(entityUnit, cancellationToken);
                AttackTarget(ecsWorld, entityUnit, entityTarget, cancellationToken.Token).Forget();
            };
        }
        
        private async UniTask AttackTarget(EcsWorld ecsWorld, int entityUnit, int entityTarget, CancellationToken cancellationToken)
        {
            Debug.Log($"[{nameof(AttackSystem)}] {nameof(AttackTarget)} Entity: {entityUnit}, Target: {entityTarget}");
            
            ChangeUnitState(ecsWorld, entityUnit, UnitState.Attack);
            
            var attackComponent =  ecsWorld.GetPool<AttackComponent>().Get(entityUnit);
            var attackCondition = new AttackCondition(ecsWorld, entityUnit, entityTarget);
            
            while (attackCondition.Check())
            {
                Attack(ecsWorld, entityUnit, entityTarget);
                
                await UniTask.Delay(TimeSpan.FromSeconds(attackComponent.AttackDelay), cancellationToken: cancellationToken).SuppressCancellationThrow();
                
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
            }
            
            ChangeUnitState(ecsWorld, entityUnit, UnitState.Stay);
        }

        private void Attack(EcsWorld ecsWorld, int entityUnit, int entityTarget)
        {
            ref var attackComponent = ref ecsWorld.GetPool<AttackComponent>().Get(entityUnit);
            ref var healthChangeComponent = ref ecsWorld.GetPool<HealthChangeComponent>().Add(entityTarget);

            healthChangeComponent.Points = attackComponent.AttackDamage;
        }
        
        private void ChangeUnitState(EcsWorld ecsWorld, int entity, UnitState unitState)
        {
            ref var unitComponent = ref ecsWorld.GetPool<UnitComponent>().Get(entity);
            unitComponent.UnitState = unitState;
        }
        
        private void DisposeAttack(int entity)
        {
            if (!_attackCancellationTokens.ContainsKey(entity) || _attackCancellationTokens[entity] == null)
            {
                return;
            }
            
            _attackCancellationTokens[entity].Cancel();
            _attackCancellationTokens[entity].Dispose();
            _attackCancellationTokens.Remove(entity);
        }
    }
}
