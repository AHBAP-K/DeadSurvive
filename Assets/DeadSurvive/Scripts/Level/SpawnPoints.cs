using System;
using System.Collections.Generic;
using UnityEngine;

namespace DeadSurvive.Level
{
    [Serializable]
    public class SpawnPoints
    {
        public DirectionType DirectionType => _directionType;

        public Transform Point => _point;

        [SerializeField] 
        private DirectionType _directionType;

        [SerializeField]
        private Transform _point;
    }
}