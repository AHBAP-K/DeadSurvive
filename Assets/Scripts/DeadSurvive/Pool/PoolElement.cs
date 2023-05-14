using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace DeadSurvive.Pool
{
    public class PoolElement : IDisposable
    {
        public string AssetId => _assetReference.AssetGUID;
        
        private readonly AssetReference _assetReference;
        
        private List<int> _spawnedObjects;
        private Queue<GameObject> _pooledObjects;
        private AsyncOperationHandle<GameObject> _addressableOperation;

        public PoolElement(AssetReference assetReference)
        {
            _assetReference = assetReference;
            _spawnedObjects = new List<int>(3);
            _pooledObjects = new Queue<GameObject>(3);
        }

        public async UniTask<GameObject> GetGameObject(Vector3 position = default, Transform parent = null)
        {
            if (_pooledObjects.Count > 0)
            {
                var gameObject = _pooledObjects.Dequeue();
                return gameObject;
            }

            var spawnedObject = await _assetReference.InstantiateAsync(position, Quaternion.identity, parent);

            _spawnedObjects.Add(spawnedObject.GetInstanceID());

            return spawnedObject;
        }

        public bool IsSpawnedObject(int id)
        {
            return _spawnedObjects.Contains(id);
        }

        public void ReturnObject(GameObject gameObject)
        {
            var id = gameObject.GetInstanceID();

            if (!_spawnedObjects.Contains(id))
            {
                return;
            }

            _pooledObjects.Enqueue(gameObject);
        }

        public void Dispose()
        {
            _spawnedObjects = null;

            foreach (var pooledObject in _pooledObjects)
            {
                Object.Destroy(pooledObject);
            }

            _pooledObjects = null;
            
            Addressables.Release(_addressableOperation);
        }
    }
}