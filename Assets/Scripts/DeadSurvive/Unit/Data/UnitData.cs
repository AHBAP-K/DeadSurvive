using DeadSurvive.Moving.Data;
using DeadSurvive.Unit.Enum;
using UnityEngine;

namespace DeadSurvive.Unit.Data
{
    [CreateAssetMenu(fileName = "UnitData", menuName = "DeadSurvive/UnitData", order = 0)]
    public class UnitData : ScriptableObject
    {
        public GameObject Prefab => _prefab;

        public MoveData MoveData => _moveData;

        public float DetectDistance => _detectDistance;

        public float MaxHealth => _maxHealth;

        public float AttackDamage => _attackDamage;

        public UnitType Type => _unitType;

        [SerializeField] private GameObject _prefab;

        [SerializeField] private MoveData _moveData;

        [SerializeField] private UnitType _unitType;
        
        [SerializeField] private float _maxHealth;
        
        [SerializeField] private float _detectDistance;

        [SerializeField] private float _attackDamage;
    }
}