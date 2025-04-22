using NetworkService.DTO;
using NetworkService.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RankingPageState : BaseState<HomePage.InnerPageState>
{
    RankingPagePresenter _rankingPagePresenter;
    RankingUIFactory _rankingUIFactory;

    IRankingService _rankingService;

    public RankingPageState(
        GameObject rankingContent,
        TMPro.TMP_Text rankingTitleText,
        Transform scrollContent,
        Transform myRankingContent,
        RankingUIFactory rankingUIFactory,
        IRankingService rankingService,
        FSM<HomePage.InnerPageState> fsm) : base(fsm)
    {
        _rankingUIFactory = rankingUIFactory;
        _rankingService = rankingService;

        RankingPageModel rankingPageModel = new RankingPageModel();
        _rankingPagePresenter = new RankingPagePresenter(rankingPageModel);
        RankingPageViewer rankingPageViewer = new RankingPageViewer(rankingContent, rankingTitleText, scrollContent, myRankingContent);
        _rankingPagePresenter.InjectViewer(rankingPageViewer);

        _rankingPagePresenter.ActiveContent(false);
    }

    public override void OnClickShopBtn()
    {
        _fsm.SetState(HomePage.InnerPageState.Shop);
    }

    public override void OnClickHomeBtn()
    {
        _fsm.SetState(HomePage.InnerPageState.Main);
    }

    const int topRange = 10;

    public override async void OnStateEnter()
    {
        string userId = ServiceLocater.ReturnSaveManager().GetSaveData().UserId;
        Tuple<List<PersonalRankingData>, PersonalRankingData> rankingData = await _rankingService.GetTopRankingData(topRange, userId);
        if (rankingData == null) return;

        for (int i = 0; i < rankingData.Item1.Count; i++)
        {
            SpawnableUI rankingUI = _rankingUIFactory.Create(rankingData.Item1[i]);
            _rankingPagePresenter.AddRakingItems(rankingUI);
        }

        SpawnableUI myRankingUI = _rankingUIFactory.Create(rankingData.Item2);
        _rankingPagePresenter.AddMyRaking(myRankingUI);
        _rankingPagePresenter.ActiveContent(true); // home ╢щ╬фаж╠Б
    }

    public override void OnStateExit()
    {
        _rankingPagePresenter.DestroyRankingItems();
        _rankingPagePresenter.ActiveContent(false); // home ╢щ╬фаж╠Б
    }
}
