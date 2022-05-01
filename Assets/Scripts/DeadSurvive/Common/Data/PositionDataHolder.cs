using System;
using System.Collections.Generic;
using DeadSurvive.Unit.Enum;
using UnityEngine;

namespace DeadSurvive.Common.Data
{
    [Serializable]
    public class PositionDataHolder
    {
        public UnitType Type => _unitType;

        [SerializeField] private UnitType _unitType;
        
        [SerializeField] private List<Transform> _targets;

        private int _currentId = 0;

        public Vector3 GetTargetPosition()
        {
            _currentId++;
            return _targets[_currentId % _targets.Count].position;
        }
    }
}