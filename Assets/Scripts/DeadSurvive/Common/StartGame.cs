using System.Collections.Generic;
using DeadSurvive.Common.Data;
using DeadSurvive.HeroButton;
using DeadSurvive.Heroes;
using DeadSurvive.Moving;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using UnityEngine;
using Voody.UniLeo.Lite;

namespace DeadSurvive.Common
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
            
            _ecsStartSystems.Add(new HeroesInitSystem());
            _ecsStartSystems.Add(new InitButtonSystem());

            _ecsStartSystems.Init();

            _ecsUpdateSystems = new EcsSystems(_ecsWorld);
            _ecsUpdateSystems.ConvertScene();
            _ecsUpdateSystems.Add(new MoveSystem());
            
            _ecsUpdateSystems.Init();
        }

        private void Update()
        {
            _ecsUpdateSystems.Run();
        }
    }
}