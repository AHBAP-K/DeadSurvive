using Cysharp.Threading.Tasks;
using DeadSurvive.Attack;
using DeadSurvive.Health;
using DeadSurvive.Moving;
using DeadSurvive.Unit;
using DeadSurvive.Unit.Data;
using DeadSurvive.ZoneDetect;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.Spawner
{
    public abstract class UnitSpawner
    {
        protected EcsWorld EcsWorld;
        protected Pool.Pool Pool;

        protected async UniTask<int> SpawnUnit(UnitData unitData, Vector3 position)
        {
            var unitObject = await Pool.SpawnObject(unitData.Prefab, position);
            var unitEntity = EcsWorld.NewEntity();

            unitObject.transform.parent = null;

            EcsWorld.AddUnitComponent(unitData.Type, unitObject.transform, unitEntity);
            EcsWorld.AddMoveComponent(unitData.MoveData, unitEntity);
            EcsWorld.AddAttackComponent(unitData.AttackConfig, unitEntity);
            EcsWorld.AddHealthComponent(unitData.HealthData, unitEntity, unitObject.transform);
            EcsWorld.AddDetectComponent(unitObject.transform, unitEntity);
            
            return unitEntity;
        }
    }
}