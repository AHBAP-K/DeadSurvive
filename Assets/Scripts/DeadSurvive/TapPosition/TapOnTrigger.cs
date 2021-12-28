using DeadSurvive.Common;
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
            var pressPositionEntity = ecsWorld.NewEntity();
            var pressPositionPool = ecsWorld.GetPool<PressPositionComponent>(); 
            
            ref var pressComponent = ref pressPositionPool.Add(pressPositionEntity);

            var pointerPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            pressComponent.SetPosition(pointerPosition);
        }
    }
}