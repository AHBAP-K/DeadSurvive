using DeadSurvive.Moving.Data;
using UnityEngine;

namespace DeadSurvive.Unit.Data
{
    [CreateAssetMenu(fileName = "UnitData", menuName = "DeadSurvive/UnitData", order = 0)]
    public class UnitData : ScriptableObject
    {
        public GameObject Prefab => _prefab;

        public MoveData MoveData => _moveData;

        public float DetectDistance => _detectDistance;

        [SerializeField] private GameObject _prefab;
        
        [SerializeField] private MoveData _moveData;

        [SerializeField] private float _detectDistance;
    }
}