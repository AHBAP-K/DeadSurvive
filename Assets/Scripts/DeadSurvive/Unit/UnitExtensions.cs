using DeadSurvive.Unit.Data;
using DeadSurvive.Unit.Enum;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.Unit
{
    public static class UnitExtensions
    {
        public static void AddUnitComponent(this EcsWorld world, UnitType unitType, Transform transform, int entity)
        {
            var unitPool = world.GetPool<UnitComponent>();

            ref var unitComponent = ref unitPool.Add(entity);

            unitComponent.UnitState = UnitState.Stay;
            unitComponent.UnitType = unitType;
            unitComponent.UnitTransform = transform;
        }
    }
}