using DeadSurvive.HeroButton;
using DeadSurvive.Moving.Data;
using DeadSurvive.TapPosition;
using DG.Tweening;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.Moving
{
    public class MoveSystem : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();
            
            var filterMove = world.Filter<MoveComponent>().Inc<ButtonComponent>().End();
            var filterPress = world.Filter<PressPositionComponent>().End();
            var poolMovable = world.GetPool<MoveComponent>();
            var buttons = world.GetPool<ButtonComponent>();
            var poolPress = world.GetPool<PressPositionComponent>();

            foreach (var entityMove in filterMove)
            {
                ref var moveComponent = ref poolMovable.Get(entityMove);
                ref var buttonComponent = ref buttons.Get(entityMove);

                foreach (var entityPress in filterPress)
                {
                    ref var pressComponent = ref poolPress.Get(entityPress);

                    if (!buttonComponent.IsSelected)
                    {
                        continue;
                    }
                    
                    MovePlayer(moveComponent.TargetTransform, pressComponent.TargetPosition, moveComponent.MoveData);
                    
                    world.DelEntity(entityPress);
                }
            }
        }

        private void MovePlayer(Transform target, Vector3 position, MoveData moveData)
        {
            var distance = Vector3.Distance(target.position, position);
            var duration = distance / moveData.Speed;
            
            target.DOKill();
            var tween = target.DOMove(position, duration);
            tween.SetEase(moveData.Ease);
        }
    }
}