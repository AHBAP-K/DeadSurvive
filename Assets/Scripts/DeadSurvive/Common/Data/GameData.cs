using System;
using System.Collections.Generic;
using DeadSurvive.Unit.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DeadSurvive.Common.Data
{
    [Serializable]
    public class GameData
    {
        public List<Transform> HeroesSpawnPoint => _heroesSpawnPoint;

        public UnitData[] UnitData => _unitData;
        
        public GameObject ButtonPrefab => _buttonPrefab;

        public Transform ButtonSpawnPoint => _buttonSpawnPoint;

        [SerializeField, FoldoutGroup("Heroes")] 
        private List<Transform> _heroesSpawnPoint;

        [SerializeField, FoldoutGroup("Heroes")]
        private UnitData[] _unitData;

        [SerializeField, FoldoutGroup("UI")]
        private GameObject _buttonPrefab;
        
        [SerializeField, FoldoutGroup("UI")]
        private Transform _buttonSpawnPoint;
    }
}