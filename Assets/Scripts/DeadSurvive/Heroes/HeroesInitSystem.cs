using System.Collections.Generic;
using DeadSurvive.Attack;
using DeadSurvive.Common.Data;
using DeadSurvive.Unit;
using DeadSurvive.Unit.Enum;
using DeadSurvive.ZoneDetect;
using Leopotam.EcsLite;
using Unity.Mathematics;
using UnityEngine;

namespace DeadSurvive.Heroes
{
    public class HeroesInitSystem : IEcsInitSystem
    {
        public void Init(EcsSystems systems)
        {
            var world = systems.GetWorld();
            var data = systems.GetShared<GameData>();
            var detectPool = world.GetPool<DetectUnitComponent>();
            var attackPool = world.GetPool<AttackComponent>();
            var unitPool = world.GetPool<UnitComponent>();

            for (int i = 0; i < data.UnitData.Length; i++)
            {
                var heroEntity = world.NewEntity();
                var prefab = data.UnitData[i].Prefab;
                var position = data.HeroesSpawnPoint[i % data.HeroesSpawnPoint.Count].position;
                var hero = Object.Instantiate(prefab, position, Quaternion.identity);
                
                ref var detectUnitComponent = ref detectPool.Add(heroEntity);
                ref var attackComponent = ref attackPool.Add(heroEntity);
                ref var unitComponent = ref unitPool.Add(heroEntity);
                
                unitComponent.UnitEntity = heroEntity;
                unitComponent.UnitState = UnitState.Stay;
                unitComponent.UnitType = UnitType.Hero;
                unitComponent.UnitTransform = hero.transform;
                unitComponent.MoveData = data.UnitData[i].MoveData;

                detectUnitComponent.DetectDistance = data.UnitData[i].DetectDistance;
                detectUnitComponent.DetectedUnitEntities = new List<int>(10);
            }
        }
    }
}