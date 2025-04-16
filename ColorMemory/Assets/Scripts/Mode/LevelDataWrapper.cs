using NetworkService.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct LevelData
{
    [Newtonsoft.Json.JsonProperty] int _mapSize;
    [Newtonsoft.Json.JsonProperty] int _colorCount;
    [Newtonsoft.Json.JsonProperty] int _randomPointCount;
    [Newtonsoft.Json.JsonProperty] float _memorizeDuration;

    public LevelData(int mapSize, int colorCount, int randomPointCount, float memorizeDuration)
    {
        _mapSize = mapSize;
        _colorCount = colorCount;
        _randomPointCount = randomPointCount;
        _memorizeDuration = memorizeDuration;
    }

    [Newtonsoft.Json.JsonIgnore] public int MapSize { get => _mapSize; }
    [Newtonsoft.Json.JsonIgnore] public int ColorCount { get => _colorCount; }
    [Newtonsoft.Json.JsonIgnore] public int RandomPointCount { get => _randomPointCount; }
    [Newtonsoft.Json.JsonIgnore] public float MemorizeDuration { get => _memorizeDuration; }
}

[Serializable]
public struct LevelDataWrapper
{
    [Newtonsoft.Json.JsonProperty] List<LevelData> stageDatas;

    public LevelDataWrapper(List<LevelData> stageDatas)
    {
        this.stageDatas = stageDatas;
    }

    [Newtonsoft.Json.JsonIgnore] public List<LevelData> StageDatas { get => stageDatas; }
}