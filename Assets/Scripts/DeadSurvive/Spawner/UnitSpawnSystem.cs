using DeadSurvive.Common.Data;
using Leopotam.EcsLite;

namespace DeadSurvive.Spawner
{
    public class UnitSpawnSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var data = systems.GetShared<GameData>();

            foreach (var unitsData in data.UnitSpawnData)
            {
                unitsData.SpawnUnit.Setup(world);
            }
            
            foreach (var unitData in data.UnitData)
            {
                var spawnData = data.GetUnitSpawnData(unitData.Type);
                var entity = spawnData.SpawnUnit.Spawn(unitData, spawnData.GetTargetPosition());
            }        
        }
    }
}