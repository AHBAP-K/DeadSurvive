using DeadSurvive.Common.Data;
using DeadSurvive.Unit.Enum;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace DeadSurvive.Spawner
{
    public class EnemySpawnSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsSharedInject<GameData> _gameData = default;

        private readonly EcsPoolInject<SpawnComponent> _spawnPool = default;
        private readonly EcsPoolInject<EnemySpawnComponent> _enemySpawnPool = default;
        
        private readonly EcsFilterInject<Inc<EnemySpawnComponent>> _enemySpawnFilter = default;
        
        public void Init(IEcsSystems systems)
        {
            // var world = systems.GetWorld();
            // var entity = world.NewEntity();
            // var gameData = systems.GetShared<GameData>();
            // var enemySpawnPool = world.GetPool<EnemySpawnComponent>();
            //
            // ref var enemySpawnComponent = ref enemySpawnPool.Add(entity);
            //
            // enemySpawnComponent.Setup(gameData.EnemySpawnData);
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _enemySpawnFilter.Value)
            {
                ref var enemySpawnComponent = ref _enemySpawnPool.Value.Get(entity);

                enemySpawnComponent.DelaySpawn -= Time.deltaTime;

                if (enemySpawnComponent.DelaySpawn > 0)
                {
                    continue;
                }
                
                var spawnEntity = _world.Value.NewEntity();
                var unitData = _gameData.Value.GetUnitData(UnitType.Enemy);
                var random = Random.Range(0, enemySpawnComponent.Positions.Count);

                ref var spawnComponent = ref _spawnPool.Value.Add(spawnEntity);
                
                // todo: fix this
                spawnComponent.Setup(unitData, enemySpawnComponent.Positions[random].Point.position);

                enemySpawnComponent.SpawnedEnemyCount++;
                enemySpawnComponent.ResetDelay();

                if (enemySpawnComponent.SpawnedEnemyCount >= enemySpawnComponent.EnemyCount)
                {
                    _enemySpawnPool.Value.Del(entity);
                }
            }
        }
    }
}