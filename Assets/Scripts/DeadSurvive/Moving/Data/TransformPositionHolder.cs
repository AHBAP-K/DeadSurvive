using DeadSurvive.Moving.Interfaces;
using UnityEngine;

namespace DeadSurvive.Moving.Data
{
    public readonly struct TransformPositionHolder : IPositionHolder
    {
        public Vector3 Position => _transform.position;

        private readonly Transform _transform;

        public TransformPositionHolder(Transform transform)
        {
            _transform = transform;
        }
    }
}