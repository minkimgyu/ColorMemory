using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class CustomLevelGenerator : LevelGenerator
{
    CollectArtData.Section _section;

    public CustomLevelGenerator(CollectArtData.Section section)
    {
        _section = section;
    }

    public bool CanGenerateLevelData()
    {
        return true;
    }

    public MapData GenerateLevelData()
    {
        List<CollectArtData.Color> colors = _section.UsedColors[0];

        List<int> pickColorIndexes = new List<int>();
        for (int i = 0; i < colors.Count; i++)
        {
            pickColorIndexes.Add(i);
        }

        int row = _section.Blocks.Count;
        int col = _section.Blocks[0].Count;

        int[,] dotColorIndexes = new int[row, col];

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                int colorIndex = colors.IndexOf(_section.Blocks[i][j].Color);
                dotColorIndexes[i, j] = colorIndex;
            }
        }

        MapData mapData = new MapData(pickColorIndexes, dotColorIndexes);
        return mapData;
    }
}
