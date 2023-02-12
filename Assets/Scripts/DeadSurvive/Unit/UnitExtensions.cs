using DeadSurvive.Common;
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
            var transformPool = world.GetPool<UnityObject<Transform>>();

            ref var unitComponent = ref unitPool.Add(entity);
            ref var transformComponent = ref transformPool.Add(entity);

            unitComponent.UnitState = UnitState.Stay;
            unitComponent.UnitType = unitType;
            
            transformComponent.Value = transform;
        }
    }
}