using UnityEngine;

namespace DeadSurvive.Health
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField] 
        private Transform _bar;

        public void MoveBar(float positionX)
        {
            var position = _bar.localPosition;
            position = new Vector3(positionX, position.y, position.z);
            _bar.localPosition = position;
        }
    }
}