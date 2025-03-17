using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public struct ArtData
{
    public struct ArtSize
    {
        [JsonProperty("height")] private int _height;
        [JsonProperty("width")] private int _width;

        [JsonIgnore] public int Height { get => _height; }
        [JsonIgnore] public int Width { get => _width; }
    }

    public struct Position
    {
        [JsonProperty("X")] private int _x;
        [JsonProperty("Y")] private int _y;

        [JsonIgnore] public int X { get => _x; }
        [JsonIgnore] public int Y { get => _y; }
    }

    public struct Color
    {
        [JsonProperty("R")] private int _r;
        [JsonProperty("G")] private int _g;
        [JsonProperty("B")] private int _b;

        [JsonIgnore] public int R { get => _r; }
        [JsonIgnore] public int G { get => _g; }
        [JsonIgnore] public int B { get => _b; }
    }

    public struct Block
    {
        [JsonProperty("Pos")] private Position _pos;
        [JsonProperty("Color")] private Color _color;

        [JsonIgnore] public Position Pos { get => _pos; }
        [JsonIgnore] public Color Color { get => _color; }
    }

    public struct Section
    {
        [JsonProperty("blocks")] private List<List<Block>> _blocks;
        [JsonProperty("isVisible")] private bool _isVisible;
        [JsonProperty("used_colors")] private List<List<Color>> _usedColors;

        [JsonIgnore] public List<List<Block>> Blocks { get => _blocks; }
        [JsonIgnore] public bool IsVisible { get => _isVisible; }
        [JsonIgnore] public List<List<Color>> UsedColors { get => _usedColors; }
    }

    [JsonProperty("originSize")] private ArtSize _originSize;
    [JsonProperty("sections")] private List<List<Section>> _sections;

    [JsonIgnore] public ArtSize OriginSize { get => _originSize; }
    [JsonIgnore] public List<List<Section>> Sections { get => _sections; }
}