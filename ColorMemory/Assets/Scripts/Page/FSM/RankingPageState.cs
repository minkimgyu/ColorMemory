using NetworkService.DTO;
using NetworkService.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class RankingPageState : BaseState<HomePage.InnerPageState>
{
    RankingPagePresenter _rankingPagePresenter;
    RankingUIFactory _rankingUIFactory;

    public RankingPageState(
        GameObject rankingContent,
        Transform scrollContent,
        Transform myRankingContent,
        RankingUIFactory rankingUIFactory,
        FSM<HomePage.InnerPageState> fsm) : base(fsm)
    {
        _rankingUIFactory = rankingUIFactory;

        RankingPageModel rankingPageModel = new RankingPageModel();
        _rankingPagePresenter = new RankingPagePresenter(rankingPageModel);
        RankingPageViewer rankingPageViewer = new RankingPageViewer(rankingContent, scrollContent, myRankingContent);
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

    async Task<Tuple<List<PlayerRankingDTO>, PlayerRankingDTO>> GetRankingDataFromServer()
    {
        ScoreManager scoreManager = new ScoreManager();
        List<PlayerRankingDTO> otherScores;
        PlayerRankingDTO myScore;
        //int ranking = 0;

        try
        {
            otherScores = await scoreManager.GetTopWeeklyScoresAsync(10);
            myScore = await scoreManager.GetPlayerWeeklyScoreAsDTOAsync("testId1");
            //ranking = await scoreManager.GetPlayerWeeklyRankingAsync("testId1");
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            Debug.Log("서버로부터 데이터를 받아오지 못함");
            return null;
        }

        return new Tuple<List<PlayerRankingDTO>, PlayerRankingDTO>(otherScores, myScore);
    }

    public override async void OnStateEnter()
    {
        Tuple<List<PlayerRankingDTO>, PlayerRankingDTO> rankingData = await GetRankingDataFromServer();
        if (rankingData == null) return;

        List<PersonalRankingData> topRankingDatas = new List<PersonalRankingData>();

        for (int i = 0; i < rankingData.Item1.Count; i++)
        {
            topRankingDatas.Add(new PersonalRankingData(1, rankingData.Item1[i].Name, rankingData.Item1[i].Score, rankingData.Item1[i].Ranking));
        }

        PlayerRankingDTO playerScoreDTO = rankingData.Item2;
        PersonalRankingData myRankingData = new PersonalRankingData(1, playerScoreDTO.Name, playerScoreDTO.Score, playerScoreDTO.Ranking);

        for (int i = 0; i < topRankingDatas.Count; i++)
        {
            SpawnableUI rankingUI = _rankingUIFactory.Create(topRankingDatas[i]);
            _rankingPagePresenter.AddRakingItems(rankingUI);
        }

        SpawnableUI myRankingUI = _rankingUIFactory.Create(myRankingData);
        _rankingPagePresenter.AddMyRaking(myRankingUI);
        _rankingPagePresenter.ActiveContent(true); // home 닫아주기
    }

    public override void OnStateExit()
    {
        _rankingPagePresenter.DestroyRankingItems();
        _rankingPagePresenter.ActiveContent(false); // home 닫아주기
    }
}
