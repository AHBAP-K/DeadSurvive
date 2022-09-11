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
        public UnitsDataHolder GetUnitSpawnData(UnitType unitType) => _unitSpawnData.FirstOrDefault(t => t.Type == unitType);

        public UnitData[] UnitData => _unitData;
        
        public GameObject ButtonPrefab => _buttonPrefab;

        public Transform ButtonSpawnPoint => _buttonSpawnPoint;

        public List<UnitsDataHolder> UnitSpawnData => _unitSpawnData;

        [SerializeField, FoldoutGroup("Units")] 
        private List<UnitsDataHolder> _unitSpawnData;

        [SerializeField, FoldoutGroup("Units")]
        private UnitData[] _unitData;
        
        [SerializeField, FoldoutGroup("UI")]
        private GameObject _buttonPrefab;
        
        [SerializeField, FoldoutGroup("UI")]
        private Transform _buttonSpawnPoint;

    }
}