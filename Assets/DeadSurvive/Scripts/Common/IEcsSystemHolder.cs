using Leopotam.EcsLite;

namespace DeadSurvive.Common
{
    public interface IEcsSystemHolder
    {
        EcsSystems EcsSystems { get; set; }
    }
}