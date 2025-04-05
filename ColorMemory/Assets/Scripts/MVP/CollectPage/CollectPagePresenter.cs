using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using NetworkService.Manager;
using NetworkService.DTO;
using System.Reflection;
using DG.Tweening;

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

    void AddFilterItem(string title)
    {
        SpawnableUI filterItem = _filterItemFactory.Create(title);
        filterItem.InjectClickEvent(() => { OnClickRankToggle(0); }); // 제거 시, 기본 필터로 전환
        _collectPageViewer.AddFilterItem(filterItem);
    }

    void AddFilterItem(Rank rank, string title)
    {
        SpawnableUI filterItem = _filterItemFactory.Create(rank, title);
        filterItem.InjectClickEvent(() => { OnClickRankToggle(0); }); // 제거 시, 기본 필터로 전환
        _collectPageViewer.AddFilterItem(filterItem);
    }

    public void OnClickRankToggle(FilterUI.RankFilter rank)
    {
        // 아트워크를 파괴하는 코드 필요
        DestroyAllArtwork();
        // 필터 뽀개는 함수 필요

        DOVirtual.DelayedCall(0.1f, () =>
        {
            _collectPageModel.RankFilter = rank;
            switch (_collectPageModel.RankFilter)
            {
                case FilterUI.RankFilter.All:
                    AddFilterItem("All");
                    break;
                case FilterUI.RankFilter.NoClear:
                    AddFilterItem("No Clear");
                    break;
                case FilterUI.RankFilter.Bronze:
                    AddFilterItem(Rank.COPPER, "Bronze");
                    break;
                case FilterUI.RankFilter.Silver:
                    AddFilterItem(Rank.SILVER, "Silver");
                    break;
                case FilterUI.RankFilter.Gold:
                    AddFilterItem(Rank.GOLD, "Gold");
                    break;
                default:
                    break;
            }

            // 필터링 하는 코드 필요함
            FilterArtData();
            ActivateFilterContent(false);

            // 다시 필터링된 아트워크를 채워넣는 코드 필요
            FillArtwork();
        });
    }

    public void OnClickDateToggle(FilterUI.DateFilter date)
    {
        // 아트워크를 파괴하는 코드 필요
        DestroyAllArtwork();
        // 필터 뽀개는 함수 필요

        DOVirtual.DelayedCall(0.1f, () =>
        {

            _collectPageModel.DateFilter = date;
            switch (_collectPageModel.DateFilter)
            {
                case FilterUI.DateFilter.Old:
                    AddFilterItem("오래된 순");
                    break;
                case FilterUI.DateFilter.New:
                    AddFilterItem("최신 순");
                    break;
                default:
                    break;
            }

            // 필터링 하는 코드 필요함
            FilterArtData();
            ActivateFilterContent(false);
            // 다시 필터링된 아트워크를 채워넣는 코드 필요
            FillArtwork();
        });
    }

    void FilterArtData()
    {
        // 데이터 초기화
        _collectPageModel.FilteredArtDatas.Clear();

        foreach (var item in _collectPageModel.ArtDatas)
        {
            switch (_collectPageModel.RankFilter)
            {
                case FilterUI.RankFilter.NoClear:
                    if (item.Value.HasIt == false)
                        _collectPageModel.FilteredArtDatas.Add(new KeyValuePair<int, ArtData>(item.Key, item.Value));
                    break;
                case FilterUI.RankFilter.Bronze:
                    if (item.Value.Rank == Rank.COPPER)
                        _collectPageModel.FilteredArtDatas.Add(new KeyValuePair<int, ArtData>(item.Key, item.Value));
                    break;
                case FilterUI.RankFilter.Silver:
                    if (item.Value.Rank == Rank.SILVER)
                        _collectPageModel.FilteredArtDatas.Add(new KeyValuePair<int, ArtData>(item.Key, item.Value));
                    break;
                case FilterUI.RankFilter.Gold:
                    if (item.Value.Rank == Rank.GOLD)
                        _collectPageModel.FilteredArtDatas.Add(new KeyValuePair<int, ArtData>(item.Key, item.Value));
                    break;
                default:
                    _collectPageModel.FilteredArtDatas.Add(new KeyValuePair<int, ArtData>(item.Key, item.Value));
                    break;
            }
        }

        switch (_collectPageModel.DateFilter)
        {
            case FilterUI.DateFilter.Old:
                _collectPageModel.FilteredArtDatas.Sort((a, b) =>
                {
                    if (!a.Value.ObtainedDate.HasValue) return 1;  // a가 null이면 뒤로
                    if (!b.Value.ObtainedDate.HasValue) return -1; // b가 null이면 앞으로
                    return a.Value.ObtainedDate.Value.CompareTo(b.Value.ObtainedDate.Value);
                });
                break;

            case FilterUI.DateFilter.New:
                _collectPageModel.FilteredArtDatas.Sort((a, b) =>
                {
                    if (!a.Value.ObtainedDate.HasValue) return 1;  // a가 null이면 뒤로
                    if (!b.Value.ObtainedDate.HasValue) return -1; // b가 null이면 앞으로
                    return b.Value.ObtainedDate.Value.CompareTo(a.Value.ObtainedDate.Value);
                });
                break;

            default:
                break;
        }
    }

    void SelectStage(int index, StageData stageData)
    {
        ServiceLocater.ReturnSaveManager().SelectArtworkSection(index);
        _collectPageModel.SelectedSectionIndex = index;
        _collectPageViewer.SelectStage(_collectPageModel.SelectedSectionIndex);

        if (stageData.IsPlayed == false) ActiveStageDetailContent(false);
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
            int keyIndex = item.Key; // ✅ 지역 변수로 캡처

            SpawnableUI artwork = _artworkFactory.Create(keyIndex, item.Value.Rank, item.Value.HasIt);
            artwork.InjectClickEvent(() => {
                ServiceLocater.ReturnSaveManager().SelectArtwork(keyIndex);
                ActiveSelectStageContent(true);
                FillStage();
            });

            _collectPageViewer.AddArtwork(artwork);

            ArtworkData artworkData = _collectPageModel.ArtworkDatas[keyIndex]; // ✅ keyIndex 사용
            SpawnableUI filteredArtwork = _filteredArtworkFactory.Create(keyIndex, artworkData.Title, item.Value.HasIt);
            filteredArtwork.InjectClickEvent(() => { // ✅ filteredArtwork에도 적용
                _collectPageViewer.ActivateFilterBottomSheet(false);
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
            if (stageData.IsLock == false && stageData.IsPlayed == true) // 잠겨있지 않고 이전에 플레이 한 경우만 진행도로 친다
            {
                progressCount++;
            }
        }

        ChangeCurrentProgress(progressCount);

        int _openIndex = 1;

        foreach (var data in artData.StageDatas)
        {
            SpawnableUI spawnableUI = _stageUIFactory.Create();
            spawnableUI.InjectClickEvent(() =>
            {
                int index = data.Key;
                SelectStage(index - 1, data.Value);
            });

            if (data.Value.IsLock == true)
            {
                spawnableUI.SetState(StageUI.State.Lock);
            }
            else
            {
                spawnableUI.SetState(StageUI.State.Open);
                if(data.Value.IsPlayed == true) spawnableUI.SetRank(data.Value.Rank); // 한번 플레이 한 경우만 랭크 표시
                else _openIndex = data.Key;
            }
            _collectPageViewer.AddStage(spawnableUI);
        }

        // 열린 스테이지 Auto Select
        SelectStage(_openIndex - 1, artData.StageDatas[_openIndex]);
    }

    public void RemoveAllStage()
    {
        _collectPageViewer.RemoveAllStage();
    }

    public void PlayCollectMode()
    {
        OnClickPlayBtn?.Invoke();
    }

    void UpdateArtInfo()
    {
        int artIndex = _collectPageModel.ArtworkIndex;
        ArtworkData artData = _collectPageModel.ArtworkDatas[artIndex];
        _collectPageViewer.ChangeArtDescription(artData.Title, artData.Description);
    }

    public void ChangeArtworkList(List<int> currentArtNames)
    {
        //_collectPageModel.HaveArtworkIndexes = currentArtNames;
        //_collectPageModel.ArtworkIndex = 0;
        UpdateArtInfo();
    }

    public void ChangeArtworkDescription(int index)
    {
        _collectPageModel.ArtworkIndex = index + 1; // -> 1 추가해서 받기
        UpdateArtInfo();
    }
}
