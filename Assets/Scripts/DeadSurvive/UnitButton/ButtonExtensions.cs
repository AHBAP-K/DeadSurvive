using Leopotam.EcsLite;

namespace DeadSurvive.UnitButton
{
    public static class ButtonExtensions
    {
        public static void AddButtonComponent(this EcsWorld world, int entity)
        {
            var buttonComponent = world.GetPool<ButtonComponent>();
            ref var buttonTag = ref buttonComponent.Add(entity);
        }
    }
}