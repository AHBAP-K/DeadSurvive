using Leopotam.EcsLite;

namespace DeadSurvive.Attack
{
    public struct CombatComponent
    {
        public EcsPackedEntity EntityTarget { get; private set; }

        public void Setup(EcsPackedEntity entityTarget)
        {
            EntityTarget = entityTarget;
        }
    }
}