using DeadSurvive.Common.Data;
using DeadSurvive.HeroButton;
using Leopotam.EcsLite;

namespace DeadSurvive.Spawner
{
    public class HeroSpawnSystem : IEcsInitSystem
    {
        public void Init(EcsSystems systems)
        {
            var world = systems.GetWorld();
            var data = systems.GetShared<GameData>();
            var buttonTagPool = world.GetPool<ButtonTag>();

            for (int i = 0; i < data.UnitData.Length; i++)
            {
                var position = data.GetUnitSpawnPosition(data.UnitData[i].Type);
                var unitEntity = data.UnitData[i].UnitSpawn(world, position);
                buttonTagPool.Add(unitEntity);
            }
        }
    }
}