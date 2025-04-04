using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        for (int i = 0; i < _collectPageModel.CurrentArtNames.Count; i++)
        {
            SpawnableUI artwork = _artworkFactory.Create(_collectPageModel.CurrentArtNames[i], Rank.COPPER);
            artwork.InjectClickEvent(() => {
                RemoveAllStage();

                ActiveSelectStageContent(true);
                FillStage();
            });

            _collectPageViewer.AddArtwork(artwork);
        }
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
        ArtName name = _collectPageModel.CurrentArtNames[_collectPageModel.ArtworkIndex];
        List<List<CollectiveArtData.Section>> sections = _collectPageModel.ArtData[name].Sections;

        int row = sections.Count;
        int col = sections[0].Count;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                SpawnableUI spawnableUI = _stageUIFactory.Create(sections[i][j].Blocks, sections[i][j].UsedColors);
                spawnableUI.ChangeIndex(new Vector2Int(i, j));

                spawnableUI.InjectClickEvent((index) => 
                {
                    ServiceLocater.ReturnSaveManager().SelectArtwork(name.ToString(), index);

                    _collectPageModel.SelectedArtworkIndex = index;
                    _collectPageViewer.SelectStage(index);
                });

                if(i == row - 1 && j == col - 1)
                {
                    spawnableUI.SetState(StageUI.State.Lock);
                }
                else
                {
                    spawnableUI.SetState(StageUI.State.Clear);
                }

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
        ArtName name = _collectPageModel.CurrentArtNames[_collectPageModel.ArtworkIndex];
        ArtData artData = _collectPageModel.ArtworkDatas[name];
        _collectPageViewer.ChangeArtDescription(artData.Title, artData.Description);
    }

    public void ChangeArtworkList(List<ArtName> currentArtNames)
    {
        _collectPageModel.CurrentArtNames = currentArtNames;
        _collectPageModel.ArtworkIndex = 0;
        UpdateArtInfo();
    }

    public void ChangeArtworkDescription(int index)
    {
        _collectPageModel.ArtworkIndex = index;
        UpdateArtInfo();
    }
}
