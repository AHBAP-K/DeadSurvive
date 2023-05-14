using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DeadSurvive.Attack;
using DeadSurvive.Common;
using DeadSurvive.Common.Data;
using DeadSurvive.Death;
using DeadSurvive.Health;
using DeadSurvive.Moving;
using DeadSurvive.Spawner;
using DeadSurvive.UnitButton;
using DeadSurvive.ZoneDetect;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
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
            _ecsWorld = new EcsWorld();
            
            _gameData.SetupPool();
            
            _ecsStartSystems = new EcsSystems(_ecsWorld, _gameData);

            for (int i = 0; i < _ecsSystemHolders.Count; i++)
            {
                _ecsSystemHolders[i].EcsSystems = _ecsStartSystems;
            }
            
            _ecsStartSystems.Add(new UnitSpawnButtonSpawner());
            _ecsStartSystems.Add(new UnitSpawnSystem());
            
            _ecsUpdateSystems = new EcsSystems(_ecsWorld, _gameData);
            
            _ecsUpdateSystems.Add(new UnitSpawnSystem());
            _ecsUpdateSystems.Add(new EnemySpawnSystem());
            _ecsUpdateSystems.Add(new MovementSystem());
            _ecsUpdateSystems.Add(new ZoneDetectSystem());
            _ecsUpdateSystems.Add(new AttackStartSystem());
            _ecsUpdateSystems.Add(new AttackSystem());
            _ecsUpdateSystems.Add(new HealthSystem());
            _ecsUpdateSystems.Add(new SelfMovementSystem());
            _ecsUpdateSystems.Add(new DeathSystem());

            _ecsStartSystems.Inject();
            _ecsUpdateSystems.Inject();
            
            _ecsStartSystems.Init();
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