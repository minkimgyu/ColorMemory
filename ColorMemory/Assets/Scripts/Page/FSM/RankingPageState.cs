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

    public override void ChangeLanguage()
    {
        OnStateExit();
        OnStateEnter();
    }

    const int topRange = 10;

    public override async void OnStateEnter()
    {
        _rankingPagePresenter.ChangeRankingTitle();

        string userId = ServiceLocater.ReturnSaveManager().GetSaveData().UserId;
        Tuple<List<PersonalRankingData>, PersonalRankingData> rankingData = await _rankingService.GetTopRankingData(topRange, userId);
        if (rankingData == null) return;

        for (int i = 0; i < rankingData.Item1.Count; i++)
        {
            SpawnableUI rankingUI = _rankingUIFactory.Create(rankingData.Item1[i]);

            if (rankingData.Item2.Rank == rankingData.Item1[i].Rank) rankingUI.ChangeSelect(true); // 내 랭킹과 같은 순위라면 선택해줌
            _rankingPagePresenter.AddRakingItems(rankingUI);
        }

        SpawnableUI myRankingUI = _rankingUIFactory.Create(rankingData.Item2);
        _rankingPagePresenter.AddMyRaking(myRankingUI);
        _rankingPagePresenter.ActiveContent(true); // home 닫아주기
    }

    public override void OnStateExit()
    {
        _rankingPagePresenter.DestroyRankingItems();
        _rankingPagePresenter.ActiveContent(false); // home 닫아주기
    }
}
