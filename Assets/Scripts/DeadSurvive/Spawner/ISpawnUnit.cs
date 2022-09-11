using DeadSurvive.Unit.Data;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.Spawner
{
    public interface ISpawnUnit
    {
        void Setup(EcsWorld ecsWorld);
        
        int Spawn<T>(T data, Vector3 position) where T : UnitData;
    }
}