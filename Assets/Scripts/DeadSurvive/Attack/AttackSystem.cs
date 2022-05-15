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

        public void Run(EcsSystems systems)
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
                    var detectedUnitComponent = unitPool.Get(detectedEntity);

                    if (unitComponent.UnitType == detectedUnitComponent.UnitType)
                    {
                        continue;
                    }

                    AttackUnit(world, entity, detectedEntity);
                }
            }
        }

        private void AttackUnit(EcsWorld ecsWorld, int entityUnit, int entityTarget)
        {
            Debug.Log($"[{nameof(AttackSystem)}] {nameof(AttackUnit)} Entity: {entityUnit}, Target: {entityTarget}");

            DisposeAttack(entityUnit);
            
            var cancellationToken = new CancellationTokenSource();
            
            _attackCancellationTokens.Add(entityUnit, cancellationToken);

            var positionPool = ecsWorld.GetPool<MoveComponent>();
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
                AttackTarget(ecsWorld, entityUnit, entityTarget, cancellationToken.Token);
            };
        }
        
        private async UniTask AttackTarget(EcsWorld ecsWorld, int entityUnit, int entityTarget, CancellationToken cancellationToken)
        {
            Debug.Log($"[{nameof(AttackSystem)}] {nameof(AttackTarget)} Entity: {entityUnit}, Target: {entityTarget}");
            
            ChangeUnitState(ecsWorld, entityUnit, UnitState.Attack);
            
            var targetHealthComponent =  ecsWorld.GetPool<HealthComponent>().Get(entityTarget);
            var unitComponent =  ecsWorld.GetPool<UnitComponent>().Get(entityUnit);
            var attackCondition = new AttackCondition(ecsWorld, entityUnit, entityTarget);
            
            while (attackCondition.Check())
            {
                targetHealthComponent.ChangeHealth(-unitComponent.AttackData.AttackDamage);
                
                await UniTask.Delay(TimeSpan.FromSeconds(unitComponent.AttackData.AttackDelay), cancellationToken: cancellationToken).SuppressCancellationThrow();
                
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
            }
            
            ChangeUnitState(ecsWorld, entityUnit, UnitState.Stay);
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
