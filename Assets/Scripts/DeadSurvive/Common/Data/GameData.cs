using System;
using DeadSurvive.Moving.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DeadSurvive.Common.Data
{
    [Serializable]
    public class GameData
    {
        public Transform HeroesSpawnPoint => _heroesSpawnPoint;

        public GameObject[] HeroesPrefabs => _heroesPrefabs;

        public MoveData MoveData => _moveData;

        public GameObject ButtonPrefab => _buttonPrefab;

        public Transform ButtonSpawnPoint => _buttonSpawnPoint;

        [SerializeField, FoldoutGroup("Player")] 
        private Transform _heroesSpawnPoint;

        [SerializeField, FoldoutGroup("Player")] 
        private GameObject[] _heroesPrefabs;

        [SerializeField, FoldoutGroup("Player")] 
        private MoveData _moveData;

        [SerializeField, FoldoutGroup("UI")]
        private GameObject _buttonPrefab;
        
        [SerializeField, FoldoutGroup("UI")]
        private Transform _buttonSpawnPoint;
    }
}