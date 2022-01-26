using DeadSurvive.Common;
using DeadSurvive.HeroButton;
using DeadSurvive.Moving;
using DeadSurvive.Moving.Data;
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
            
            var pressPositionPool = ecsWorld.GetPool<TargetPositionComponent>(); 
            var buttonsPool = ecsWorld.GetPool<ButtonComponent>();
            
            var filterMove = ecsWorld.Filter<ButtonComponent>().End();
            
            foreach (var entityMove in filterMove)
            {
                ref var buttonComponent = ref buttonsPool.Get(entityMove);

                if (!buttonComponent.IsSelected)
                {
                    continue;
                }
                
                if (pressPositionPool.Has(entityMove))
                {
                    pressPositionPool.Del(entityMove);
                }

                ref var pressComponent = ref pressPositionPool.Add(entityMove);
                var vectorPosition = new VectorPositionHolder(_camera.ScreenToWorldPoint(Input.mousePosition));
                pressComponent.Configure(vectorPosition);
            }
        }
    }
}