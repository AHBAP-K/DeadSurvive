using Leopotam.EcsLite;

namespace DeadSurvive.Attack
{
    public static class AttackExtensions
    {
        public static void AddAttackComponent(this EcsWorld world, AttackConfig healthConfig, int entity)
        {
            var attackPool = world.GetPool<AttackComponent>();
            
            ref var attackComponent = ref attackPool.Add(entity);

            attackComponent.Setup(healthConfig);
        }
    }
}