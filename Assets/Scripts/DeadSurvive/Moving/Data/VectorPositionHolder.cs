using UnityEngine;

namespace DeadSurvive.Moving.Data
{
    public readonly struct VectorPositionHolder : IPositionHolder
    {
        public Vector3 Position { get; }

        public VectorPositionHolder(Vector3 position)
        {
            Position = position;
        }
    }
}