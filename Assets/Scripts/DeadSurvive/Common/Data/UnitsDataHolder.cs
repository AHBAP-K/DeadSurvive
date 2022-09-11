using System;
using System.Collections.Generic;
using DeadSurvive.Spawner;
using DeadSurvive.Unit.Enum;
using UnityEngine;

namespace DeadSurvive.Common.Data
{
    [Serializable]
    public class UnitsDataHolder
    {
        public UnitType Type => _unitType;

        public ISpawnUnit SpawnUnit => _spawnUnit;

        [SerializeField]
        private UnitType _unitType;

        [SerializeReference]
        private ISpawnUnit _spawnUnit;
        
        [SerializeField] 
        private List<Transform> _spawnParents;
        
        private int _currentId = 0;

        public Vector3 GetTargetPosition()
        {
            _currentId++;
            return _spawnParents[_currentId % _spawnParents.Count].position;
        }
    }
}