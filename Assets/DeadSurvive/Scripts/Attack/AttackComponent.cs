namespace DeadSurvive.Attack
{
    public struct AttackComponent
    {
        public float AttackDamage { get; private set; }

        public float AttackRange { get; private set; }

        public float AttackDetectRange { get; private set; }
        
        public float Delay { get; set; }

        private float _attackDelay;

        public void Setup(AttackConfig attackConfig)
        {
            _attackDelay = attackConfig.AttackDelay;
            AttackDamage = attackConfig.AttackDamage;
            AttackRange = attackConfig.AttackRange;
            AttackDetectRange = attackConfig.AttackDetectRange;
            
            RefreshDelay();
        }

        public void RefreshDelay()
        {
            Delay = _attackDelay;
        }
    }
}