using DeadSurvive.Moving.Data;
using DeadSurvive.Unit.Enum;
using UnityEngine;

namespace DeadSurvive.Unit
{
    public struct UnitComponent
    {
        public UnitState UnitState { get; set; }
        
        public UnitType UnitType { get; set; }
        
        public Transform UnitTransform { get; set; }
    }
}