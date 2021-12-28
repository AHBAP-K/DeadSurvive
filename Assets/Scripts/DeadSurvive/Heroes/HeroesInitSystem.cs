using DeadSurvive.Common.Data;
using DeadSurvive.Moving;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.Heroes
{
    public class HeroesInitSystem : IEcsInitSystem
    {
        public void Init(EcsSystems systems)
        {
            var world = systems.GetWorld();
            var data = systems.GetShared<GameData>();
            var movePool = world.GetPool<MoveComponent>();

            for (int i = 0; i < data.HeroesPrefabs.Length; i++)
            {
                var heroEntity = world.NewEntity();
                var hero = Object.Instantiate(data.HeroesPrefabs[i], data.HeroesSpawnPoint);
                ref var moveComponent = ref movePool.Add(heroEntity);
                moveComponent.TargetTransform = hero.transform;
                moveComponent.MoveData = data.MoveData;
            }
        }
    }
}