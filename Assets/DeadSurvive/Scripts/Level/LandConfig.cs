using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DeadSurvive.Level
{
    [CreateAssetMenu(fileName = "LandConfig", menuName = "DeadSurvive/Land", order = 0)]
    public class LandConfig : ScriptableObject
    {
        public List<AssetReference> LandPrefabs => _landPrefabs;

        public Vector2Int LandSize => _landSize;

        [SerializeField] 
        private List<AssetReference> _landPrefabs;

        [SerializeField] 
        private Vector2Int _landSize;

        public AssetReference GetRandomLand()
        {
            return _landPrefabs[Random.Range(0, _landPrefabs.Count - 1)];
        }
    }
}