using NetworkService.DTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectPageModel
{
    bool _activeContent;
    bool _activeSelectStageContent;



    bool _activeArtworkInfoContent;
    string _title;
    string _description;

    Dictionary<int, ArtworkData> _artworkDatas;
    Dictionary<int, CollectArtData> _collectArtDatas;
    Dictionary<int, ArtData> _artDatas;
    float _totalCompleteRatio;


    List<KeyValuePair<int, ArtData>> _filteredArtDatas;

    int _scrollIndex;
    int _selectedSectionIndex;

    int _currentProgress;

    bool _activeStageDetailContent;
    int _usedHintCount;
    int _wrongCount;

    float _collectionRatio;
    string _collectionCheerText;

    bool _activeFilterBottomSheet;
    bool _activeFilterContent;
    float _collectRatio;

    string _ownFilterTitle;
    string _rankFilterTitle;
    string _dateFilterTitle;

    FilterUI.OwnFilter _ownFilter;
    List<FilterUI.RankFilter> _rankFilters;
    FilterUI.DateFilter _dateFilter;


    public CollectPageModel(Dictionary<int, ArtData> artDatas, Dictionary<int, ArtworkData> artworkDatas, Dictionary<int, CollectArtData> collectArtDatas)
    {
        _activeContent = false;
        _activeSelectStageContent = false;
        _activeStageDetailContent = false;

        _activeArtworkInfoContent = true;
        _title = "";
        _description = "";
        _scrollIndex = 0;
        _selectedSectionIndex = 0;

        _collectionRatio = 0;
        _collectionCheerText = "";

        _currentProgress = 0;

        _usedHintCount = 0;
        _wrongCount = 0;

        _activeFilterBottomSheet = false;
        _activeFilterContent = false;
        _collectRatio = 0;

        _ownFilter = FilterUI.OwnFilter.All;
        _rankFilters = new List<FilterUI.RankFilter>();
        _dateFilter = FilterUI.DateFilter.Old;

        _artDatas = artDatas;

        int completeCount = 0;
        foreach (var artData in _artDatas)
        {
            if (artData.Value.HasIt == true) completeCount++;
        }

        _totalCompleteRatio = (float)completeCount / _artDatas.Count;

        _filteredArtDatas = new List<KeyValuePair<int, ArtData>>(_artDatas);
        _artworkDatas = artworkDatas;
        _collectArtDatas = collectArtDatas;
    }

    public float CollectionRatio { get => _collectionRatio; set => _collectionRatio = value; }
    public string CollectionCheerText { get => _collectionCheerText; set => _collectionCheerText = value; }


    public string OwnFilterTitle { get => _ownFilterTitle; set => _ownFilterTitle = value; }
    public string RankFilterTitle { get => _rankFilterTitle; set => _rankFilterTitle = value; }
    public string DateFilterTitle { get => _dateFilterTitle; set => _dateFilterTitle = value; }

    public bool ActiveContent { get => _activeContent; set => _activeContent = value; }
    public bool ActiveSelectStageContent { get => _activeSelectStageContent; set => _activeSelectStageContent = value; }
    public bool ActiveStageDetailContent { get => _activeStageDetailContent; set => _activeStageDetailContent = value; }


    public bool ActiveArtworkInfoContent { get => _activeArtworkInfoContent; set => _activeArtworkInfoContent = value; }


    public string Title { get => _title; set => _title = value; }
    public string Description { get => _description; set => _description = value; }
    public int ArtworkIndex { get => _scrollIndex; set => _scrollIndex = value; }
    public int SelectedSectionIndex { get => _selectedSectionIndex; set => _selectedSectionIndex = value; }

    public int UsedHintCount { get => _usedHintCount; set => _usedHintCount = value; }
    public int WrongCount { get => _wrongCount; set => _wrongCount = value; }


    public Dictionary<int, ArtworkData> ArtworkDatas { get => _artworkDatas; }
    public Dictionary<int, CollectArtData> CollectArtDatas { get => _collectArtDatas; set => _collectArtDatas = value; }
    public Dictionary<int, ArtData> ArtDatas { get => _artDatas; set => _artDatas = value; }

    public int CurrentProgress { get => _currentProgress; set => _currentProgress = value; }


    public float CollectRatio { get => _collectRatio; set => _collectRatio = value; }
    public bool ActiveFilterContent { get => _activeFilterContent; set => _activeFilterContent = value; }
    public List<KeyValuePair<int, ArtData>> FilteredArtDatas { get => _filteredArtDatas; set => _filteredArtDatas = value; }
    public bool ActiveFilterBottomSheet { get => _activeFilterBottomSheet; set => _activeFilterBottomSheet = value; }
    public float TotalCompleteRatio { get => _totalCompleteRatio; set => _totalCompleteRatio = value; }
    
    
    public FilterUI.OwnFilter OwnFilter { get => _ownFilter; set => _ownFilter = value; }
    public List<FilterUI.RankFilter> RankFilters { get => _rankFilters; set => _rankFilters = value; }
    public FilterUI.DateFilter DateFilter { get => _dateFilter; set => _dateFilter = value; }

}