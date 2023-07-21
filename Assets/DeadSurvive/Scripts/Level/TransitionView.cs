using DeadSurvive.Common;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DeadSurvive.Level
{
    public class TransitionView : MonoBehaviour, IPointerClickHandler, IEcsWorldReceiver
    {
        public DirectionType Direction => _direction;

        [SerializeField] 
        private DirectionType _direction;
        
        private EcsWorld _ecsWorld;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"Clicked on transition {Direction}");
            
            var transitionEntity = _ecsWorld.NewEntity();
            var transitionPool = _ecsWorld.GetPool<TransitionComponent>();
            
            ref var transitionComponent = ref transitionPool.Add(transitionEntity);
            
            transitionComponent.SetDirection(Direction);
        }

        public void SetEcsWorld(EcsWorld world)
        {
            _ecsWorld = world;
        }
    }
}