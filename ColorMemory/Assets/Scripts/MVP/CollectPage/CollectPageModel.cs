using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectPageModel
{
    bool _activeContent;
    bool _activeSelectStageContent;

    string _title;
    string _description;

    Dictionary<int, ArtworkData> _artworkDatas;
    Dictionary<int, CollectiveArtData> _artData;

    int _artworkIndex;
    List<int> _haveArtworkIndexes;

    Vector2Int _selectedArtworkIndex;

    int _usedHintCount;
    int _wrongCount;

    public CollectPageModel(List<int> currentArtworkIndexes, Dictionary<int, ArtworkData> artworkDatas, Dictionary<int, CollectiveArtData> artData)
    {
        _activeContent = false;
        _activeSelectStageContent = false;
        _title = "";
        _description = "";
        _artworkIndex = 0;
        _selectedArtworkIndex = Vector2Int.zero;

        _usedHintCount = 0;
        _wrongCount = 0;

        _haveArtworkIndexes = currentArtworkIndexes;
        _artworkDatas = artworkDatas;
        _artData = artData;
    }

    public bool ActiveContent { get => _activeContent; set => _activeContent = value; }
    public bool ActiveSelectStageContent { get => _activeSelectStageContent; set => _activeSelectStageContent = value; }

    public string Title { get => _title; set => _title = value; }
    public string Description { get => _description; set => _description = value; }
    public int ArtworkIndex { get => _artworkIndex; set => _artworkIndex = value; }
    public Dictionary<int, CollectiveArtData> ArtData { get => _artData; }
    public Dictionary<int, ArtworkData> ArtworkDatas { get => _artworkDatas; }
    public List<int> HaveArtworkIndexes { get => _haveArtworkIndexes; set => _haveArtworkIndexes = value; }
    public Vector2Int SelectedArtworkIndex { get => _selectedArtworkIndex; set => _selectedArtworkIndex = value; }

    public int UsedHintCount { get => _usedHintCount; set => _usedHintCount = value; }
    public int WrongCount { get => _wrongCount; set => _wrongCount = value; }
}
