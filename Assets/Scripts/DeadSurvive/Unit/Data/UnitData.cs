using DeadSurvive.Attack;
using DeadSurvive.Health;
using DeadSurvive.Moving.Data;
using DeadSurvive.Unit.Enum;
using UnityEngine;

namespace DeadSurvive.Unit.Data
{
    public abstract class UnitData : ScriptableObject
    {
        public GameObject Prefab => _prefab;

        public MoveData MoveData => _moveData;
        
        public AttackData AttackData => _attackData;
        
        public HealthData HealthData => _healthData;
        
        public UnitType Type => _unitType;

        [SerializeField] 
        private GameObject _prefab;

        [SerializeField] 
        private MoveData _moveData;
        
        [SerializeField] 
        private HealthData _healthData;
        
        [SerializeField] 
        private AttackData _attackData;

        [SerializeField] 
        private UnitType _unitType;
    }
}