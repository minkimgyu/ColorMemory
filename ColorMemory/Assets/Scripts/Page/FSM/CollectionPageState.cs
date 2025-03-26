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
        Button playBtn,

        ArtworkScrollUI artworkScrollUI,

        ArtworkUIFactory artworkFactory,
        StageUIFactory stageUIFactory,

        Dictionary<ArtName, ArtData> artworkDatas,
        Dictionary<ArtName, CollectiveArtData> artDatas,
        FSM<HomePage.InnerPageState> fsm) : base(fsm)
    {
        int artDataCount = Enum.GetValues(typeof(ArtName)).Length;
        artworkScrollUI.SetUp(artDataCount);

        List<ArtName> artNames = new List<ArtName>();
        for (int i = 0; i < artworkDatas.Count; i++) artNames.Add((ArtName)i);

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
            playBtn,
            _collectPagePresenter);

        _collectPagePresenter.InjectViewer(collectPageViewer);
        _collectPagePresenter.ActiveContent(false);
    }

    void GoToCollectMode()
    {
        ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.CollectScene);
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
