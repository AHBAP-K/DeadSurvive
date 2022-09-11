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
            var buttonFilter = world.Filter<ButtonComponent>().End();

            foreach (var entity in buttonFilter)
            {
                var button = Object.Instantiate(data.ButtonPrefab, data.ButtonSpawnPoint);
                var buttonUnityBehaviour = button.GetComponent<Button>();
                
                buttonUnityBehaviour.onClick.AddListener(() =>
                {
                    SetSelected(entity);
                });
            }
        }

        private void SetSelected(int targetEntity)
        {
            var buttonPoll = _ecsSystems.GetWorld().GetPool<ButtonComponent>();
            var filter = _ecsSystems.GetWorld().Filter<ButtonComponent>().End();
            
            foreach (var entity in filter)
            {
                ref var buttonComponent = ref buttonPoll.Get(entity);
                buttonComponent.IsSelected = targetEntity == entity;
            }
        }
    }
}