using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectPageModel
{
    bool _activeContent;
    bool _activeSelectStageContent;

    string _title;
    string _description;

    Dictionary<ArtName, ArtData> _artworkDatas;
    Dictionary<ArtName, CollectiveArtData> _artData;

    int _artworkIndex;
    List<ArtName> _currentArtNames;

    public CollectPageModel(List<ArtName> currentArtNames, Dictionary<ArtName, ArtData> artworkDatas, Dictionary<ArtName, CollectiveArtData> artData)
    {
        _activeContent = false;
        _activeSelectStageContent = false;
        _title = "";
        _description = "";
        _artworkIndex = 0;

        _currentArtNames = currentArtNames;
        _artworkDatas = artworkDatas;
        _artData = artData;
    }

    public bool ActiveContent { get => _activeContent; set => _activeContent = value; }
    public bool ActiveSelectStageContent { get => _activeSelectStageContent; set => _activeSelectStageContent = value; }

    public string Title { get => _title; set => _title = value; }
    public string Description { get => _description; set => _description = value; }
    public int ArtworkIndex { get => _artworkIndex; set => _artworkIndex = value; }
    public Dictionary<ArtName, CollectiveArtData> ArtData { get => _artData; }
    public Dictionary<ArtName, ArtData> ArtworkDatas { get => _artworkDatas; }
    public List<ArtName> CurrentArtNames { get => _currentArtNames; set => _currentArtNames = value; }
}
