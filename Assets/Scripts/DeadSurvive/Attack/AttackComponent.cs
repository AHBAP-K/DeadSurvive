namespace DeadSurvive.Attack
{
    public struct AttackComponent
    {
        public float AttackDamage { get; private set; }

        public float AttackDelay { get; private set; }

        public float AttackRange { get; private set; }

        public float AttackDetectRange { get; private set; }

        public void Setup(AttackData attackData)
        {
            AttackDamage = attackData.AttackDamage;
            AttackDelay = attackData.AttackDelay;
            AttackRange = attackData.AttackRange;
            AttackDetectRange = attackData.AttackDetectRange;
        }
    }
}