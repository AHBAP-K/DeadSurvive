using System;
using DeadSurvive.Moving;
using DeadSurvive.Unit.Data;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.Spawner
{
    public class EnemySpawn : ISpawnUnit
    {
        private EcsWorld _ecsWorld;

        public void Setup(EcsWorld ecsWorld)
        {
            _ecsWorld = ecsWorld;
        }

        public int Spawn<T>(T data, Vector3 position) where T : UnitData
        {
            if (data is not EnemyUnitData enemyUnitData)
            {
                throw new Exception("Wrong data");
            }
            
            var entity = _ecsWorld.UnitSpawn(enemyUnitData, position);
            _ecsWorld.AddSelfMoveComponent(enemyUnitData.SelfMoveData, entity);
            return entity;
        }
    }
}