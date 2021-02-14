using System.Collections;
using Svelto.ECS;
using Svelto.Tasks;
using UnityEngine;

namespace DeadSurvive.Player
{
    public class PlayerInputEngine : IQueryingEntitiesEngine
    {
        public EntitiesDB entitiesDB { get; set; }
        public void Ready()
        {
            ReadInput().RunOnScheduler(StandardSchedulers.earlyScheduler);
        }

        private IEnumerator ReadInput()
        {
            void Iterate()
            {
                var horizontal = Input.GetAxisRaw("Horizontal");
                var vertical = Input.GetAxisRaw("Vertical");

                var (inputComponents, count) = entitiesDB.QueryEntities<InputComponent>(ECSGroups.PlayersGroup);

                for (int i = 0; i < count; i++)
                {
                    inputComponents[i].position = new Vector2(horizontal, vertical);
                }
            }

            while (true)
            {
                Iterate();
                yield return null;
            }
        }
    }
}