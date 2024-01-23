using Cysharp.Threading.Tasks;
using DeadSurvive.Common.Data;
using DeadSurvive.Level;
using DeadSurvive.Spawner;
using DeadSurvive.Unit.Enum;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DeadSurvive.UnitButton
{
    public class UnitSpawnButtonSpawner : IEcsInitSystem
    {
        private readonly EcsWorldInject _ecsWorld = default;
        
        private readonly EcsSharedInject<GameData> _gameData = default;

        private readonly EcsFilterInject<Inc<LandComponent>> _landFilter = default;

        private readonly EcsPoolInject<SpawnComponent> _spawnPool = default;
        private readonly EcsPoolInject<ButtonComponent> _buttonPool = default;
        private readonly EcsPoolInject<LandComponent> _landComponent = default;

        public void Init(IEcsSystems systems)
        {
            SpawnButton(_gameData.Value.ButtonPrefab, _gameData.Value.ButtonSpawnPoint).Forget();
        }

        private async UniTaskVoid SpawnButton(AssetReference buttonPrefab, Transform parent)
        {
            void OnClick()
            {
                var spawnEntity = _ecsWorld.Value.NewEntity();
                var unitData = _gameData.Value.GetUnitData(UnitType.Hero);

                ref var spawnComponent = ref _spawnPool.Value.Add(spawnEntity);

                foreach (var entity in _landFilter.Value)
                {
                    ref var landComponent = ref _landComponent.Value.Get(entity);
                    var random = Random.Range(0, landComponent.LandView.Points.Count);
                    
                    spawnComponent.Setup(unitData, landComponent.LandView.Points[random].Point.position);
                    
                    break;
                }
            }
            
            var buttonObject = await _gameData.Value.Pool.SpawnObject(buttonPrefab, parent:parent);
            var buttonUnityBehaviour = buttonObject.GetComponent<ButtonView>();
            var entity = _ecsWorld.Value.NewEntity();
            
            AddComponent(entity, buttonUnityBehaviour);

            buttonUnityBehaviour.Text.text = $"Spawn Uint";
            buttonUnityBehaviour.Button.onClick.RemoveAllListeners();
            buttonUnityBehaviour.Button.onClick.AddListener(OnClick);
        }
        
        private void AddComponent(int entity, ButtonView buttonUnityBehaviour)
        {
            ref var buttonComponent = ref _buttonPool.Value.Add(entity);
            buttonComponent.ButtonView = buttonUnityBehaviour;
        }
    }
}