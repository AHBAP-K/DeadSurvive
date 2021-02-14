using Svelto.ECS;

namespace DeadSurvive.Player
{
    public class PlayerEntityDescriptor : IEntityDescriptor
    {
        public IComponentBuilder[] componentsToBuild { get; } =
        {
            new ComponentBuilder<InputComponent>(),
            new ComponentBuilder<PositionViewComponent>()
        };

    }
}