using System;
using DeadSurvive.Moving.Data;
using UnityEngine;

namespace DeadSurvive.Moving
{
    [Serializable]
    public struct MoveComponent
    {
        public Transform TargetTransform { get; set; }

        public MoveData MoveData { get; set; }
    }
}