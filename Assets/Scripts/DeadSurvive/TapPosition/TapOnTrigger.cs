using DeadSurvive.Common;
using DeadSurvive.Condition;
using DeadSurvive.UnitButton;
using DeadSurvive.Moving;
using DeadSurvive.Moving.Data;
using DeadSurvive.Unit;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DeadSurvive.TapPosition
{
    public class TapOnTrigger : MonoBehaviour, IPointerClickHandler, IEcsSystemHolder
    {
        public EcsSystems EcsSystems { get; set; }

        [SerializeField] private Camera _camera;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            var ecsWorld = EcsSystems.GetWorld();
            
            var movePool = ecsWorld.GetPool<MoveDestinationComponent>(); 
            var buttonsPool = ecsWorld.GetPool<ButtonComponent>();
            var unitPool = ecsWorld.GetPool<UnitComponent>();
            
            var filterMove = ecsWorld.Filter<ButtonComponent>().End();
            
            foreach (var entityMove in filterMove)
            {
                ref var buttonComponent = ref buttonsPool.Get(entityMove);

                if (!buttonComponent.IsSelected)
                {
                    continue;
                }
                
                if (movePool.Has(entityMove))
                {
                    movePool.Del(entityMove);
                }

                ref var moveComponent = ref movePool.Add(entityMove);
                ref var unitComponent = ref unitPool.Get(entityMove);
                
                var pressPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                var vectorPosition = new VectorPositionHolder(pressPosition);
                var moveCondition = new MoveToPositionCondition(unitComponent.UnitTransform ,pressPosition, 0.1f);
                
                moveComponent.Configure(vectorPosition, moveCondition);
            }
        }
    }
}