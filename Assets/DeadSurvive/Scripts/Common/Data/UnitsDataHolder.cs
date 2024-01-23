using System;
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
    }
}