using DeadSurvive.Moving.Data;
using UnityEngine;

namespace DeadSurvive.Unit.Data
{
    [CreateAssetMenu(fileName = "EnemyUnitData", menuName = "DeadSurvive/Unit/EnemyUnitData", order = 0)]
    public class EnemyUnitData : UnitData
    {
        public SelfMoveData SelfMoveData => _selfMoveData;
        
        [SerializeField] private SelfMoveData _selfMoveData;
    }
}