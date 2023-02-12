using DeadSurvive.Common;
using DeadSurvive.Unit;
using DeadSurvive.Unit.Enum;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.Moving
{
    public class MovementSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var filterUnit = world.Filter<UnitComponent>().Inc<MoveDestinationComponent>().End();

            foreach (var unitEntity in filterUnit)
            {
                MoveObject(world, unitEntity);
            }
        }

        private void MoveObject(EcsWorld world, int unitEntity)
        {
            Debug.Log($"[{nameof(MovementSystem)}] Entity: {unitEntity}, {nameof(MoveObject)}");
            
            ref var moveComponent = ref world.GetPool<MoveComponent>().Get(unitEntity);
            ref var transformComponent = ref world.GetPool<UnityObject<Transform>>().Get(unitEntity);
            ref var moveDestinationComponent = ref world.GetPool<MoveDestinationComponent>().Get(unitEntity);
            ref var unitComponent = ref world.GetPool<UnitComponent>().Get(unitEntity);
            
            unitComponent.UnitState = UnitState.Move;

            if (moveDestinationComponent.Condition.Check())
            {
                var speed = moveComponent.Speed * Time.deltaTime;
                var newPosition = Vector2.MoveTowards(transformComponent.Value.position, moveDestinationComponent.PositionHolder.Position, speed);
                
                transformComponent.Value.position = newPosition;
                
                return;
            }

            MovementComplete(world, unitEntity);
        }
        
        private void MovementComplete(EcsWorld ecsWorld, int entity)
        {
            Debug.Log($"[{nameof(MovementSystem)}] Entity: {entity}, {nameof(MovementComplete)}");
            
            var moveDestinationPool = ecsWorld.GetPool<MoveDestinationComponent>();

            ref var moveDestinationComponent = ref moveDestinationPool.Get(entity);
            ref var unitComponent = ref ecsWorld.GetPool<UnitComponent>().Get(entity);
            
            unitComponent.UnitState = UnitState.Stay;

            moveDestinationComponent.ReachedTarget?.Invoke();
            moveDestinationPool.Del(entity);
        }
    }
}