using System;
using DeadSurvive.Condition.Interfaces;

namespace DeadSurvive.Attack
{
    public struct AttackComponent
    {
        public ICondition Condition { get; private set; }
        
        public Action ReachedTarget { get; set; }

        public void Configure(ICondition condition)
        {
            Condition = condition;
            ReachedTarget = delegate {  };
        }
    }
}