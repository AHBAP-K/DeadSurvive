using DeadSurvive.Unit;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.Moving
{
    public class SelfMovementSystem : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();

            var filterUnit = world.Filter<UnitComponent>().Inc<SelfMoveComponent>().End();

            var unitPool = world.GetPool<UnitComponent>();
            var movePool = world.GetPool<MoveComponent>();
            var selfMovePool = world.GetPool<SelfMoveComponent>();
            
            foreach (var unitEntity in filterUnit)
            {
                ref var selfMoveComponent = ref selfMovePool.Get(unitEntity);

                selfMoveComponent.Delay -= Time.deltaTime;

                if (selfMoveComponent.Delay < 0)
                {
                    selfMoveComponent.Delay = 5f;
                    ref var moveComponent = ref movePool.Add(unitEntity);
                    //moveComponent.PositionHolder
                }
            }
        }

        // private Vector3 GetPosition()
        // {
        //     
        // }
    }
}