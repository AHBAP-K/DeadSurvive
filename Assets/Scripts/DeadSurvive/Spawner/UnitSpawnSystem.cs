using Cysharp.Threading.Tasks;
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
                unitsData.SpawnUnit.Setup(systems);
            }
            
            foreach (var unitData in data.UnitData)
            {
                var spawnData = data.GetUnitSpawnData(unitData.Type);
                spawnData.SpawnUnit.Spawn(unitData, spawnData.GetTargetPosition()).Forget();
            }        
        }
    }
}