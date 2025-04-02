using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

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
        ChangeArtworkDescription(_collectPageModel.ArtworkIndex);
    }

    public void ChangeStageDetails(int usedHintCount, int wrongCount)
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
        for (int i = 0; i < _collectPageModel.HaveArtworkIndexes.Count; i++)
        {
            SpawnableUI artwork = _artworkFactory.Create(_collectPageModel.HaveArtworkIndexes[i], NetworkService.DTO.Rank.COPPER);
            artwork.InjectClickEvent(() => {
                RemoveAllStage();

                ActiveSelectStageContent(true);
                FillStage();
            });

            _collectPageViewer.AddArtwork(artwork);
        }

        _collectPageViewer.SetUpArtworkScroll(_collectPageModel.HaveArtworkIndexes.Count);
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
        int artworkIndex = _collectPageModel.HaveArtworkIndexes[_collectPageModel.ArtworkIndex];
        List<List<CollectiveArtData.Section>> sections = _collectPageModel.ArtData[artworkIndex].Sections;

        int row = sections.Count;
        int col = sections[0].Count;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                SpawnableUI spawnableUI = _stageUIFactory.Create(new Vector2Int(i, j));
                spawnableUI.InjectClickEvent((index) => 
                {
                    ServiceLocater.ReturnSaveManager().SelectArtwork(artworkIndex.ToString(), index);

                    _collectPageModel.SelectedArtworkIndex = index;
                    _collectPageViewer.SelectStage(index);
                });

                spawnableUI.SetState(StageUI.State.Open);
                spawnableUI.SetRank(NetworkService.DTO.Rank.SILVER);
                _collectPageViewer.AddStage(spawnableUI);
            }
        }
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
        int artIndex = _collectPageModel.HaveArtworkIndexes[_collectPageModel.ArtworkIndex];
        ArtworkData artData = _collectPageModel.ArtworkDatas[artIndex];
        _collectPageViewer.ChangeArtDescription(artData.Title, artData.Description);
    }

    public void ChangeArtworkList(List<int> currentArtNames)
    {
        _collectPageModel.HaveArtworkIndexes = currentArtNames;
        _collectPageModel.ArtworkIndex = 0;
        UpdateArtInfo();
    }

    public void ChangeArtworkDescription(int index)
    {
        _collectPageModel.ArtworkIndex = index;
        UpdateArtInfo();
    }
}
