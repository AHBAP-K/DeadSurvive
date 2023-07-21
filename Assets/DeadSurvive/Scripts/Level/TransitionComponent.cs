using UnityEngine;

namespace DeadSurvive.Level
{
    public struct TransitionComponent
    {
        public DirectionType Direction { get; private set; }

        public void SetDirection(DirectionType vector2Int)
        {
            Direction = vector2Int;
        }
    }
}