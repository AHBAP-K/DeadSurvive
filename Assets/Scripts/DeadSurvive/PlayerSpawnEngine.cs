using System.Collections;
using System.Collections.Generic;
using DeadSurvive.Player;
using Svelto.ECS;
using Svelto.ECS.Hybrid;
using UnityEngine;

namespace DeadSurvive
{
    public class PlayerSpawnEngine : IQueryingEntitiesEngine
    {
        public EntitiesDB entitiesDB { get; set; }

        private readonly GameObjectFactory _gameObjectFactory;
        private readonly IEntityFactory _entityFactory;

        public PlayerSpawnEngine(GameObjectFactory gameObjectFactory, IEntityFactory entityFactory)
        {
            _gameObjectFactory = gameObjectFactory;
            _entityFactory = entityFactory;
        }

        public void Ready()
        {
            SpawnPlayer().Run();
        }

        private IEnumerator SpawnPlayer()
        {
            var playerLoader = _gameObjectFactory.Build("Player");

            yield return playerLoader;

            var player = playerLoader.Current;

            if (player == null)
            {
                Debug.Log("Player not found");
                yield break; 
            }

            IImplementor playerPositionImplementor = player.AddComponent<PositionImplementor>();
            var implementors = new List<IImplementor> {playerPositionImplementor};

            Debug.Log("PlayerSpawnerEngine");
            
            _entityFactory.BuildEntity<PlayerEntityDescriptor>(0, ECSGroups.PlayersGroup, implementors);
        }
    }
}