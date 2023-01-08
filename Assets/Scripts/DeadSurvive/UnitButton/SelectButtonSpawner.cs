using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DeadSurvive.UnitButton
{
    public class SelectButtonSpawner
    {
        private EcsWorld _ecsWorld;
        private Pool.Pool _pool;

        public void Setup(EcsWorld ecsWorld, Pool.Pool pool)
        {
            _ecsWorld = ecsWorld;
            _pool = pool;
        }
        
        public async UniTask SpawnButton(int entity, AssetReference buttonPrefab, Transform parent)
        {
            void SetSelected(int targetEntity)
            {
                var buttonPoll =  _ecsWorld.GetPool<ButtonComponent>();
                var filter = _ecsWorld.Filter<ButtonComponent>().End();
            
                foreach (var entityButton in filter)
                {
                    ref var buttonComponent = ref buttonPoll.Get(entityButton);
                    buttonComponent.IsSelected = targetEntity == entityButton;
                }
            }
            
            var buttonObject = await _pool.SpawnObject(assetReference: buttonPrefab, parent:parent);
            var buttonUnityBehaviour = buttonObject.GetComponent<ButtonView>();

            AddComponent(entity, buttonUnityBehaviour);

            buttonUnityBehaviour.Text.text = $"Unit {entity}";
            buttonUnityBehaviour.Button.onClick.RemoveAllListeners();
            buttonUnityBehaviour.Button.onClick.AddListener(() =>
            {
                SetSelected(entity);
            });
        }
        
        private void AddComponent(int entity, ButtonView buttonUnityBehaviour)
        {
            var buttonPool = _ecsWorld.GetPool<ButtonComponent>();
            ref var buttonComponent = ref buttonPool.Add(entity);
            buttonComponent.ButtonView = buttonUnityBehaviour;
        }
    }
}