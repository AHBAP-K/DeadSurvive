using Cysharp.Threading.Tasks;
using DeadSurvive.Common.Data;
using DeadSurvive.Spawner;
using DeadSurvive.Unit.Enum;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DeadSurvive.UnitButton
{
    public class UnitSpawnButtonSpawner : IEcsInitSystem
    {
        private EcsWorld _ecsWorld;
        private GameData _gameData;
        
        public void Init(IEcsSystems systems)
        {
            _ecsWorld = systems.GetWorld();
            _gameData = systems.GetShared<GameData>();
            
            SpawnButton(_gameData.ButtonPrefab, _gameData.ButtonSpawnPoint).Forget();
        }

        private async UniTaskVoid SpawnButton(AssetReference buttonPrefab, Transform parent)
        {
            void OnClick()
            {
                var spawnPool = _ecsWorld.GetPool<SpawnComponent>();
                var spawnEntity = _ecsWorld.NewEntity();
                var unitData = _gameData.GetUnitData(UnitType.Hero);
                var position = _gameData.GetUnitSpawnData(UnitType.Hero);

                ref var spawnComponent = ref spawnPool.Add(spawnEntity);
                spawnComponent.Setup(unitData, position.GetTargetPosition());
            }
            
            var buttonObject = await _gameData.Pool.SpawnObject(buttonPrefab, parent:parent);
            var buttonUnityBehaviour = buttonObject.GetComponent<ButtonView>();
            var entity = _ecsWorld.NewEntity();
            
            AddComponent(entity, buttonUnityBehaviour);

            buttonUnityBehaviour.Text.text = $"Spawn Uint";
            buttonUnityBehaviour.Button.onClick.RemoveAllListeners();
            buttonUnityBehaviour.Button.onClick.AddListener(OnClick);
        }
        
        private void AddComponent(int entity, ButtonView buttonUnityBehaviour)
        {
            var buttonPool = _ecsWorld.GetPool<ButtonComponent>();
            ref var buttonComponent = ref buttonPool.Add(entity);
            buttonComponent.ButtonView = buttonUnityBehaviour;
        }
    }
}