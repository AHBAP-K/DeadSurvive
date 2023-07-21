using DeadSurvive.Common;
using DeadSurvive.Condition;
using DeadSurvive.UnitButton;
using DeadSurvive.Moving;
using DeadSurvive.Moving.Data;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DeadSurvive.TapPosition
{
    public class TapOnTrigger : MonoBehaviour, IPointerClickHandler, IEcsWorldReceiver
    {
        [SerializeField] private Camera _camera;

        private EcsWorld _ecsWorld;

        private void Awake()
        {
            if (_camera == null)
            {
                _camera = Camera.main;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var movePool = _ecsWorld.GetPool<MoveDestinationComponent>(); 
            var buttonsPool = _ecsWorld.GetPool<ButtonComponent>();
            var transformPool = _ecsWorld.GetPool<UnityObject<Transform>>();
            
            var filterMove = _ecsWorld.Filter<ButtonComponent>().End();
            
            foreach (var entityMove in filterMove)
            {
                ref var buttonComponent = ref buttonsPool.Get(entityMove);

                if (!buttonComponent.IsSelected)
                {
                    continue;
                }
                
                ref var moveComponent = ref movePool.GetOrAdd(entityMove);
                ref var transformComponent = ref transformPool.Get(entityMove);
                
                var pressPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                var vectorPosition = new VectorPositionHolder(pressPosition);
                var moveCondition = new MoveToPositionCondition(transformComponent.Value ,pressPosition, 0.1f);
                
                moveComponent.Configure(vectorPosition, moveCondition);
            }
        }

        public void SetEcsWorld(EcsWorld world)
        {
            _ecsWorld = world;
        }
    }
}