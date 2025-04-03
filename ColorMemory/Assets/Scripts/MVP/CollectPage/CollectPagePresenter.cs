using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using NetworkService.Manager;
using NetworkService.DTO;
using System.Reflection;

public class CollectPagePresenter
{
    CollectPageViewer _collectPageViewer;
    CollectPageModel _collectPageModel;

    ArtworkUIFactory _artworkFactory;
    StageUIFactory _stageUIFactory;

    Action OnClickPlayBtn;

    public CollectPagePresenter(
        CollectPageModel collectPageModel,
        ArtworkUIFactory artworkFactory,
        StageUIFactory stageUIFactory,
        Action OnClickPlayBtn)
    {
        _collectPageModel = collectPageModel;

        _artworkFactory = artworkFactory;
        _stageUIFactory = stageUIFactory;

        this.OnClickPlayBtn = OnClickPlayBtn;
    }

    public void InjectViewer(CollectPageViewer collectPageViewer)
    {
        _collectPageViewer = collectPageViewer;
        //ChangeArtworkDescription(_collectPageModel.ArtworkIndex);
    }

    void SelectStage(int index)
    {
        ServiceLocater.ReturnSaveManager().SelectArtworkSection(index - 1);
        _collectPageModel.SelectedSectionIndex = index;
        _collectPageViewer.SelectStage(_collectPageModel.SelectedSectionIndex);
    }

    void ChangeCurrentProgress(int currentProgress)
    {
        _collectPageModel.CurrentProgress = currentProgress;
        _collectPageViewer.ChangeCurrentProgress(_collectPageModel.CurrentProgress);
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
        _collectPageModel.ActiveSelectStageContent = active;
        _collectPageViewer.ActiveSelectStageContent(_collectPageModel.ActiveSelectStageContent);
    }

    public void FillArtwork()
    {
        foreach (var item in _collectPageModel.ArtDatas)
        {
            int keyIndex = item.Key;

            SpawnableUI artwork = _artworkFactory.Create(keyIndex, NetworkService.DTO.Rank.COPPER);
            artwork.InjectClickEvent(() => {
                RemoveAllStage();

                ActiveSelectStageContent(true);
                FillStage();
            });

            _collectPageViewer.AddArtwork(artwork);
        }

        _collectPageViewer.SetUpArtworkScroll(_collectPageModel.ArtDatas.Count);
    }

    public void AddArtwork(SpawnableUI artwork)
    {
        _collectPageViewer.AddArtwork(artwork);
    }

    public void RemoveAllArtwork()
    {
        _collectPageViewer.RemoveAllArtwork();
    }



    public void FillStage()
    {
        int artworkIndex = _collectPageModel.ArtworkIndex;
        ArtData artData = _collectPageModel.ArtDatas[artworkIndex];

        bool allStageNoClear = true; // 모든 스테이지가 클리어 되지 않은 경우
        int progressCount = 0;

        foreach (var data in artData.StageDatas)
        {
            StageData stageData = artData.StageDatas[data.Key];
            if (stageData.Rank != Rank.NONE)
            {
                progressCount++;
                allStageNoClear = false;
            }
        }

        foreach (var data in artData.StageDatas)
        {
            SpawnableUI spawnableUI = _stageUIFactory.Create();
            spawnableUI.InjectClickEvent(() =>
            {
                int index = data.Key;
                SelectStage(index);
                ChangeStageDetails(artData.StageDatas[index].HintUsage, artData.StageDatas[index].IncorrectCnt);
            });

            StageData stageData = artData.StageDatas[data.Key];

            if(allStageNoClear == true && data.Key == 1) // 모두 미클이고 첫번째 스테이지인 경우
            {
                spawnableUI.SetState(StageUI.State.Open); // 스테이지 열기
            }
            else
            {
                switch (stageData.Rank)
                {
                    case Rank.NONE:
                        spawnableUI.SetState(StageUI.State.Lock);
                        break;
                    default:
                        allStageNoClear = false;
                        spawnableUI.SetState(StageUI.State.Open);
                        spawnableUI.SetRank(stageData.Rank);
                        break;
                }
            }

            _collectPageViewer.AddStage(spawnableUI);
        }

        // Auto Select 기능
        SelectStage(1);
        ChangeStageDetails(artData.StageDatas[1].HintUsage, artData.StageDatas[1].IncorrectCnt);
        ChangeCurrentProgress(progressCount);
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
