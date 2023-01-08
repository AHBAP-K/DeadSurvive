using UnityEngine;

namespace DeadSurvive.Spawner
{
    [CreateAssetMenu(fileName = nameof(EnemySpawnData), menuName = "DeadSurvive/EnemySpawnData", order = 0)]
    public class EnemySpawnData : ScriptableObject
    {
        public float DelaySpawn => _delaySpawn;

        public int MaxEnemies => _maxEnemies;
        
        [SerializeField]
        private float _delaySpawn;
        
        [SerializeField]
        private int _maxEnemies;
    }
}