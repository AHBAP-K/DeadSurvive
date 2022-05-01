using System.Collections.Generic;
using DeadSurvive.Attack;
using DeadSurvive.Common.Data;
using DeadSurvive.Health;
using DeadSurvive.Unit.Enum;
using DeadSurvive.ZoneDetect;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.Unit
{
    public class UnitInitSystem : IEcsInitSystem
    {
        public void Init(EcsSystems systems)
        {
            var world = systems.GetWorld();
            var data = systems.GetShared<GameData>();
            var detectPool = world.GetPool<DetectUnitComponent>();
            var attackPool = world.GetPool<AttackComponent>();
            var unitPool = world.GetPool<UnitComponent>();
            var healthPool = world.GetPool<HealthComponent>();

            for (int i = 0; i < data.UnitData.Length; i++)
            {
                var unitEntity = world.NewEntity();
                var unitData = data.UnitData[i];
                var prefab = unitData.Prefab;
                var position = data.GetUnitSpawnPosition(unitData.Type);
                var unitObject = Object.Instantiate(prefab, position, Quaternion.identity);
                var healthBarView = unitObject.GetComponent<HealthBarView>();
                
                ref var detectUnitComponent = ref detectPool.Add(unitEntity);
                ref var attackComponent = ref attackPool.Add(unitEntity);
                ref var unitComponent = ref unitPool.Add(unitEntity);
                ref var healthComponent = ref healthPool.Add(unitEntity);

                unitComponent.UnitEntity = unitEntity;
                unitComponent.UnitState = UnitState.Stay;
                unitComponent.UnitType = unitData.Type;
                unitComponent.UnitTransform = unitObject.transform;
                unitComponent.MoveData = unitData.MoveData;

                detectUnitComponent.DetectDistance = unitData.DetectDistance;
                detectUnitComponent.DetectedUnitEntities = new List<int>(10);
                
                attackComponent.Configure(unitData.AttackDamage);
                
                healthComponent.Configure(unitData.MaxHealth);
                healthComponent.HealthChanged += healthBarView.MoveBar;
            }
        }
    }
}