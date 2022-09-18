namespace DeadSurvive.Attack
{
    public struct AttackComponent
    {
        public float AttackDamage { get; private set; }

        public float AttackRange { get; private set; }

        public float AttackDetectRange { get; private set; }
        
        public float Delay { get; set; }

        private float _attackDelay;

        public void Setup(AttackData attackData)
        {
            _attackDelay = attackData.AttackDelay;
            AttackDamage = attackData.AttackDamage;
            AttackRange = attackData.AttackRange;
            AttackDetectRange = attackData.AttackDetectRange;
            
            RefreshDelay();
        }

        public void RefreshDelay()
        {
            Delay = _attackDelay;
        }
    }
}