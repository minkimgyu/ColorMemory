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
    Dictionary<int, CollectArtData> _collectArtDatas;
    Dictionary<int, ArtData> _artDatas;

    int _artworkIndex;
    int _selectedSectionIndex;

    float _currentProgress;

    int _usedHintCount;
    int _wrongCount;

    public CollectPageModel(Dictionary<int, ArtData> artDatas, Dictionary<int, ArtworkData> artworkDatas, Dictionary<int, CollectArtData> collectArtDatas)
    {
        _activeContent = false;
        _activeSelectStageContent = false;
        _title = "";
        _description = "";
        _artworkIndex = 1;
        _selectedSectionIndex = 1;

        _currentProgress = 0;

        _usedHintCount = 0;
        _wrongCount = 0;

        _artDatas = artDatas;
        _artworkDatas = artworkDatas;
        _collectArtDatas = collectArtDatas;
    }

    public bool ActiveContent { get => _activeContent; set => _activeContent = value; }
    public bool ActiveSelectStageContent { get => _activeSelectStageContent; set => _activeSelectStageContent = value; }

    public string Title { get => _title; set => _title = value; }
    public string Description { get => _description; set => _description = value; }
    public int ArtworkIndex { get => _artworkIndex; set => _artworkIndex = value; }
    public int SelectedSectionIndex { get => _selectedSectionIndex; set => _selectedSectionIndex = value; }

    public int UsedHintCount { get => _usedHintCount; set => _usedHintCount = value; }
    public int WrongCount { get => _wrongCount; set => _wrongCount = value; }


    public Dictionary<int, ArtworkData> ArtworkDatas { get => _artworkDatas; }
    public Dictionary<int, CollectArtData> CollectArtDatas { get => _collectArtDatas; set => _collectArtDatas = value; }
    public Dictionary<int, ArtData> ArtDatas { get => _artDatas; set => _artDatas = value; }

    public float CurrentProgress { get => _currentProgress; set => _currentProgress = value; }
}
