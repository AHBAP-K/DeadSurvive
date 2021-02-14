using Svelto.ECS;

namespace DeadSurvive
{
    public static class ECSGroups
    {
        public static readonly ExclusiveGroup PlayersGroup = new ExclusiveGroup();
    }
}