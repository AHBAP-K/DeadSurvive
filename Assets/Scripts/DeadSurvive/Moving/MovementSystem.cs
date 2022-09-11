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
        private readonly Dictionary<int, CancellationTokenSource> _moveCancellationTokens = new();
        
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var filterUnit = world.Filter<UnitComponent>().Inc<MoveDestinationComponent>().End();

            foreach (var unitEntity in filterUnit)
            {
                MoveUnit(world, unitEntity);
            }
        }
        
        private void MoveUnit(EcsWorld world, int unitEntity)
        {
            var poolUnit = world.GetPool<UnitComponent>();
            var poolMove = world.GetPool<MoveDestinationComponent>();

            DisposeMoving(unitEntity);
            
            ref var unitComponent = ref poolUnit.Get(unitEntity);
            
            unitComponent.UnitState = UnitState.Move;
            
            var cancellationToken = new CancellationTokenSource();
            var dummy = MoveObject(world, unitEntity, cancellationToken.Token);
            
            _moveCancellationTokens.Add(unitEntity, cancellationToken);
            poolMove.Del(unitEntity);
        }

        private async UniTask MoveObject(EcsWorld world, int unitEntity, CancellationToken token)
        {
            Debug.Log($"[{nameof(MovementSystem)}] Entity: {unitEntity}, {nameof(MoveObject)}");
            
            var unitComponent = world.GetPool<UnitComponent>().Get(unitEntity);
            var moveComponent = world.GetPool<MoveComponent>().Get(unitEntity);
            var moveDestinationComponent = world.GetPool<MoveDestinationComponent>().Get(unitEntity);

            var unitTransform = unitComponent.UnitTransform;

            while (moveDestinationComponent.Condition.Check())
            {
                var speed = moveComponent.Speed * Time.deltaTime;
                var newPosition = Vector2.MoveTowards(unitTransform.position, moveDestinationComponent.PositionHolder.Position, speed);
                
                unitTransform.position = newPosition;

                await UniTask.WaitForEndOfFrame(token).SuppressCancellationThrow();
                
                if (token.IsCancellationRequested)
                {
                    return;
                }
            }

            MovementComplete(world, unitEntity);
            moveDestinationComponent.ReachedTarget?.Invoke();
        }
        
        private void MovementComplete(EcsWorld ecsWorld, int entity)
        {
            Debug.Log($"[{nameof(MovementSystem)}] Entity: {entity}, {nameof(MovementComplete)}");
            
            ref var unitComponent = ref ecsWorld.GetPool<UnitComponent>().Get(entity);
            unitComponent.UnitState = UnitState.Stay;

            DisposeMoving(entity);
        }
        
        private void DisposeMoving(int entity)
        {
            if (!_moveCancellationTokens.ContainsKey(entity))
            {
                return;
            }
            
            Debug.Log($"[{nameof(MovementSystem)}] Entity: {entity}, {nameof(DisposeMoving)}");

            _moveCancellationTokens[entity].Cancel();
            _moveCancellationTokens[entity].Dispose();
            _moveCancellationTokens.Remove(entity);
        }
    }
}