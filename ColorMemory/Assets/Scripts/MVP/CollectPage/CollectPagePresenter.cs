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
        foreach (var item in _collectPageModel.ArtDatas)
        {
            int keyIndex = item.Key;

            SpawnableUI artwork = _artworkFactory.Create(keyIndex, NetworkService.DTO.Rank.COPPER);
            artwork.InjectClickEvent(() => {
                ServiceLocater.ReturnSaveManager().SelectArtwork(keyIndex);
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
