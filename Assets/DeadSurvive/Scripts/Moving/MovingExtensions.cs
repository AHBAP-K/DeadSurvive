using DeadSurvive.Moving.Data;
using Leopotam.EcsLite;

namespace DeadSurvive.Moving
{
    public static class MovingExtensions
    {
        public static void AddMoveComponent(this EcsWorld world, MoveData moveData, int entity)
        {
            var movePool = world.GetPool<MoveComponent>();
            ref var moveComponent = ref movePool.Add(entity);
            moveComponent.Speed = moveData.Speed;
        }
        
        public static void AddSelfMoveComponent(this EcsWorld world, SelfMoveData selfMoveData, int entity)
        {
            var selfMovePool = world.GetPool<SelfMoveComponent>();
            ref var selfMoveComponent = ref selfMovePool.Add(entity);
            selfMoveComponent.Setup(selfMoveData);
        }
    }
}