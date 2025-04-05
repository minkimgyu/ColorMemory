using NetworkService.DTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FilterUI;

public class CollectPageModel
{
    bool _activeContent;
    bool _activeSelectStageContent;

    string _title;
    string _description;

    Dictionary<int, ArtworkData> _artworkDatas;
    Dictionary<int, CollectArtData> _collectArtDatas;
    Dictionary<int, ArtData> _artDatas;
    List<KeyValuePair<int, ArtData>> _filteredArtDatas;

    int _artworkIndex;
    int _selectedSectionIndex;

    int _currentProgress;

    bool _activeStageDetailContent;
    int _usedHintCount;
    int _wrongCount;

    bool _activeFilterBottomSheet;
    bool _activeFilterContent;
    float _collectRatio;
    RankFilter _rankFilter;
    DateFilter _dateFilter;

    public CollectPageModel(Dictionary<int, ArtData> artDatas, Dictionary<int, ArtworkData> artworkDatas, Dictionary<int, CollectArtData> collectArtDatas)
    {
        _activeContent = false;
        _activeSelectStageContent = false;
        _activeStageDetailContent = false;
        _title = "";
        _description = "";
        _artworkIndex = 1;
        _selectedSectionIndex = 1;

        _currentProgress = 0;

        _usedHintCount = 0;
        _wrongCount = 0;

        _activeFilterBottomSheet = false;
        _activeFilterContent = false;
        _collectRatio = 0;
        _rankFilter = RankFilter.None;
        _dateFilter = DateFilter.None;

        _artDatas = artDatas;
        _filteredArtDatas = new List<KeyValuePair<int, ArtData>>(_artDatas);
        _artworkDatas = artworkDatas;
        _collectArtDatas = collectArtDatas;
    }

    public bool ActiveContent { get => _activeContent; set => _activeContent = value; }
    public bool ActiveSelectStageContent { get => _activeSelectStageContent; set => _activeSelectStageContent = value; }
    public bool ActiveStageDetailContent { get => _activeStageDetailContent; set => _activeStageDetailContent = value; }

    public string Title { get => _title; set => _title = value; }
    public string Description { get => _description; set => _description = value; }
    public int ArtworkIndex { get => _artworkIndex; set => _artworkIndex = value; }
    public int SelectedSectionIndex { get => _selectedSectionIndex; set => _selectedSectionIndex = value; }

    public int UsedHintCount { get => _usedHintCount; set => _usedHintCount = value; }
    public int WrongCount { get => _wrongCount; set => _wrongCount = value; }


    public Dictionary<int, ArtworkData> ArtworkDatas { get => _artworkDatas; }
    public Dictionary<int, CollectArtData> CollectArtDatas { get => _collectArtDatas; set => _collectArtDatas = value; }
    public Dictionary<int, ArtData> ArtDatas { get => _artDatas; set => _artDatas = value; }

    public int CurrentProgress { get => _currentProgress; set => _currentProgress = value; }



    public float CollectRatio { get => _collectRatio; set => _collectRatio = value; }
    public RankFilter RankFilter { get => _rankFilter; set => _rankFilter = value; }
    public DateFilter DateFilter { get => _dateFilter; set => _dateFilter = value; }
    public bool ActiveFilterContent { get => _activeFilterContent; set => _activeFilterContent = value; }
    public List<KeyValuePair<int, ArtData>> FilteredArtDatas { get => _filteredArtDatas; set => _filteredArtDatas = value; }
    public bool ActiveFilterBottomSheet { get => _activeFilterBottomSheet; set => _activeFilterBottomSheet = value; }
}
