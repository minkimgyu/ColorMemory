using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public struct ArtworkData
{
    [JsonProperty] string _artist;
    [JsonProperty] string _title;
    [JsonProperty] string _description;

    public ArtworkData(string artist, string title, string description)
    {
        _artist = artist;
        _title = title;
        _description = description;
    }

    [JsonIgnore] public string Artist { get => _artist; }
    [JsonIgnore] public string Title { get => _title; }
    [JsonIgnore] public string Description { get => _description; }
}

[System.Serializable]
public struct ArtworkDateWrapper
{
    [JsonProperty] Dictionary<int, ArtworkData> _data;

    public ArtworkDateWrapper(Dictionary<int, ArtworkData> data)
    {
        _data = data;
    }

    [JsonIgnore] public Dictionary<int, ArtworkData> Data { get => _data; }
}