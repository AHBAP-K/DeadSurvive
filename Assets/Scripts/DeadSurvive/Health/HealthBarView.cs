using UnityEngine;

namespace DeadSurvive.Health
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField] 
        private Transform _bar;

        [SerializeField] 
        private float _maxPositionX = 1f;
        
        [SerializeField] 
        private float _minPositionX = 0f;

        public void MoveBar(float direction)
        {
            var position = _bar.localPosition;
            var newBarPositionX = Mathf.Clamp(position.x + direction / 100f, _minPositionX, _maxPositionX);
            position = new Vector3(newBarPositionX , position.y, position.z);
            _bar.localPosition = position;
        }
    }
}