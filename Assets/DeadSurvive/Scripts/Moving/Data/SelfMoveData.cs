using UnityEngine;

namespace DeadSurvive.Moving.Data
{
    [CreateAssetMenu(fileName = "SelfMoving", menuName = "DeadSurvive/Move/SelfMoving", order = 0)]
    public class SelfMoveData : ScriptableObject
    {
        public float Delay => _delay;

        [SerializeField]
        private float _delay;
    }
}