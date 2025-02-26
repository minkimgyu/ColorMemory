using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class RandomLevelGenerator : LevelGenerator
{
    int _pickCount;
    Vector2Int _levelSize = new Vector2Int(5, 5); // row, col
    Vector2Int[] _closePoints;

    int _pickColorSize;

    public RandomLevelGenerator(int pickColorSize, int pickCount, Vector2Int levelSize)
    {
        _pickColorSize = pickColorSize;
        _pickCount = pickCount;
        _levelSize = levelSize;

        _closePoints = new Vector2Int[4]
        {
            new Vector2Int(-1, 0), // ↑
            new Vector2Int(0, 1), // →
            new Vector2Int(1, 0), // ↓
            new Vector2Int(0, -1), // ←
            //new Vector2Int(-1, 1), // ↗
            //new Vector2Int(1, 1), // ↘
            //new Vector2Int(1, -1), // ↙
            //new Vector2Int(-1, -1), // ↖
        };
    }

    List<int> GetRandomColors()
    {
        List<int> randomColorIndexes = new List<int>();

        while (randomColorIndexes.Count < _pickCount) 
        {
            int randomIndex = Random.Range(0, _pickColorSize);
            if (randomColorIndexes.Contains(randomIndex)) continue;

            randomColorIndexes.Add(randomIndex);
        }

        return randomColorIndexes;
    }

    List<Vector2Int> GetRandomPoints()
    {
        List<Vector2Int> randomPoints = new List<Vector2Int>();

        while (randomPoints.Count < _pickCount)
        {
            int x = Random.Range(0, _levelSize.x);
            int y = Random.Range(0, _levelSize.y);

            Vector2Int index = new Vector2Int(x, y);
            if (randomPoints.Contains(index)) continue;

            randomPoints.Add(index);
        }

        return randomPoints;
    }

    bool OutOfRange(Vector2Int point)
    {
        return point.x < 0 || point.y < 0 || point.x >= _levelSize.x || point.y >= _levelSize.y;
    }

    List<Vector2Int> GetNearPoints(Vector2Int point)
    {
        List<Vector2Int> nearPoints = new List<Vector2Int>();

        for (int i = 0; i < _closePoints.Length; i++)
        {
            Vector2Int nearPoint = point + _closePoints[i];

            if (OutOfRange(nearPoint)) continue;
            nearPoints.Add(nearPoint);
        }

        return nearPoints;
    }

    public override bool CanGenerateLevelData()
    {
        if (_pickColorSize < _pickCount) return false;
        return true;
    }

    public override MapData GenerateLevelData()
    {
        int[,] levelMap = new int[_levelSize.x, _levelSize.y];
        for (int x = 0; x < _levelSize.x; x++)
        {
            for (int y = 0; y < _levelSize.y; y++)
            {
                levelMap[x, y] = -1;
            }
        }

        List<Vector2Int> randomPoints = GetRandomPoints();
        List<int> randomColors = GetRandomColors();

        Queue<Tuple<Vector2Int, int>> queue = new Queue<Tuple<Vector2Int, int>>();

        for (int i = 0; i < randomPoints.Count; i++)
        {
            Tuple<Vector2Int, int> tuple = new Tuple<Vector2Int, int>(randomPoints[i], randomColors[i]);

            levelMap[randomPoints[i].x, randomPoints[i].y] = randomColors[i];
            queue.Enqueue(tuple);
        }

        while (queue.Count > 0) // bfs를 통한 배열 채우기
        {
            Tuple<Vector2Int, int> front = queue.Dequeue();

            List<Vector2Int> nearPoints = GetNearPoints(front.Item1);
            for (int i = 0; i < nearPoints.Count; i++)
            {
                if (levelMap[nearPoints[i].x, nearPoints[i].y] >= 0) continue;

                levelMap[nearPoints[i].x, nearPoints[i].y] = front.Item2;

                Tuple<Vector2Int, int> tuple = new Tuple<Vector2Int, int>(nearPoints[i], front.Item2);
                queue.Enqueue(tuple);
            }
        }

        MapData levelData = new MapData(randomColors, levelMap);
        return levelData;
    }
}
