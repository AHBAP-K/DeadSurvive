using UnityEngine;

namespace DeadSurvive.Health
{
    [CreateAssetMenu(fileName = "HealthData", menuName = "DeadSurvive/HealthData")]
    public class HealthData : ScriptableObject
    {
        public float MaxHealth => _maxHealth;

        public GameObject HealthBarPrefab => _healthBarPrefab;

        public Vector3 Position => _position;

        [SerializeField] 
        private float _maxHealth;
        
        [SerializeField] 
        private Vector3 _position;

        [SerializeField] 
        private GameObject _healthBarPrefab;
    }
}