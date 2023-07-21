using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.Common
{
    public static class Extensions
    {
        public static ref T GetOrAdd<T>(this EcsPool<T> ecsPool, int entity) where T : struct
        {
            return ref ecsPool.Has(entity) ? ref ecsPool.Get(entity) : ref ecsPool.Add(entity);
        }
        
        public static void Shuffle<T>(this List<T> list)
        {
            var n = list.Count;
        
            while (n > 1)
            {
                n--;
                var k = Random.Range(0, n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}