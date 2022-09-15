using System;
using Cysharp.Threading.Tasks;
using DeadSurvive.Common.Data;
using DeadSurvive.Moving;
using DeadSurvive.Unit.Data;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.Spawner
{
    public class EnemySpawn : UnitSpawner, ISpawnUnit
    {
        public void Setup(IEcsSystems systems)
        {
            EcsWorld = systems.GetWorld();
            Pool = systems.GetShared<GameData>().Pool;
        }

        public async UniTask<int> Spawn<T>(T data, Vector3 position) where T : UnitData
        {
            if (data is not EnemyUnitData enemyUnitData)
            {
                throw new Exception("Wrong data");
            }
            
            var entity = await SpawnUnit(enemyUnitData, position);
            EcsWorld.AddSelfMoveComponent(enemyUnitData.SelfMoveData, entity);
            return entity;
        }
    }
}