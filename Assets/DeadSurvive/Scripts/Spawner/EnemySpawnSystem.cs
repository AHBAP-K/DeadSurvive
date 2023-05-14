using DeadSurvive.Common.Data;
using DeadSurvive.Unit.Enum;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.Spawner
{
    public class EnemySpawnSystem : IEcsInitSystem, IEcsRunSystem
    {
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var entity = world.NewEntity();
            var gameData = systems.GetShared<GameData>();
            var enemySpawnPool = world.GetPool<EnemySpawnComponent>();

            ref var enemySpawnComponent = ref enemySpawnPool.Add(entity);
            
            enemySpawnComponent.Setup(gameData.EnemySpawnData);
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var gameData = systems.GetShared<GameData>();
            var enemySpawnFilter = world.Filter<EnemySpawnComponent>().End();
            var enemySpawnPool = world.GetPool<EnemySpawnComponent>();
            
            foreach (var entity in enemySpawnFilter)
            {
                ref var enemySpawnComponent = ref enemySpawnPool.Get(entity);
                enemySpawnComponent.DelaySpawn -= Time.deltaTime;

                if (enemySpawnComponent.DelaySpawn > 0)
                {
                    continue;
                }
                
                var spawnPool = world.GetPool<SpawnComponent>();
                var spawnEntity = world.NewEntity();
                var unitData = gameData.GetUnitData(UnitType.Enemy);
                var position = gameData.GetUnitSpawnData(UnitType.Enemy);

                ref var spawnComponent = ref spawnPool.Add(spawnEntity);
                
                spawnComponent.Setup(unitData, position.GetTargetPosition());
                
                enemySpawnComponent.ResetDelay();
            }
        }
    }
}