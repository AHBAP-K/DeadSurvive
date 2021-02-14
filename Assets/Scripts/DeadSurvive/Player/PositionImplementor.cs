using Svelto.ECS.Hybrid;
using UnityEngine;

namespace DeadSurvive.Player
{
    public class PositionImplementor : MonoBehaviour, IPositionComponent, IImplementor
    {
        public Vector2 position
        {
            get => _transform.position;
            set => _transform.position = value;
        }

        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }
    }
}