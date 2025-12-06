using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;
using System;
using NetworkService.DTO;

[System.Serializable]
public struct StageData
{
    [JsonProperty("rank")] Rank _rank;
    [JsonProperty("hintUsage")] int _hintUsage;
    [JsonProperty("incorrectCnt")] int _incorrectCnt;
    [JsonProperty("stageStauts")] StageStauts _stageStauts;

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

    [JsonIgnore] public Rank Rank { get => _rank; }

    /// <summary>
    /// 플레이 한 경우 false로 적용
    /// </summary>
    //public bool IsPlayed { get { return HintUsage != -1 || IncorrectCnt != -1; } }

    [JsonIgnore] public int HintUsage { get => _hintUsage; }
    [JsonIgnore] public int IncorrectCnt { get => _incorrectCnt; }
    [JsonIgnore] public StageStauts Stauts { get => _stageStauts; }
}

[System.Serializable]
public struct ArtData
{
    [JsonProperty("rank")] NetworkService.DTO.Rank _rank;
    [JsonProperty("hasIt")] bool _hasIt;

    [JsonProperty("stageDatas")] Dictionary<int, StageData> _stageDatas;

    [JsonProperty("totalMistakes")] int _totalMistakes;
    [JsonProperty("totalHints")] int _totalHints;

    [JsonProperty("obtainedDate")] DateTime? _obtainedDate;

    public ArtData(
        NetworkService.DTO.Rank rank,
        bool hasIt,

        Dictionary<int, StageData> stageDatas,
        int totalHints,
        int totalMistakes,
        DateTime? obtainedDate)
    {
        _rank = rank;
        _hasIt = hasIt;

        _stageDatas = stageDatas;

        _totalHints = totalHints;
        _totalMistakes = totalMistakes;
        _obtainedDate = obtainedDate;
    }

    [JsonIgnore] public NetworkService.DTO.Rank Rank { get => _rank; }
    [JsonIgnore] public bool HasIt { get => _hasIt; }

    [JsonIgnore] public DateTime? ObtainedDate { get => _obtainedDate; set => _obtainedDate = value; }
    [JsonIgnore] public Dictionary<int, StageData> StageDatas { get => _stageDatas; set => _stageDatas = value; }
    [JsonIgnore] public int TotalMistakes { get => _totalMistakes; set => _totalMistakes = value; }
    [JsonIgnore] public int TotalHints { get => _totalHints; set => _totalHints = value; }
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