using Leopotam.EcsLite;

namespace DeadSurvive.Common
{
    public static class Extensions
    {
        public static ref T GetOrAdd<T>(this EcsPool<T> ecsPool, int entity) where T : struct
        {
            return ref ecsPool.Has(entity) ? ref ecsPool.Get(entity) : ref ecsPool.Add(entity);
        }
    }
}