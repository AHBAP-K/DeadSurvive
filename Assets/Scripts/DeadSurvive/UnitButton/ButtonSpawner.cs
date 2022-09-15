using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace DeadSurvive.UnitButton
{
    public class ButtonSpawner
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
                var buttonPoll = _ecsWorld.GetPool<ButtonComponent>();
                var filter = _ecsWorld.Filter<ButtonComponent>().End();
            
                foreach (var entityButton in filter)
                {
                    ref var buttonComponent = ref buttonPoll.Get(entityButton);
                    buttonComponent.IsSelected = targetEntity == entityButton;
                }
            }
            
            var button = await _pool.SpawnObject(assetReference: buttonPrefab, parent:parent);
            var buttonUnityBehaviour = button.GetComponent<Button>();
            
            buttonUnityBehaviour.onClick.RemoveAllListeners();
            buttonUnityBehaviour.onClick.AddListener(() =>
            {
                SetSelected(entity);
            });
        }
    }
}