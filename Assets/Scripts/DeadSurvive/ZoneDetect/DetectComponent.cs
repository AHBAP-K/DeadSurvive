using System.Collections.Generic;

namespace DeadSurvive.ZoneDetect
{
    public struct DetectComponent
    {
        public float DetectDistance { get; set; }

        public List<int> DetectedEntities { get; set; }
    }
}