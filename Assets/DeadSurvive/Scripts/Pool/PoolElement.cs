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
        
        private List<GameObject> _spawnedObjects;
        private Queue<GameObject> _pooledObjects;
        private AsyncOperationHandle<GameObject> _addressableOperation;
        private UniTaskCompletionSource _loadSource;

        public PoolElement(AssetReference assetReference)
        {
            _assetReference = assetReference;
            _spawnedObjects = new List<GameObject>(3);
            _pooledObjects = new Queue<GameObject>(3);
        }

        public async UniTask<GameObject> GetGameObject(Vector3 position = default, Transform parent = null)
        {
            if (_pooledObjects.Count > 0)
            {
                var gameObject = _pooledObjects.Dequeue();
                
                gameObject.transform.SetParent(parent, true);
                gameObject.transform.position = position;         
                
                return gameObject;
            }

            if (_loadSource != null)
            {
                await _loadSource.Task;
            }
            else if (!_addressableOperation.IsValid())
            {
                await LoadAddressable();
            }

            var spawnedObject = Object.Instantiate(_addressableOperation.Result, position, Quaternion.identity, parent);
            
            _spawnedObjects.Add(spawnedObject);

            return spawnedObject;
        }

        private async UniTask LoadAddressable()
        {
            _addressableOperation = Addressables.LoadAssetAsync<GameObject>(_assetReference);
            _loadSource = new UniTaskCompletionSource();
                
            await _addressableOperation;
                
            _loadSource.TrySetResult();
            _loadSource = null;
        }

        public bool IsSpawnedObject(GameObject gameObject)
        {
            return _spawnedObjects.Contains(gameObject);
        }

        public void ReturnObject(GameObject gameObject)
        {
            if (!_spawnedObjects.Contains(gameObject))
            {
                return;
            }

            _pooledObjects.Enqueue(gameObject);
        }

        public void Dispose()
        {
            foreach (var gameObject in _spawnedObjects)
            {
                Object.Destroy(gameObject);
            }

            _spawnedObjects = null;
            _pooledObjects = null;
            
            Addressables.Release(_addressableOperation);
        }
    }
}