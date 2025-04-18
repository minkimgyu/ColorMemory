using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;
using System;
using NetworkService.DTO;

public struct StageData
{
    Rank _rank;
    int _hintUsage;
    int _incorrectCnt;
    StageStauts _stageStauts;

    public StageData(
        Rank rank,
        int hintUsage,
        int incorrectCnt,
        StageStauts stageStauts)
    {
        _rank = rank;
        _hintUsage = hintUsage;
        _incorrectCnt = incorrectCnt;
        _stageStauts = stageStauts;
    }

    public Rank Rank { get => _rank; }

    /// <summary>
    /// 플레이 한 경우 false로 적용
    /// </summary>
    //public bool IsPlayed { get { return HintUsage != -1 || IncorrectCnt != -1; } }

    public int HintUsage { get => _hintUsage; }
    public int IncorrectCnt { get => _incorrectCnt; }
    public StageStauts Stauts { get => _stageStauts; }
}

public struct ArtData
{
    NetworkService.DTO.Rank _rank;
    bool _hasIt;

    Dictionary<int, StageData> _stageDatas;

    int _totalMistakesAndHints;
    DateTime? _obtainedDate;

    public ArtData(
        NetworkService.DTO.Rank rank,
        bool hasIt,

        Dictionary<int, StageData> stageDatas,
        int totalMistakesAndHints,
        DateTime? obtainedDate)
    {
        _rank = rank;
        _hasIt = hasIt;

        _stageDatas = stageDatas;

        _totalMistakesAndHints = totalMistakesAndHints;
        _obtainedDate = obtainedDate;
    }

    public NetworkService.DTO.Rank Rank { get => _rank; }
    public bool HasIt { get => _hasIt; }

    public int TotalMistakesAndHints { get => _totalMistakesAndHints; }
    public DateTime? ObtainedDate { get => _obtainedDate; set => _obtainedDate = value; }
    public Dictionary<int, StageData> StageDatas { get => _stageDatas; set => _stageDatas = value; }
}

[System.Serializable]
public struct CollectArtData
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