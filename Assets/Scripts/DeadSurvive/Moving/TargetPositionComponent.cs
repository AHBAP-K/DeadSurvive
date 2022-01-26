using System;
using DeadSurvive.Moving.Interfaces;

namespace DeadSurvive.Moving
{
    [Serializable]
    public struct TargetPositionComponent
    {
        public IPositionHolder PositionHolder { get; private set; }

        public Action ReachedTarget { get; set; }

        public void Configure(IPositionHolder targetPosition)
        {
            ReachedTarget = delegate {  };
            PositionHolder = targetPosition;
        }
    }
}