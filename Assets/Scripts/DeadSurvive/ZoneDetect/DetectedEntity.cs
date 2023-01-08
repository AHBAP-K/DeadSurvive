using Leopotam.EcsLite;

namespace DeadSurvive.ZoneDetect
{
    public struct DetectedEntity
    {
        public EcsPackedEntity PackedEntity { get; }
        public float Distance { get; set; }

        public DetectedEntity(EcsPackedEntity packedEntity, float distance)
        {
            PackedEntity = packedEntity;
            Distance = distance;
        }
    }
}