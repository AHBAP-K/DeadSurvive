using Cysharp.Threading.Tasks;
using DeadSurvive.Common.Data;
using Leopotam.EcsLite;

namespace DeadSurvive.Spawner
{
    public class UnitSpawnSystem : IEcsInitSystem, IEcsRunSystem
    {
        public void Init(IEcsSystems systems)
        {
            var data = systems.GetShared<GameData>();

            foreach (var unitsData in data.UnitSpawnData)
            {
                unitsData.SpawnUnit.Setup(systems);
            }
        }
        
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var data = systems.GetShared<GameData>();
            var filterUnit = world.Filter<SpawnComponent>().End();
            var spawnPool = world.GetPool<SpawnComponent>();

            foreach (var unitEntity in filterUnit)
            {
                ref var spawnComponent = ref spawnPool.Get(unitEntity);
                var spawnData = data.GetUnitSpawnData(spawnComponent.UnitData.Type);
                spawnData.SpawnUnit.Spawn(spawnComponent.UnitData, spawnComponent.SpawnPosition).Forget();
                spawnPool.Del(unitEntity);
            }            
        }
    }
}