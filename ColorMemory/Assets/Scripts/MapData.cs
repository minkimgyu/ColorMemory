using System.Collections.Generic;
using UnityEngine;

public struct MapData
{
    List<int> _pickColorIndexes;
    int[,] _dotColorIndexes;

    public MapData(List<int> pickColorIndexes, int[,] dotColorIndexes)
    {
        _pickColorIndexes = pickColorIndexes;
        _dotColorIndexes = dotColorIndexes;
    }

    public List<int> PickColors { get => _pickColorIndexes; }
    public int[,] DotColor { get => _dotColorIndexes; }
}