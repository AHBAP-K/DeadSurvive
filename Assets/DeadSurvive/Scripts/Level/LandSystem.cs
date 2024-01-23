using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DeadSurvive.Common.Data;
using DeadSurvive.Spawner;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DeadSurvive.Level
{
    public class LandSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsSharedInject<GameData> _gameData = default;
        
        private readonly EcsFilterInject<Inc<TransitionComponent>> _transitionFilter = default;
        
        private readonly EcsPoolInject<TransitionComponent> _transitionPool = default;
        private readonly EcsPoolInject<LandComponent> _landPool = default;
        private readonly EcsPoolInject<EnemySpawnComponent> _enemySpawnPool = default;

        private int _currentLandEntity;
        private LandHolder _currentLand;
        private LandHolder[,] _landHolders;
        private LandConfig _landConfig;
        private Pool.Pool _pool;

        public void Init(IEcsSystems systems)
        {
            FillLandHolder();
            SpawnStartPoint().Forget();
        }

        private void FillLandHolder()
        {
            _landConfig = _gameData.Value.LandConfig;
            _pool = _gameData.Value.Pool;
            
            var levelGenerator = new LevelGenerator(_landConfig.LandSize);
            var landPositions = levelGenerator.GetLevel();
            
            _landHolders = new LandHolder[_landConfig.LandSize.y, _landConfig.LandSize.x];

            for (var i = 0; i < landPositions.GetLength(0); i++)
            {
                for (var j = 0; j < landPositions.GetLength(1); j++)
                {
                    var pointType = landPositions[i, j];

                    if (pointType == PointType.None)
                    {
                        continue;
                    }

                    var levelPosition = new Vector2Int(j, i);
                    var transitions = GetPointTransitions(levelPosition, _landConfig.LandSize, landPositions);
                    var land = new LandHolder(levelPosition, _landConfig.GetRandomLand(), pointType, transitions);
                    
                    _landHolders[i, j] = land;
                }
            }
        }

        private List<DirectionType> GetPointTransitions(Vector2Int targetPoint, Vector2Int areaSize, PointType[,] allPoint)
        {
            var allowedTransitions = new List<DirectionType>();
            
            foreach (var direction in Constants.Directions)
            {
                var newDirection = direction.Value + targetPoint;
                
                if (!IsCanCross(newDirection, areaSize, allPoint))
                {
                    continue;
                }

                allowedTransitions.Add(direction.Key);
            }

            return allowedTransitions;
        }
        
        private bool IsCanCross(Vector2Int point, Vector2Int areaSize, PointType[,] allPoint)
        {
            return point.x >= 0 && point.x < areaSize.x && point.y >= 0 && point.y < areaSize.y && allPoint[point.y, point.x] != PointType.None;
        }

        private async UniTask SpawnStartPoint()
        {
            foreach (var landHolder in _landHolders)
            {
                if (landHolder == null || landHolder.PointType != PointType.StartPoint)
                {
                    continue;
                }

                _currentLand = landHolder;

                var landObject = await _pool.SpawnObject(landHolder.LevelReference);
                var landView = landObject.GetComponent<LandView>();
                
                landView.ConfigureTransitions(landHolder.Transitions);
                landView.SetEcsWorld(_world.Value);
                
                CreateLandComponent(landView);
            }
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _transitionFilter.Value)
            {
                ref var transitionComponent = ref _transitionPool.Value.Get(entity);
                var direction = Constants.Directions[transitionComponent.Direction];
                var targetPoint = direction + _currentLand.LevelPoint;

                LoadNextLevel(_landHolders[targetPoint.y, targetPoint.x]).Forget();
                
                _transitionPool.Value.Del(entity);
            }
        }

        private async UniTask LoadNextLevel(LandHolder landHolder)
        {
            Debug.Log($"[{nameof(LandSystem)} LoadNextLevel {landHolder.LevelPoint.ToString()}]");

            UnloadCurrentLevel();
            
            _currentLand = landHolder;
            
            var landObject = await _pool.SpawnObject(_currentLand.LevelReference);
            var landView = landObject.GetComponent<LandView>();
            
            landView.ConfigureTransitions(landHolder.Transitions);
            landView.SetEcsWorld(_world.Value);

            CreateLandComponent(landView);
        }
        
        private void UnloadCurrentLevel()
        {
            if (_currentLandEntity <= 0)
            {
                return;
            }
            
            _world.Value.DelEntity(_currentLandEntity);
            _pool.DisposeObjects(_currentLand.LevelReference);
        }

        private void CreateLandComponent(LandView landView)
        {           
            _currentLandEntity = _world.Value.NewEntity();

            ref var landComponent = ref _landPool.Value.Add(_currentLandEntity);
            ref var enemySpawnComponent = ref _enemySpawnPool.Value.Add(_currentLandEntity);
            
            landComponent.Setup(landView);
            
            enemySpawnComponent.SetupEnemyData(landView.EnemySpawnData);
            enemySpawnComponent.SetupPositions(landView.Points);
        }
    }
}