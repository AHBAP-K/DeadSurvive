namespace DeadSurvive.Spawner
{
    public struct EnemySpawnComponent
    {
        public float DelaySpawn { get; set; }

        public int MaxEnemies { get; private set; }

        private float _defaultDelay;

        public void Setup(EnemySpawnData enemySpawnData)
        {
            _defaultDelay = enemySpawnData.DelaySpawn;
            
            DelaySpawn = enemySpawnData.DelaySpawn;
            MaxEnemies = enemySpawnData.MaxEnemies;
        }

        public void ResetDelay()
        {
            DelaySpawn = _defaultDelay;
        }
    }
}