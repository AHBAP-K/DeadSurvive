using System;
using DeadSurvive.HeroButton;
using DeadSurvive.Unit.Data;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.Spawner
{
    public class HeroSpawn : ISpawnUnit
    {
        private EcsWorld _ecsWorld;
        private EcsPool<ButtonComponent> _buttonComponent;

        public void Setup(EcsWorld ecsWorld)
        {
            _ecsWorld = ecsWorld;
            _buttonComponent = ecsWorld.GetPool<ButtonComponent>();
        }

        public int Spawn<T>(T data, Vector3 position) where T : UnitData
        {
            if (data is not HeroUnitData heroUnitData)
            {
                throw new Exception("Wrong data");
            }
            
            var entity = _ecsWorld.UnitSpawn(heroUnitData, position);
            ref var buttonTag = ref _buttonComponent.Add(entity);
            return entity;
        }
    }
}