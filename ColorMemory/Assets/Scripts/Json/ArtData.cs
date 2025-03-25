using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;

public enum ArtName
{
    ABlossomingBush,
    AChristmasRepast,
    ACoastalLandscapeintheSouthofFrance,
    ACottageGardenWithChickens,
    ADoewithFawn,
    AFavoriteSummerPastime,
    AForestPathwithHunteratSunset,
    AFreshBreeze,
    AGardenIdyll,
    AGardeninSeptember
}

[System.Serializable]
public struct ArtworkDataObject
{
    [JsonProperty] Dictionary<ArtName, ArtData> _data;

    public ArtworkDataObject(Dictionary<ArtName, ArtData> data)
    {
        _data = data;
    }

    [JsonIgnore] public Dictionary<ArtName, ArtData> Data { get => _data; }
}

[System.Serializable]
public struct ArtData
{
    [JsonProperty] ArtworkUI.Type _type;
    [JsonProperty] string _title;
    [JsonProperty] string _description;

    public ArtData(ArtworkUI.Type type, string title, string description)
    {
        _type = type;
        _title = title;
        _description = description;
    }

    [JsonIgnore] public string Title { get => _title; }
    [JsonIgnore] public string Description { get => _description; }
    public ArtworkUI.Type Type { get => _type; }
}

[System.Serializable]
public struct CollectiveArtData
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

        public UnityEngine.Color GetColor() => new UnityEngine.Color(_r/255f, _g/255f, _b/255f);
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