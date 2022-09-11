using System.Collections.Generic;
using DeadSurvive.Attack;
using DeadSurvive.Health;
using DeadSurvive.Moving;
using DeadSurvive.Unit;
using DeadSurvive.Unit.Data;
using DeadSurvive.Unit.Enum;
using DeadSurvive.ZoneDetect;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.Spawner
{
    public static class UnitExtensions
    {
        public static int UnitSpawn(this EcsWorld world, UnitData unitData, Vector3 position)
        {
            var detectPool = world.GetPool<DetectComponent>();
            var unitPool = world.GetPool<UnitComponent>();
            
            var unitEntity = world.NewEntity();
            var prefab = unitData.Prefab;
            var unitObject = Object.Instantiate(prefab, position, Quaternion.identity);
            
            ref var detectUnitComponent = ref detectPool.Add(unitEntity);
            ref var unitComponent = ref unitPool.Add(unitEntity);

            unitComponent.UnitState = UnitState.Stay;
            unitComponent.UnitType = unitData.Type;
            unitComponent.UnitTransform = unitObject.transform;

            world.AddMoveComponent(unitData.MoveData, unitEntity);
            world.AddAttackComponent(unitData.AttackData, unitEntity);
            world.AddHealthComponent(unitData.HealthData, unitEntity, unitObject.transform);

            detectUnitComponent.ObjectTransform = unitObject.transform;
            detectUnitComponent.DetectedEntities = new List<DetectedEntity>(5);

            return unitEntity;
        }
    }
}