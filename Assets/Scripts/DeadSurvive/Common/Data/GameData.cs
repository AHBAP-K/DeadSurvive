using System;
using System.Collections.Generic;
using System.Linq;
using DeadSurvive.Unit.Data;
using DeadSurvive.Unit.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DeadSurvive.Common.Data
{
    [Serializable]
    public class GameData
    {
        public UnitData[] UnitData => _unitData;
        
        public GameObject ButtonPrefab => _buttonPrefab;

        public Transform ButtonSpawnPoint => _buttonSpawnPoint;
        
        [SerializeField, FoldoutGroup("Units")] 
        private List<PositionDataHolder> _unitSpawnPoint;

        [SerializeField, FoldoutGroup("Units")]
        private UnitData[] _unitData;

        [SerializeField, FoldoutGroup("UI")]
        private GameObject _buttonPrefab;
        
        [SerializeField, FoldoutGroup("UI")]
        private Transform _buttonSpawnPoint;

        public Vector3 GetUnitSpawnPosition(UnitType unitType)
        {
            foreach (var positionDataHolder in _unitSpawnPoint.Where(t => t.Type == unitType))
            {
                return positionDataHolder.GetTargetPosition();
            }

            return Vector3.zero;
        }
    }
}