using System.Collections.Generic;
using UnityEngine;

public struct MapData
{
    List<int> _pickColors;
    int[,] _dotColors;

    public MapData(List<int> pickColors, int[,] dotColors)
    {
        _pickColors = pickColors;
        _dotColors = dotColors;
    }

    public List<int> PickColors { get => _pickColors; }
    public int[,] DotColor { get => _dotColors; }
}