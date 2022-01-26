using System.Collections.Generic;

namespace DeadSurvive.ZoneDetect
{
    public struct DetectUnitComponent
    {
        public float DetectDistance { get; set; }

        public List<int> DetectedUnitEntities { get; set; }
    }
}