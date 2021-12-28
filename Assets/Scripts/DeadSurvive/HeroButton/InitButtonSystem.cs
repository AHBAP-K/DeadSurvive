using DeadSurvive.Common.Data;
using DeadSurvive.Moving;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.UI;

namespace DeadSurvive.HeroButton
{
    public class InitButtonSystem : IEcsInitSystem
    {
        private EcsSystems _ecsSystems;
        
        public void Init(EcsSystems systems)
        {
            _ecsSystems = systems;
            
            var world = systems.GetWorld();
            var data = systems.GetShared<GameData>();
            var moveComponentFilter = world.Filter<MoveComponent>().End();
            var buttonPoll = world.GetPool<ButtonComponent>();

            foreach (var moveEntity in moveComponentFilter)
            {
                var button = Object.Instantiate(data.ButtonPrefab, data.ButtonSpawnPoint);
                var buttonUnityBehaviour = button.GetComponent<Button>();
                
                buttonPoll.Add(moveEntity);
                
                buttonUnityBehaviour.onClick.AddListener(() =>
                {
                    DisableAll();
                    
                    ref var buttonComponent = ref buttonPoll.Get(moveEntity);
                    buttonComponent.IsSelected = true;
                });
            }
        }


        private void DisableAll()
        {
            var buttonPoll = _ecsSystems.GetWorld().GetPool<ButtonComponent>();
            var filter = _ecsSystems.GetWorld().Filter<ButtonComponent>().End();
            
            foreach (var entity in filter)
            {
                ref var buttonComponent = ref buttonPoll.Get(entity);
                buttonComponent.IsSelected = false;
            }
        }
    }
}