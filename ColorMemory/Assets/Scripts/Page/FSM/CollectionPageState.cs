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

        Image currentComplete,
        TMP_Text currentCompleteRatio,

        Image totalComplete,
        TMP_Text totalCompleteRatio,

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

        Toggle[] ownToggles,
        Toggle[] rankToggles,
        Toggle[] dateToggles,

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

            artworkInfoContent,
            artworkCompleteRatioContent,

            titleTxt,
            descriptionTxt,

            currentComplete,
            currentCompleteRatio,
            totalComplete,
            totalCompleteRatio,

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

            ownToggles,
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

        // ��Ʈ��ũ ä���
        _collectPagePresenter.FillArtwork();

        // content �����ֱ�
        _collectPagePresenter.ActiveContent(true);

        // ����� �� �ҷ��ͼ� �����ϱ�
        SaveData data = ServiceLocater.ReturnSaveManager().GetSaveData();
        _collectPagePresenter.ScrollArtworkToIndex(data.SelectedArtworkKey);
    }

    public override void OnStateExit()
    {
        _collectPagePresenter.RemoveAllArtwork();
        _collectPagePresenter.ActiveContent(false); // home �ݾ��ֱ�
    }
}
