using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using NetworkService.Manager;
using NetworkService.DTO;
using System.Reflection;
using DG.Tweening;
using Challenge;
using System.Collections.ObjectModel;

public class CollectPagePresenter
{
    CollectPageViewer _collectPageViewer;
    CollectPageModel _collectPageModel;

    ArtworkUIFactory _artworkFactory;
    StageUIFactory _stageUIFactory;

    FilteredArtworkFactory _filteredArtworkFactory;
    FilterItemFactory _filterItemFactory;

    Action OnClickPlayBtn;

    public CollectPagePresenter(
        CollectPageModel collectPageModel,
        ArtworkUIFactory artworkFactory,
        StageUIFactory stageUIFactory,
        FilteredArtworkFactory filteredArtworkFactory,
        FilterItemFactory filterItemFactory,

        Action OnClickPlayBtn)
    {
        _collectPageModel = collectPageModel;

        _artworkFactory = artworkFactory;
        _stageUIFactory = stageUIFactory;

        _filteredArtworkFactory = filteredArtworkFactory;
        _filterItemFactory = filterItemFactory;

        this.OnClickPlayBtn = OnClickPlayBtn;
    }

    public void SwitchArtworkInfoContent(bool active)
    {
        _collectPageModel.ActiveArtworkInfoContent = active;
        _collectPageViewer.SwitchArtworkInfoContent(_collectPageModel.ActiveArtworkInfoContent);
    }

    public void ChangeCollectionRatioInfo()
    {
        int hasCount = 0;
        int maxCount = _collectPageModel.ArtDatas.Count;
        foreach (var item in _collectPageModel.ArtDatas)
        {
            if (item.Value.HasIt == true) hasCount++;
        }

        float ratio = hasCount / maxCount;
        _collectPageModel.CollectionRatio = Mathf.RoundToInt(ratio * 100);
        _collectPageViewer.ChangeCollectionRatioInfo(_collectPageModel.CollectionRatio);
    }

    public void ChangeCollectionCheerInfo()
    {
        _collectPageModel.CollectionCheerText = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.FilterCheerInfo);
        _collectPageViewer.ChangeCollectionCheerInfo(_collectPageModel.CollectionCheerText);
    }

    public void ChangeFilterTitle()
    {
        _collectPageModel.OwnFilterTitle = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.OwnFilterTitle);
        _collectPageModel.RankFilterTitle = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.RankFilterTitle);
        _collectPageModel.DateFilterTitle = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.DateFilterTitle);

        _collectPageViewer.ChangeFilterTitle(
            _collectPageModel.OwnFilterTitle,
            _collectPageModel.RankFilterTitle,
            _collectPageModel.DateFilterTitle);
    }

    public void InjectViewer(CollectPageViewer collectPageViewer)
    {
        _collectPageViewer = collectPageViewer;
        //ChangeArtworkDescription(_collectPageModel.ArtworkIndex);
    }

    public void ActivateFilterScrollUI(bool active)
    {
        _collectPageModel.ActiveFilterBottomSheet = active;
        _collectPageViewer.ActivateFilterScrollUI(_collectPageModel.ActiveFilterBottomSheet);
    }


    public void ActivateFilterContent(bool active)
    {
        // 아트워크를 파괴하는 코드 필요
        // DestroyAllArtwork();

        _collectPageModel.ActiveFilterContent = active;
        _collectPageViewer.ActiveFilterContent(_collectPageModel.ActiveFilterContent);
    }


    #region Filter

    // 데이터에 맞게 토클도 변경시켜줄 필요가 있음

    void UpdateFilterToggle()
    {
        _collectPageViewer.UpdateToggles(
            _collectPageModel.OwnFilter,
            _collectPageModel.RankFilters,
            _collectPageModel.DateFilter);
    }

    void UpdateFilterItems()
    {
        DestroyFilterItems();
        DOVirtual.DelayedCall(0.5f, () =>
        {
            FillFilterItems();
        });
    }

    void DestroyFilterItems()
    {
        _collectPageViewer.DestroyFilterItems();
    }

    readonly Dictionary<FilterUI.OwnFilter, string> _ownFilterDescription = new Dictionary<FilterUI.OwnFilter, string>
    {
        { FilterUI.OwnFilter.All, "All" },
        { FilterUI.OwnFilter.Clear, "Clear" },
        { FilterUI.OwnFilter.NoClear, "No Clear" },
    };

    readonly Dictionary<FilterUI.DateFilter, string> _dateFilterDescription = new Dictionary<FilterUI.DateFilter, string>
    {
        { FilterUI.DateFilter.Old, "Old" },
        { FilterUI.DateFilter.New, "New" },
    };

    void FillFilterItems()
    {
        SpawnableUI ownFilterItem = _filterItemFactory.Create(_ownFilterDescription[_collectPageModel.OwnFilter]);
        _collectPageViewer.AddFilterItem(ownFilterItem);

        ownFilterItem.InjectClickEvent(() => {
            // 데이터를 바탕으로 필터를 자동으로 생성해준다.
            // 제거 시, 데이터를 변경하고 다시 모든 필터 부수고 생성시키기
            // 토글도 다시 정상화 필요함
            OnClickOwnToggle(FilterUI.OwnFilter.All);
        });


        // 필터가 획득한 명화만 보여주는 경우에 랭킹과 날짜 필터를 추가해준다.
        if (_collectPageModel.OwnFilter == FilterUI.OwnFilter.Clear)
        {
            for (int i = 0; i < _collectPageModel.RankFilters.Count; i++)
            {
                FilterUI.RankFilter filter = _collectPageModel.RankFilters[i];
                string rankFilterName = filter.ToString();

                Rank convertedRank = (Rank)((int)filter);
                SpawnableUI rankFilterItem = _filterItemFactory.Create(convertedRank, rankFilterName);
                _collectPageViewer.AddFilterItem(rankFilterItem);

                rankFilterItem.InjectClickEvent(() => {
                    // 데이터를 바탕으로 필터를 자동으로 생성해준다.
                    // 제거 시, 데이터를 변경하고 다시 모든 필터 부수고 생성시키기
                    OnUnClickRankToggle(filter);
                });
            }

            SpawnableUI dateFilterItem = _filterItemFactory.Create(_dateFilterDescription[_collectPageModel.DateFilter]);
            _collectPageViewer.AddFilterItem(dateFilterItem);

            dateFilterItem.InjectClickEvent(() => {
                // 데이터를 바탕으로 필터를 자동으로 생성해준다.
                // 제거 시, 데이터를 변경하고 다시 모든 필터 부수고 생성시키기
                OnClickDateToggle(FilterUI.DateFilter.Old);
            });
        }
    }

    void UpdateFilter()
    {
        // 아트워크를 파괴하는 코드 필요
        DestroyAllArtwork();
        UpdateFilterItems(); // 필터를 업데이트 하는 함수
        UpdateFilterToggle(); // 필터 토글 업데이트 하는 함수

        // 필터링 하는 코드 필요함
        FilterArtData();
        ActivateFilterContent(false);

        DOVirtual.DelayedCall(0.5f, () =>
        {
            // 다시 필터링된 아트워크를 채워넣는 코드 필요
            FillArtwork();

            // 여기에 설명 업데이트도 필요함
            // 첫번째 위치로 스크롤 해줌
            _collectPageViewer.SetArtworkScrollIndex(0);
            OnArtworkScrollChanged(0);
        });
    }

    public void OnClickOwnToggle(FilterUI.OwnFilter own)
    {
        _collectPageModel.OwnFilter = own;
        UpdateFilter();
    }

    public void OnClickRankToggle(FilterUI.RankFilter rank)
    {
        if(_collectPageModel.RankFilters.Contains(rank) == false) _collectPageModel.RankFilters.Add(rank);
        UpdateFilter();
    }

    public void OnUnClickRankToggle(FilterUI.RankFilter rank)
    {
        if (_collectPageModel.RankFilters.Contains(rank) == true) _collectPageModel.RankFilters.Remove(rank);
        UpdateFilter();
    }



    public void OnClickDateToggle(FilterUI.DateFilter date)
    {
        _collectPageModel.DateFilter = date;
        UpdateFilter();
    }

    void FilterArtData()
    {
        // 데이터 초기화
        _collectPageModel.FilteredArtDatas.Clear();

        foreach (var item in _collectPageModel.ArtDatas)
        {
            if (_collectPageModel.OwnFilter == FilterUI.OwnFilter.All)
            {
                _collectPageModel.FilteredArtDatas.Add(new KeyValuePair<int, ArtData>(item.Key, item.Value));
            }
            else if (_collectPageModel.OwnFilter == FilterUI.OwnFilter.NoClear &&
                item.Value.HasIt == false)
            {
                _collectPageModel.FilteredArtDatas.Add(new KeyValuePair<int, ArtData>(item.Key, item.Value));
            }
            else if(_collectPageModel.OwnFilter == FilterUI.OwnFilter.Clear && 
                item.Value.HasIt == true)
            {
                if (_collectPageModel.RankFilters.Count == 0) // 필터가 없는 경우
                {
                    _collectPageModel.FilteredArtDatas.Add(new KeyValuePair<int, ArtData>(item.Key, item.Value));
                }
                else
                {
                    if (_collectPageModel.RankFilters.Contains(FilterUI.RankFilter.Bronze) &&
                        item.Value.Rank == Rank.COPPER)
                    {
                        _collectPageModel.FilteredArtDatas.Add(new KeyValuePair<int, ArtData>(item.Key, item.Value));
                    }

                    if (_collectPageModel.RankFilters.Contains(FilterUI.RankFilter.Silver) &&
                        item.Value.Rank == Rank.SILVER)
                    {
                        _collectPageModel.FilteredArtDatas.Add(new KeyValuePair<int, ArtData>(item.Key, item.Value));
                    }

                    if (_collectPageModel.RankFilters.Contains(FilterUI.RankFilter.Gold) &&
                        item.Value.Rank == Rank.GOLD)
                    {
                        _collectPageModel.FilteredArtDatas.Add(new KeyValuePair<int, ArtData>(item.Key, item.Value));
                    }
                }
            }
        }

        // 필터가 보유한 명화만 보여주는 경우 정렬을 적용한다.
        if (_collectPageModel.OwnFilter == FilterUI.OwnFilter.Clear)
        {
            switch (_collectPageModel.DateFilter)
            {
                case FilterUI.DateFilter.Old:
                    _collectPageModel.FilteredArtDatas.Sort((a, b) =>
                    {
                        return a.Value.ObtainedDate.Value.CompareTo(b.Value.ObtainedDate.Value);
                    });
                    break;

                case FilterUI.DateFilter.New:
                    _collectPageModel.FilteredArtDatas.Sort((a, b) =>
                    {
                        return b.Value.ObtainedDate.Value.CompareTo(a.Value.ObtainedDate.Value);
                    });
                    break;

                default:
                    break;
            }
        }
    }

    #endregion Filter

    void SelectStage(int index, StageData stageData)
    {
        ServiceLocater.ReturnSaveManager().SelectArtworkSection(index);
        _collectPageModel.SelectedSectionIndex = index;
        _collectPageViewer.SelectStage(_collectPageModel.SelectedSectionIndex);

        if (stageData.Stauts != StageStauts.Clear) ActiveStageDetailContent(false);
        else ActiveStageDetailContent(true);

        ChangeStageDetails(stageData.HintUsage, stageData.IncorrectCnt);
    }

    void ChangeCurrentProgress(int currentProgress)
    {
        _collectPageModel.CurrentProgress = currentProgress;
        _collectPageViewer.ChangeCurrentProgress(_collectPageModel.CurrentProgress);
    }



    void ActiveStageDetailContent(bool active)
    {
        _collectPageModel.ActiveStageDetailContent = active;
        _collectPageViewer.ActiveStageDetailContent(_collectPageModel.ActiveStageDetailContent);
    }

    void ChangeStageDetails(int usedHintCount, int wrongCount)
    {
        _collectPageModel.UsedHintCount = usedHintCount;
        _collectPageModel.WrongCount = wrongCount;
        _collectPageViewer.ChangeStageDetails(_collectPageModel.UsedHintCount, _collectPageModel.WrongCount);
    }

    public void ActiveContent(bool active)
    {
        _collectPageModel.ActiveContent = active;
        _collectPageViewer.ActiveContent(_collectPageModel.ActiveContent);
    }

    public void ActiveSelectStageContent(bool active)
    {
        if (active == false) RemoveAllStage();
        _collectPageModel.ActiveSelectStageContent = active;
        _collectPageViewer.ActiveSelectStageContent(_collectPageModel.ActiveSelectStageContent);
    }

    public void FillArtwork()
    {
        foreach (var item in _collectPageModel.FilteredArtDatas)
        {
            int artworkIndex = item.Key; // ✅ 지역 변수로 캡처

            SpawnableUI artwork = _artworkFactory.Create(artworkIndex, item.Value.Rank, item.Value.HasIt);
            artwork.InjectClickEvent(() => {
                ServiceLocater.ReturnSaveManager().SelectArtwork(artworkIndex);
                ActiveSelectStageContent(true);
                FillStage();
            });

            _collectPageViewer.AddArtwork(artwork);

            ArtworkData artworkData = _collectPageModel.ArtworkDatas[artworkIndex]; // ✅ keyIndex 사용
            int scrollIndex = _collectPageModel.FilteredArtDatas.FindIndex(x => x.Key == artworkIndex);

            SpawnableUI filteredArtwork = _filteredArtworkFactory.Create(artworkIndex, artworkData.Title, item.Value.HasIt);
            filteredArtwork.InjectClickEvent(() => { // ✅ filteredArtwork에도 적용

                _collectPageViewer.ActivateFilterBottomSheet(false);
                _collectPageViewer.SetArtworkScrollIndex(scrollIndex);
                OnArtworkScrollChanged(scrollIndex);
                // ✅ 위 아트워크를 스크롤하는 코드 추가 필요
            });

            _collectPageViewer.AddFilteredArtwork(filteredArtwork);
        }

        _collectPageViewer.SetUpArtworkScroll(_collectPageModel.FilteredArtDatas.Count);
    }

    public void AddArtwork(SpawnableUI artwork)
    {
        _collectPageViewer.AddArtwork(artwork);
    }

    public void DestroyAllArtwork()
    {
        //for (int i = 0; i < _spawnedArtworks.Count; i++)
        //{
        //    _spawnedArtworks[i].DestroyObject();
        //}

        //_spawnedArtworks.Clear();
        _collectPageViewer.DestroyAllArtwork();
        _collectPageViewer.DestroyFilteredArtwork();
    }




    public void FillStage()
    {
        int artworkIndex = _collectPageModel.ArtworkIndex;
        ArtData artData = _collectPageModel.ArtDatas[artworkIndex];

        int progressCount = 0;

        foreach (var data in artData.StageDatas)
        {
            StageData stageData = artData.StageDatas[data.Key];
            if (stageData.Stauts == StageStauts.Clear) 
            {
                // 이전에 플레이 한 경우만 진행도로 친다
                progressCount++;
            }
        }

        ChangeCurrentProgress(progressCount);

        int _openIndex = 0;

        foreach (var data in artData.StageDatas)
        {
            SpawnableUI spawnableUI = _stageUIFactory.Create();
            spawnableUI.InjectClickEvent(() =>
            {
                int index = data.Key;
                SelectStage(index, data.Value);
            });

            switch (data.Value.Stauts)
            {
                case StageStauts.Lock:
                    spawnableUI.SetState(StageUI.State.Lock);
                    break;
                case StageStauts.Open:
                    spawnableUI.SetState(StageUI.State.Open);
                    _openIndex = data.Key;
                    break;
                case StageStauts.Clear:
                    spawnableUI.SetState(StageUI.State.Open);
                    spawnableUI.SetRank(data.Value.Rank); // 한번 플레이 한 경우만 랭크 표시
                    break;
                default:
                    break;
            }

            _collectPageViewer.AddStage(spawnableUI);
        }

        // 열린 스테이지 Auto Select
        SelectStage(_openIndex, artData.StageDatas[_openIndex]);
    }

    public void RemoveAllStage()
    {
        _collectPageViewer.RemoveAllStage();
    }

    public void PlayCollectMode()
    {
        OnClickPlayBtn?.Invoke();
    }

    void UpdateArtInfo(int artworkIndex)
    {
        ArtworkData artworkData = _collectPageModel.ArtworkDatas[artworkIndex];
        _collectPageViewer.ChangeArtDescription(artworkData.Title, artworkData.Description);

        ArtData artData = _collectPageModel.ArtDatas[artworkIndex];
        int totalStageCount = artData.StageDatas.Count;
        int clearStageCount = 0;
        foreach (var data in artData.StageDatas)
        {
            if (data.Value.Stauts == StageStauts.Clear) clearStageCount++;
        }

        float currentCompleteRatio = (float)clearStageCount / totalStageCount;
        _collectPageViewer.ChangeArtCompleteRatio(currentCompleteRatio, _collectPageModel.TotalCompleteRatio);
    }

    void UpdateArtInfo()
    {
        string noFilteredArtworkInfo = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.NoFilteredArtwork);
        _collectPageViewer.ChangeArtDescription(noFilteredArtworkInfo, "");
        _collectPageViewer.ChangeArtCompleteRatio(0, _collectPageModel.TotalCompleteRatio);
    }

    //
    public void OnArtworkScrollChanged(int scrollIndex)
    {
        if (_collectPageModel.FilteredArtDatas.Count == 0)
        {
            UpdateArtInfo();
            return;
        }

        var item = _collectPageModel.FilteredArtDatas[scrollIndex];

        _collectPageModel.ArtworkIndex = item.Key; // -> 1 추가해서 받기
        UpdateArtInfo(_collectPageModel.ArtworkIndex);
    }

    public void ScrollArtworkToIndex(int artworkIndex)
    {
        _collectPageModel.ArtworkIndex = artworkIndex; // -> 1 추가해서 받기
        UpdateArtInfo(_collectPageModel.ArtworkIndex);

        int scrollIndex = _collectPageModel.FilteredArtDatas.FindIndex(x => x.Key == artworkIndex);
        _collectPageViewer.SetArtworkScrollIndex(scrollIndex);
    }
}