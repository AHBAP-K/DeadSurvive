using UnityEngine;

namespace DeadSurvive.Attack.Data
{
    [CreateAssetMenu(fileName = "AttackData", menuName = "DeadSurvive/Attack", order = 0)]
    public class AttackData : ScriptableObject
    {
        public float AttackDamage => _attackDamage;

        public float AttackDelay => _attackDelay;

        public float AttackRange => _attackRange;

        [SerializeField] 
        private float _attackDamage;

        [SerializeField] 
        private float _attackRange = 1f;

        [SerializeField] 
        private float _attackDelay = 1f;
    }
}