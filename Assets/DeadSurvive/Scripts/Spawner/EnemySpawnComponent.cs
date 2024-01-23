using System.Collections.Generic;
using DeadSurvive.Level;

namespace DeadSurvive.Spawner
{
    public struct EnemySpawnComponent
    {
        public float DelaySpawn { get; set; }

        public int EnemyCount { get; private set; }
        
        public int SpawnedEnemyCount { get; set; }
        
        public List<SpawnPoints> Positions { get; set; }

        private float _defaultDelay;

        public void SetupEnemyData(EnemySpawnData enemySpawnData)
        {
            _defaultDelay = enemySpawnData.DelaySpawn;
            
            DelaySpawn = enemySpawnData.DelaySpawn;
            EnemyCount = enemySpawnData.MaxEnemies;
        }
        
        public void SetupPositions(List<SpawnPoints> positions)
        {
            Positions = positions;
        }

        public void ResetDelay()
        {
            DelaySpawn = _defaultDelay;
        }
    }
}