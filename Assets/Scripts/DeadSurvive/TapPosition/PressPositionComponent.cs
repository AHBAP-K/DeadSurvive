using System;
using UnityEngine;

namespace DeadSurvive.TapPosition
{
    [Serializable]
    public struct PressPositionComponent
    {
        public Vector2 TargetPosition => _targetPosition;

        private Vector2 _targetPosition;

        public void SetPosition(Vector2 targetPosition)
        {
            _targetPosition = targetPosition;
        }
    }
}