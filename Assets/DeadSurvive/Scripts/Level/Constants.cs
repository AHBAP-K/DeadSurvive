using System.Collections.Generic;
using UnityEngine;

namespace DeadSurvive.Level
{
    public static class Constants
    {
        public static readonly Dictionary<DirectionType, Vector2Int> Directions = new()
        {
            { DirectionType.North, new Vector2Int(0, -1) },
            { DirectionType.East, new Vector2Int(1, 0) },
            { DirectionType.South, new Vector2Int(0, 1) },
            { DirectionType.West, new Vector2Int(-1, 0) },
        };
    }
}