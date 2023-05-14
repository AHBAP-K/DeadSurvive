using DeadSurvive.Condition.Interfaces;
using UnityEngine;

namespace DeadSurvive.Condition
{
    public class MoveToPositionCondition : ICondition
    {
        private readonly Vector2 _targetPosition;
        private readonly Transform _unitTransform;

        private readonly float _reachRadius;
        
        public MoveToPositionCondition(Transform unitTransform ,Vector2 targetPosition, float reachRadius)
        {
            _unitTransform = unitTransform;
            _targetPosition = targetPosition;
            _reachRadius = reachRadius;
        }
        
        public bool Check()
        {
            var distance = Vector2.Distance(_unitTransform.position, _targetPosition);
            return _reachRadius < distance;
        }
    }
}