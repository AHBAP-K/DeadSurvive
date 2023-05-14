using DG.Tweening;
using UnityEngine;

namespace DeadSurvive.Moving.Data
{
    [CreateAssetMenu(fileName = "Moving", menuName = "DeadSurvive/Moving/Moving", order = 0)]
    public class MoveData : ScriptableObject
    {
        public float Speed => _speed;

        public Ease Ease => _ease;

        [SerializeField] 
        private float _speed;

        [SerializeField]
        private Ease _ease;
    }
}