using System;
using Cysharp.Threading.Tasks;
using DeadSurvive.Common.Data;
using DeadSurvive.UnitButton;
using DeadSurvive.Unit.Data;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.Spawner
{
    public class HeroSpawn : UnitSpawner, ISpawnUnit
    {
        private SelectButtonSpawner _selectButtonSpawner;
        private GameData _gameData;
        
        public void Setup(IEcsSystems systems)
        {
            _gameData = systems.GetShared<GameData>();
            
            EcsWorld = systems.GetWorld();
            Pool = _gameData.Pool;

            _selectButtonSpawner = new SelectButtonSpawner();
            _selectButtonSpawner.Setup(EcsWorld, Pool);
        }

        public async UniTask<int> Spawn<T>(T data, Vector3 position) where T : UnitData
        {
            if (data is not HeroUnitData heroUnitData)
            {
                throw new Exception("Wrong data");
            }

            var entity = await SpawnUnit(heroUnitData, position);
            _selectButtonSpawner.SpawnButton(entity, _gameData.ButtonPrefab, _gameData.ButtonSpawnPoint).Forget();
            return entity;
        }
    }
}