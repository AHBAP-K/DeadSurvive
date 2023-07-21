using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DeadSurvive.Level
{
    public class LandHolder
    {
        public Vector2Int LevelPoint { get; }
        
        public AssetReference LevelReference { get; }
        
        public PointType PointType { get; }
        
        public List<DirectionType> Transitions { get; }

        public LandHolder(Vector2Int levelPoint, AssetReference levelReference, PointType pointType, List<DirectionType> transitions)
        {
            LevelPoint = levelPoint;
            LevelReference = levelReference;
            PointType = pointType;
            Transitions = transitions;
        }
    }
}