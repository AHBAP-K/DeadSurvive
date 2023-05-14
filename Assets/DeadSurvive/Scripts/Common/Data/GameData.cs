using System;
using System.Collections.Generic;
using System.Linq;
using DeadSurvive.Spawner;
using DeadSurvive.Unit.Data;
using DeadSurvive.Unit.Enum;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DeadSurvive.Common.Data
{
    [Serializable]
    public class GameData
    {
        public Pool.Pool Pool { get; private set; }
        
        public UnitsDataHolder GetUnitSpawnData(UnitType unitType) => _unitSpawnData.FirstOrDefault(t => t.Type == unitType);
        
        public UnitData GetUnitData(UnitType unitType) => _unitData.FirstOrDefault(t => t.Type == unitType);

        public UnitData[] UnitData => _unitData;
        
        public AssetReference ButtonPrefab => _buttonPrefab;

        public Transform ButtonSpawnPoint => _buttonSpawnPoint;

        public List<UnitsDataHolder> UnitSpawnData => _unitSpawnData;

        public EnemySpawnData EnemySpawnData => _enemySpawnData;

        [SerializeField, FoldoutGroup("Units")] 
        private List<UnitsDataHolder> _unitSpawnData;

        [SerializeField, FoldoutGroup("Units")]
        private UnitData[] _unitData;
        
        [SerializeField, FoldoutGroup("Units")]
        private EnemySpawnData _enemySpawnData;
        
        [SerializeField, FoldoutGroup("UI")]
        private AssetReference _buttonPrefab;
        
        [SerializeField, FoldoutGroup("UI")]
        private Transform _buttonSpawnPoint;

        public void SetupPool()
        {
            Pool = new Pool.Pool();
        }
    }
}