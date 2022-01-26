using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DeadSurvive.Unit;
using UnityEngine;

namespace DeadSurvive.Moving
{
    public struct MovementComponent : IDisposable
    {
        private float _completeDistance;
        private CancellationTokenSource _cancellationToken;

        public void Begin(UnitComponent unitComponent, TargetPositionComponent targetPosition)
        {
            _completeDistance = 0.1f;
            _cancellationToken = new CancellationTokenSource();
            
            MoveObject(unitComponent, targetPosition, _cancellationToken.Token);
        }
        
        private async void MoveObject(UnitComponent unitComponent, TargetPositionComponent targetComponent, CancellationToken token)
        {
            var unitTransform = unitComponent.UnitTransform;
            var distance = Vector2.Distance(unitTransform.position, targetComponent.PositionHolder.Position);

            while (distance > _completeDistance)
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
        
        public void Dispose()
        {
            _cancellationToken?.Cancel();
            _cancellationToken?.Dispose();
        }
    }
}