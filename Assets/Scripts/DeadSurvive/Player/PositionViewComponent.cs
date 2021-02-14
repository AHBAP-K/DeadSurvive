using Svelto.ECS;
using Svelto.ECS.Hybrid;

namespace DeadSurvive.Player
{
    public struct PositionViewComponent : IEntityViewComponent
    {
        public IPositionComponent PositionComponent;
        public EGID ID { get; set; }
    }
}
