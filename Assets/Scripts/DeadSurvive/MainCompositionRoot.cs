using DeadSurvive.Player;
using Svelto.Context;
using Svelto.ECS;
using Svelto.ECS.Schedulers.Unity;
using Svelto.Tasks;

namespace DeadSurvive
{
    public class MainCompositionRoot : ICompositionRoot
    {
        private EnginesRoot _enginesRoot;
        private string _nameScheduler = "Engine";

        public void OnContextInitialized<T>(T contextHolder)
        {
            CompositionRoot();
        }

        public void OnContextDestroyed()
        {
            _enginesRoot.Dispose();
            TaskRunner.StopAndCleanupAllDefaultSchedulers();
        }

        public void OnContextCreated<T>(T contextHolder)
        {
            
        }

        private void CompositionRoot()
        {
            _enginesRoot = new EnginesRoot(new UnityEntitiesSubmissionScheduler(_nameScheduler));

            var entityFactory = _enginesRoot.GenerateEntityFactory();
            var gameObjectFactory = new GameObjectFactory();

            var playerSpawnEngine = new PlayerSpawnEngine(gameObjectFactory, entityFactory);
            var playerInputEngine = new PlayerInputEngine();
            var playerMoveEngine = new PlayerMovementEngine();
            
            _enginesRoot.AddEngine(playerSpawnEngine);
            _enginesRoot.AddEngine(playerInputEngine);
            _enginesRoot.AddEngine(playerMoveEngine);
        }
    }
}