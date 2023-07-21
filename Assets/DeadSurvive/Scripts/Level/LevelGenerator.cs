using System;
using System.Collections.Generic;
using System.Text;
using DeadSurvive.Common;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DeadSurvive.Level
{
    public class LevelGenerator
    {
        private readonly int _levelWidth;
        private readonly int _levelHeight;

        private bool[,] _visited;
        private PointType[,] _level;
        private Vector2Int _lastPoint;

        public LevelGenerator(Vector2Int landSize)
        {
            _levelHeight = landSize.y;
            _levelWidth = landSize.x;
        }

        public PointType[,] GetLevel()
        {
            GenerateLevel();

            _level[_lastPoint.y, _lastPoint.x] = PointType.EndPoint;
            
            PrintLevel();
            
            return _level;
        }

        private void GenerateLevel()
        {
            _level = new PointType[_levelHeight, _levelWidth];
            _visited = new bool[_levelHeight, _levelWidth];

            var startX = Random.Range(0, _levelWidth);
            var startY = Random.Range(0, _levelHeight);
            
            _level[startY, startX] = PointType.StartPoint;

            GenerateMaze(startX, startY);
        }

        private void GenerateMaze(int x, int y)
        {
            _visited[y, x] = true;
            _lastPoint = new Vector2Int(x, y);

            var directions = GetShuffledDirections();

            foreach (var direction in directions)
            {
                var newX = x + direction.x;
                var newY = y + direction.y;

                if (!IsInBounds(newX, newY) || _visited[newY, newX])
                {
                    continue;
                }

                _level[newY, newX] = PointType.Point;
                _level[(y + newY) / 2, (x + newX) / 2] = PointType.Point;

                GenerateMaze(newX, newY);
            }
        }

        private bool IsInBounds(int x, int y)
        {
            return x >= 0 && x < _levelWidth && y >= 0 && y < _levelHeight;
        }

        private List<Vector2Int> GetShuffledDirections()
        {
            var directions = new List<Vector2Int>
            {
                new(0, -2), // North
                new(2, 0), // East
                new(0, 2), // South
                new(-2, 0) // West
            };

            directions.Shuffle();
            
            return directions;
        }

        private void PrintLevel()
        {
            var landLog = new StringBuilder();
            
            for (var y = 0; y < _levelHeight; y++)
            {
                for (var x = 0; x < _levelWidth; x++)
                {
                    var cell = _level[y, x];
                    landLog.Append((int)cell);
                }

                landLog.Append("\n");
            }
            
            Debug.Log(landLog.ToString());
        }
    }
}