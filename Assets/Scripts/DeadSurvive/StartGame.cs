using System.Collections.Generic;
using DeadSurvive.Attack;
using DeadSurvive.Common;
using DeadSurvive.Common.Data;
using DeadSurvive.Health;
using DeadSurvive.HeroButton;
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

        private void Start()
        {
            _ecsWorld = new EcsWorld();

            _ecsStartSystems = new EcsSystems(_ecsWorld, _gameData);

            for (int i = 0; i < _ecsSystemHolders.Count; i++)
            {
                _ecsSystemHolders[i].EcsSystems = _ecsStartSystems;
            }
            
            _ecsStartSystems.Add(new UnitSpawnSystem());
            _ecsStartSystems.Add(new InitButtonSystem());

            _ecsStartSystems.Init();

            _ecsUpdateSystems = new EcsSystems(_ecsWorld, _gameData);
            
            _ecsUpdateSystems.Add(new MovementSystem());
            _ecsUpdateSystems.Add(new ZoneDetectSystem());
            _ecsUpdateSystems.Add(new AttackSystem());
            _ecsUpdateSystems.Add(new HealthSystem());
            _ecsUpdateSystems.Add(new SelfMovementSystem());
            
            _ecsUpdateSystems.Init();
        }

        private void Update()
        {
            _ecsUpdateSystems.Run();
        }
    }
}