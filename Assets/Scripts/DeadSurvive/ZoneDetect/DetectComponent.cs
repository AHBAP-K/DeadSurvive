using System.Collections.Generic;
using UnityEngine;

namespace DeadSurvive.ZoneDetect
{
    public struct DetectComponent
    {
        public Transform ObjectTransform { get; set; }
        
        public List<DetectedEntity> DetectedEntities { get; set; }
    }
}