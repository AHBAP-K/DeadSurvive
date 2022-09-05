using DeadSurvive.Common.Data;
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
            var unitComponentFilter = world.Filter<ButtonTag>().End();
            var buttonPoll = world.GetPool<ButtonComponent>();

            foreach (var entity in unitComponentFilter)
            {
                var button = Object.Instantiate(data.ButtonPrefab, data.ButtonSpawnPoint);
                var buttonUnityBehaviour = button.GetComponent<Button>();
                
                buttonPoll.Add(entity);
                
                buttonUnityBehaviour.onClick.AddListener(() =>
                {
                    DisableAll();
                    
                    ref var buttonComponent = ref buttonPoll.Get(entity);
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