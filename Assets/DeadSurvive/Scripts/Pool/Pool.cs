using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DeadSurvive.Pool
{
    public class Pool 
    {
        private readonly List<PoolElement> _poolElements = new List<PoolElement>(5);

        private readonly GameObject _poolParent;

        public Pool()
        {
            _poolParent = new GameObject("Pool");
            _poolParent.SetActive(false);
        }

        public async UniTask<GameObject> SpawnObject(AssetReference assetReference, Vector3 position = default, Transform parent = null)
        {
            var poolElement = GetPoolElement(assetReference);

            return await poolElement.GetGameObject(position, parent);
        }

        public void ReturnObject(GameObject gameObject)
        {
            var id = gameObject.GetInstanceID();

            foreach (var poolElement in _poolElements)
            {
                if (poolElement.IsSpawnedObject(id))
                {
                    poolElement.ReturnObject(gameObject);
                    gameObject.transform.SetParent(_poolParent.transform);
                }
            }
        }

        private PoolElement GetPoolElement(AssetReference assetReference)
        {
            foreach (var poolElement in _poolElements)
            {
                if (poolElement.AssetId.Equals(assetReference.AssetGUID))
                {
                    return poolElement;
                }
            }

            var newPoolElement = new PoolElement(assetReference);

            _poolElements.Add(newPoolElement);

            return newPoolElement;
        }

    }
}