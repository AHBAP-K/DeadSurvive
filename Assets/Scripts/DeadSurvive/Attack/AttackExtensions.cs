using Leopotam.EcsLite;

namespace DeadSurvive.Attack
{
    public static class AttackExtensions
    {
        public static void AddAttackComponent(this EcsWorld world, AttackData healthData, int entity)
        {
            var attackPool = world.GetPool<AttackComponent>();
            
            ref var attackComponent = ref attackPool.Add(entity);

            attackComponent.Setup(healthData);
        }
    }
}