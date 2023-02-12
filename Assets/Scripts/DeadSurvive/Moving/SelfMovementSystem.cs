using DeadSurvive.Common;
using DeadSurvive.Condition;
using DeadSurvive.Moving.Data;
using DeadSurvive.Unit;
using DeadSurvive.Unit.Enum;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.Moving
{
    public class SelfMovementSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var filterUnit = world.Filter<UnitComponent>().Inc<SelfMoveComponent>().End();

            var unitPool = world.GetPool<UnitComponent>();
            var transformPool = world.GetPool<UnityObject<Transform>>();
            var movePool = world.GetPool<MoveDestinationComponent>();
            var selfMovePool = world.GetPool<SelfMoveComponent>();
            
            foreach (var unitEntity in filterUnit)
            {
                ref var unitComponent = ref unitPool.Get(unitEntity);
                ref var selfMoveComponent = ref selfMovePool.Get(unitEntity);
                ref var transformComponent = ref transformPool.Get(unitEntity);

                if (unitComponent.UnitState != UnitState.Stay)
                {
                    continue;
                }

                selfMoveComponent.Delay -= Time.deltaTime;

                if (selfMoveComponent.Delay > 0)
                {
                    continue;
                }
                
                selfMoveComponent.RefreshDelay();
                
                var newPosition = transformComponent.Value.position + new Vector3(Random.Range(-1f, 1), Random.Range(-1f, 1), 0f);
                var moveCondition = new MoveToPositionCondition(transformComponent.Value ,newPosition, 0.1f);
                var vectorPosition = new VectorPositionHolder(newPosition);
                
                ref var moveComponent = ref movePool.GetOrAdd(unitEntity);
                moveComponent.Configure(vectorPosition, moveCondition);
            }
        }
    }
}