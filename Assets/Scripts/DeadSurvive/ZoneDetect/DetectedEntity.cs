namespace DeadSurvive.ZoneDetect
{
    public struct DetectedEntity
    {
        public int Entity { get; }
        public float Distance { get; set; }

        public DetectedEntity(int entity, float distance)
        {
            Entity = entity;
            Distance = distance;
        }
    }
}