using System;
using DeadSurvive.Condition.Interfaces;
using DeadSurvive.Moving.Data;

namespace DeadSurvive.Moving
{
    public struct MoveDestinationComponent
    {
        public IPositionHolder PositionHolder { get; private set; }
        
        public ICondition Condition { get; private set; }

        public Action ReachedTarget { get; set; }

        public void Configure(IPositionHolder targetPosition, ICondition condition)
        {
            PositionHolder = targetPosition;
            Condition = condition;
            ReachedTarget = delegate {  };
        }
    }
}