using System.Collections.Generic;
using DeadSurvive.Common;
using DeadSurvive.Spawner;
using DeadSurvive.TapPosition;
using Leopotam.EcsLite;
using UnityEngine;

namespace DeadSurvive.Level
{
    public class LandView : MonoBehaviour, IEcsWorldReceiver
    {
        public TapOnTrigger TapOnTrigger => _tapOnTrigger;

        public List<SpawnPoints> Points => _spawnPoints;

        public EnemySpawnData EnemySpawnData => _enemySpawnData;

        public BoxCollider2D AreaZone => _areaZone;

        [SerializeField]
        private TapOnTrigger _tapOnTrigger;

        [SerializeField] 
        private BoxCollider2D _areaZone;

        [SerializeField] 
        private EnemySpawnData _enemySpawnData;

        [SerializeField] 
        private List<TransitionView> _transitionViews;

        [SerializeField] 
        private List<SpawnPoints> _spawnPoints;

        public void SetEcsWorld(EcsWorld world)
        {
            TapOnTrigger.SetEcsWorld(world);

            foreach (var transitionView in _transitionViews)
            {
                transitionView.SetEcsWorld(world);
            }
        }
        
        public void ConfigureTransitions(List<DirectionType> allowedTransitions)
        {
            foreach (var transitionView in _transitionViews)
            {
                if (allowedTransitions.Contains(transitionView.Direction))
                {
                    continue;
                }
                
                transitionView.gameObject.SetActive(false);
            }
        }
    }
}