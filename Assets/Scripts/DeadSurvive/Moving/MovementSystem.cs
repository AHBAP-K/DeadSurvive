using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DeadSurvive.Unit;
using DeadSurvive.Unit.Enum;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.Moving
{
    public class MovementSystem : IEcsRunSystem
    {
        private readonly Dictionary<int, CancellationTokenSource> _moveCancellationTokens = new Dictionary<int, CancellationTokenSource>();
        
        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();
            
            var filterUnit = world.Filter<UnitComponent>().Inc<TargetPositionComponent>().End();

            foreach (var unitEntity in filterUnit)
            {
                MoveUnit(world, unitEntity);
            }
        }
        
        private void MoveUnit(EcsWorld world, int unitEntity)
        {
            var poolUnit = world.GetPool<UnitComponent>();
            var targetPositionPool = world.GetPool<TargetPositionComponent>();

            DisposeMoving(unitEntity);
            
            ref var targetPositionComponent = ref targetPositionPool.Get(unitEntity);
            ref var unitComponent = ref poolUnit.Get(unitEntity);
            
            unitComponent.UnitState = UnitState.Move;
            targetPositionComponent.ReachedTarget += () => MovementComplete(world, unitEntity);
            
            var cancellationToken = new CancellationTokenSource();
            var dummy = MoveObject(world, unitEntity, cancellationToken.Token);
            
            _moveCancellationTokens.Add(unitEntity, cancellationToken);
            
            targetPositionPool.Del(unitEntity);
        }

        private void DisposeMoving(int entity)
        {
            if (!_moveCancellationTokens.ContainsKey(entity))
            {
                return;
            }
            
            _moveCancellationTokens[entity].Cancel();
            _moveCancellationTokens[entity].Dispose();
            _moveCancellationTokens.Remove(entity);
        }
        
        private async UniTask MoveObject(EcsWorld world, int unitEntity, CancellationToken token)
        {
            var unitComponent = world.GetPool<UnitComponent>().Get(unitEntity);
            var targetComponent = world.GetPool<TargetPositionComponent>().Get(unitEntity);
            
            var unitTransform = unitComponent.UnitTransform;
            var distance = Vector2.Distance(unitTransform.position, targetComponent.PositionHolder.Position);

            while (distance > targetComponent.CompleteDistance)
            {
                var speed = unitComponent.MoveData.Speed * Time.deltaTime;
                var newPosition = Vector2.MoveTowards(unitTransform.position, targetComponent.PositionHolder.Position, speed);
                
                unitTransform.position = newPosition;

                await UniTask.WaitForEndOfFrame(token).SuppressCancellationThrow();
                
                distance = Vector2.Distance(newPosition, targetComponent.PositionHolder.Position);

                if (token.IsCancellationRequested)
                {
                    return;
                }
            }
            
            targetComponent.ReachedTarget?.Invoke();
        }

        private void MovementComplete(EcsWorld ecsWorld, int entity)
        {
            ref var unitComponent = ref ecsWorld.GetPool<UnitComponent>().Get(entity);
            unitComponent.UnitState = UnitState.Stay;
        }
    }
}