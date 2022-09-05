using DeadSurvive.Common.Data;
using Leopotam.EcsLite;

namespace DeadSurvive.Spawner
{
    public class EnemySpawnSystem : IEcsInitSystem
    {
        public void Init(EcsSystems systems)
        {
            var world = systems.GetWorld();
            var data = systems.GetShared<GameData>();

            for (int i = 0; i < data.EnemyData.Length; i++)
            {
                var position = data.GetUnitSpawnPosition(data.EnemyData[i].Type);
                data.EnemyData[i].UnitSpawn(world, position);
            }
        }
    }
}