using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DeadSurvive.Attack;
using DeadSurvive.Common;
using DeadSurvive.Common.Data;
using DeadSurvive.Health;
using DeadSurvive.UnitButton;
using DeadSurvive.Moving;
using DeadSurvive.Spawner;
using DeadSurvive.ZoneDetect;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DeadSurvive
{
    public class StartGame : SerializedMonoBehaviour
    {
        [SerializeField]
        private GameData _gameData;

        [SerializeField] 
        private List<IEcsSystemHolder> _ecsSystemHolders;

        private EcsWorld _ecsWorld;
        private EcsSystems _ecsStartSystems;
        private EcsSystems _ecsUpdateSystems;

        private CancellationTokenSource _cancellationTokenSource;

        private void Start()
        {
            StartAsync().Forget();
        }

        private async UniTaskVoid StartAsync()
        {
            _ecsWorld = new EcsWorld();
            
            _gameData.SetupPool();
            
            _ecsStartSystems = new EcsSystems(_ecsWorld, _gameData);

            for (int i = 0; i < _ecsSystemHolders.Count; i++)
            {
                _ecsSystemHolders[i].EcsSystems = _ecsStartSystems;
            }
            
            _ecsStartSystems.Add(new UnitSpawnSystem());
            
            _ecsStartSystems.Init();

            _ecsUpdateSystems = new EcsSystems(_ecsWorld, _gameData);
            
            _ecsUpdateSystems.Add(new MovementSystem());
            _ecsUpdateSystems.Add(new ZoneDetectSystem());
            _ecsUpdateSystems.Add(new AttackSystem());
            _ecsUpdateSystems.Add(new HealthSystem());
            _ecsUpdateSystems.Add(new SelfMovementSystem());
            
            _ecsUpdateSystems.Init();

            _cancellationTokenSource = new CancellationTokenSource();
            
            UpdateSystems(_cancellationTokenSource.Token).Forget();
        }
        
        private async UniTaskVoid UpdateSystems(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                _ecsUpdateSystems.Run();
                await UniTask.DelayFrame(1, PlayerLoopTiming.Update, token);
            }
        }

        private void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}