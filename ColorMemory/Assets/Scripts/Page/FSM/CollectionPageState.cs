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

        GameObject artworkInfoContent,
        GameObject artworkCompleteRatioContent,

        TMP_Text titleTxt,
        TMP_Text descriptionTxt,

        Image completeSlider,
        TMP_Text leftCompleteText,
        TMP_Text totalCompleteText,

        GameObject selectStageContent,
        Transform stageUIContent,
        Button exitBtn,
        Button playBtn,

        GameObject stageDetailContent,

        TMP_Text stageUsedHintUseCount,
        TMP_Text stageWrongCount,

        FilterUI filterScrollUI,
        Button filterOpenBtn,
        Button filterExitBtn,
        GameObject filterContent,
        TMP_Text collectionRatioText,
        Toggle[] rankToggles,
        Toggle[] dateToggles,

        ArtworkScrollUI artworkScrollUI,

        ArtworkUIFactory artworkFactory,
        StageUIFactory stageUIFactory,

        FilteredArtworkFactory filteredArtworkFactory,
        FilterItemFactory filterItemFactory,

        Dictionary<int, ArtData> artDatas,
        Dictionary<int, ArtworkData> artworkDatas,
        Dictionary<int, CollectArtData> collectArtDatas,
        FSM<HomePage.InnerPageState> fsm) : base(fsm)
    {
        CollectPageModel collectPageModel = new CollectPageModel(
            artDatas,
            artworkDatas,
            collectArtDatas);

        _collectPagePresenter = new CollectPagePresenter(
            collectPageModel,
            artworkFactory,
            stageUIFactory,
            filteredArtworkFactory,
            filterItemFactory,
            GoToCollectMode);

        CollectPageViewer collectPageViewer = new CollectPageViewer(
            collectionContent,

            artworkInfoContent,
            artworkCompleteRatioContent,

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
            stageDetailContent,
            stageUsedHintUseCount,
            stageWrongCount,
            filterScrollUI,
            filterOpenBtn,
            filterExitBtn,
            filterContent,
            collectionRatioText,
            rankToggles,
            dateToggles,
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
        _collectPagePresenter.ChangeCollectionRatioInfo();
        _collectPagePresenter.ActivateFilterScrollUI(true);
        _collectPagePresenter.ChangeArtworkDescription(0);
        _collectPagePresenter.FillArtwork();
        _collectPagePresenter.ActiveContent(true); // home ╢щ╬фаж╠Б
    }

    public override void OnStateExit()
    {
        _collectPagePresenter.DestroyAllArtwork();
        _collectPagePresenter.ActivateFilterScrollUI(false);
        _collectPagePresenter.ActiveContent(false); // home ╢щ╬фаж╠Б
    }
}
