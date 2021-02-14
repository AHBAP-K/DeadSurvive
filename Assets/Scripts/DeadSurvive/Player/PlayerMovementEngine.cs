using System.Collections;
using Svelto.ECS;
using Svelto.Tasks;

namespace DeadSurvive.Player
{
    public class PlayerMovementEngine : IQueryingEntitiesEngine
    {
        public EntitiesDB entitiesDB { get; set; }
        public void Ready()
        {
            MovePlayer().RunOnScheduler(StandardSchedulers.physicScheduler);
        }

        private IEnumerator MovePlayer()
        {
            while (true)
            {
                PlayerMove();
                yield return null;
            }
        }

        private void PlayerMove()
        {
            var (inputs, players, count) = entitiesDB.QueryEntities<InputComponent, PositionViewComponent>(ECSGroups.PlayersGroup);

            for (int i = 0; i < count; i++)
            {
                players[i].PositionComponent.position += inputs[i].position;
            }
        }
    }
}