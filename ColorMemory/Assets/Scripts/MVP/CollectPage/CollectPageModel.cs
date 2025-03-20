using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectPageModel
{
    bool _activeContent;
    string _title;
    string _description;

    Dictionary<ArtName, ArtData> _artworkDatas;
    Dictionary<ArtName, CollectiveArtData> _artData;
    int _artworkIndex;

    public CollectPageModel(Dictionary<ArtName, ArtData> artworkDatas, Dictionary<ArtName, CollectiveArtData> artData)
    {
        _activeContent = false;
        _title = "";
        _description = "";
        _artworkIndex = 0;
        _artworkDatas = artworkDatas;
        _artData = artData;
    }

    public bool ActiveContent { get => _activeContent; set => _activeContent = value; }
    public string Title { get => _title; set => _title = value; }
    public string Description { get => _description; set => _description = value; }
    public int ArtworkIndex { get => _artworkIndex; set => _artworkIndex = value; }
    public Dictionary<ArtName, CollectiveArtData> ArtData { get => _artData; }
    public Dictionary<ArtName, ArtData> ArtworkDatas { get => _artworkDatas; }
}
