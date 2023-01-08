using DeadSurvive.Unit.Data;
using UnityEngine;

namespace DeadSurvive.Spawner
{
    public struct SpawnComponent
    {
        public UnitData UnitData { get; private set; }
        
        public Vector3 SpawnPosition { get; private set; }

        public void Setup(UnitData unitData, Vector3 spawnPosition)
        {
            UnitData = unitData;
            SpawnPosition = spawnPosition;
        }
    }
}