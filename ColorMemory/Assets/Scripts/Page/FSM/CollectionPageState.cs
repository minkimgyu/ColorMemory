using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionPageState : BaseState<HomePage.InnerPageState>
{
    ArtworkUIFactory _artworkFactory;
    CollectPagePresenter _collectPagePresenter;

    public CollectionPageState(
        Button homeBtn,
        GameObject collectionContent,
        TMP_Text titleTxt,
        TMP_Text descriptionTxt,

        Image completeSlider,
        TMP_Text leftCompleteText,
        TMP_Text totalCompleteText,

        GameObject selectStageContent,
        Transform stageUIContent,
        Button exitBtn,
        Button playBtn,

        TMP_Text stageUsedHintUseCount,
        TMP_Text stageWrongCount,

        ArtworkScrollUI artworkScrollUI,

        ArtworkUIFactory artworkFactory,
        StageUIFactory stageUIFactory,

        List<int> artNames,
        Dictionary<int, ArtworkData> artworkDatas,
        Dictionary<int, CollectiveArtData> artDatas,
        FSM<HomePage.InnerPageState> fsm) : base(fsm)
    {
        CollectPageModel collectPageModel = new CollectPageModel(artNames, artworkDatas, artDatas);
        _collectPagePresenter = new CollectPagePresenter(collectPageModel, artworkFactory, stageUIFactory, GoToCollectMode);
        CollectPageViewer collectPageViewer = new CollectPageViewer(
            collectionContent,
            titleTxt,
            descriptionTxt,
            artworkScrollUI,
            completeSlider,
            leftCompleteText,
            totalCompleteText,
            selectStageContent,
            stageUIContent,
            exitBtn,
            playBtn,
            stageUsedHintUseCount,
            stageWrongCount,
            _collectPagePresenter);

        _collectPagePresenter.InjectViewer(collectPageViewer);
        _collectPagePresenter.ActiveContent(false);
    }

    void GoToCollectMode()
    {
        ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.CollectScene);
    }

    public override void OnClickShopBtn()
    {
        _fsm.SetState(HomePage.InnerPageState.Shop);
    }

    public override void OnClickRankingBtn()
    {
        _fsm.SetState(HomePage.InnerPageState.Ranking);
    }

    public override void OnClickHomeBtn()
    {
        _fsm.SetState(HomePage.InnerPageState.Main);
    }

    public override void OnStateEnter()
    {
        _collectPagePresenter.FillArtwork();
        _collectPagePresenter.ActiveContent(true); // home ╢щ╬фаж╠Б
    }

    public override void OnStateExit()
    {
        _collectPagePresenter.RemoveAllArtwork();
        _collectPagePresenter.ActiveContent(false); // home ╢щ╬фаж╠Б
    }
}
