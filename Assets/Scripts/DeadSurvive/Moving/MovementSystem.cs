using DeadSurvive.Unit;
using DeadSurvive.Unit.Enum;
using Leopotam.EcsLite;

namespace DeadSurvive.Moving
{
    public class MovementSystem : IEcsRunSystem
    {
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
            var movePool = world.GetPool<MovementComponent>();

            DisposeActiveMovement(movePool, unitEntity);

            ref var targetPositionComponent = ref targetPositionPool.Get(unitEntity);
            ref var unitComponent = ref poolUnit.Get(unitEntity);
            ref var moveComponent = ref movePool.Add(unitEntity);

            unitComponent.UnitState = UnitState.Move;
            
            targetPositionComponent.ReachedTarget += () => MovementComplete(world, unitEntity);

            moveComponent.Begin(unitComponent, targetPositionComponent);
            
            targetPositionPool.Del(unitEntity);
        }

        private void DisposeActiveMovement(EcsPool<MovementComponent> movePool, int unitEntity)
        {
            if (!movePool.Has(unitEntity))
            {
                return;
            }
            
            ref var currentMoveComponent = ref movePool.Get(unitEntity);
            currentMoveComponent.Dispose();
            movePool.Del(unitEntity);
        }
        
        private void MovementComplete(EcsWorld world, int unitEntity)
        {
            var poolUnit = world.GetPool<UnitComponent>();
            var movePool = world.GetPool<MovementComponent>();

            ref var completedTarget = ref poolUnit.Get(unitEntity);

            completedTarget.UnitState = UnitState.Stay;
            
            movePool.Del(unitEntity);
        }
    }
}