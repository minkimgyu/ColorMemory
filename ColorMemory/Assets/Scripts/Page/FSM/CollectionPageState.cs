using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        CustomProgressUI currentComplete,
        TMP_Text currentCompleteRatio,

        CustomProgressUI totalComplete,
        TMP_Text totalCompleteRatio,

        TMP_Text titleTxt,
        TMP_Text descriptionTxt,

        Image completeSlider,
        TMP_Text leftCompleteText,
        TMP_Text totalCompleteText,

        TMP_Text stageNameTxt,
        TMP_Text selectStageTitle,

        GameObject selectStageContent,
        Transform stageUIContent,

        TMP_Text stageUsedHintUseTitle,
        TMP_Text stageWrongCountTitle,

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
        TMP_Text collectionCheerText,

        TMP_Text ownTitleText,
        TMP_Text rankTitleText,
        TMP_Text dateTitleText,

        Toggle[] ownToggles,
        Toggle[] rankToggles,
        Toggle[] dateToggles,

        ArtworkScrollUI artworkScrollUI,

        ArtworkUIFactory artworkFactory,
        StageUIFactory stageUIFactory,

        FilteredArtworkFactory filteredArtworkFactory,
        FilterItemFactory filterItemFactory,

        Dictionary<int, ArtData> artDatas,
        Dictionary<ILocalization.Language, ArtworkDateWrapper> artworkDatas,
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
            filterItemFactory);

        _collectPagePresenter.OnClickPlayBtn += GoToCollectMode;

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

            stageNameTxt,
            selectStageTitle,

            selectStageContent,
            stageUIContent,

            stageUsedHintUseTitle,
            stageWrongCountTitle,

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
            collectionCheerText,

            ownTitleText,
            rankTitleText,
            dateTitleText,

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

    public override void ChangeLanguage()
    {
        OnStateExit();
        DOVirtual.DelayedCall(0.5f, () =>
        {
            OnStateEnter();
        });
    }

    public override void OnStateEnter()
    {
        AdManager.Instance.DestroyBannerAd();
        // content 열어주기
        _collectPagePresenter.ActiveContent(true);

        _collectPagePresenter.ChangeCollectionRatioInfo();
        _collectPagePresenter.ChangeCollectionCheerInfo();
        _collectPagePresenter.ChangeFilterTitle();

        _collectPagePresenter.ActivateFilterScrollUI(true);

        // 전체 필터 선택
        _collectPagePresenter.OnClickOwnToggle(FilterUI.OwnFilter.All);
    }

    public override void OnStateExit()
    {
        AdManager.Instance.LoadBannerAd();

        _collectPagePresenter.DestroyAllArtwork();
        _collectPagePresenter.ActivateFilterScrollUI(false);
        _collectPagePresenter.ActiveContent(false); // home 닫아주기
    }
}
