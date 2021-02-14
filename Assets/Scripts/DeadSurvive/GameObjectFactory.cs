using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DeadSurvive
{
    public class GameObjectFactory
    {
        public IEnumerator<GameObject> Build(string prefabName)
        {
            var operation =  Addressables.InstantiateAsync(prefabName);

            while (operation.IsDone == false) yield return null;

            Debug.Log($"Game object is instantiated {operation.Result.name}");

            yield return operation.Result;
        }
    }
}