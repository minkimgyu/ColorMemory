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

    //RankingData GetRankingData()
    //{
    //    List<PersonalRankingData> topRankingDatas = new List<PersonalRankingData>();
    //    string[] names = { "Alice", "Bob", "Charlie", "David", "Eve", "Frank", "Grace", "Hank", "Ivy", "Jack" };

    //    int count = 10; // 생성할 데이터 개수
    //    for (int i = 0; i < count; i++)
    //    {
    //        RankingIconName iconName = (RankingIconName)UnityEngine.Random.Range(0, Enum.GetValues(typeof(RankingIconName)).Length);
    //        string name = names[i];
    //        int score = UnityEngine.Random.Range(0, 100000000);
    //        int rank = i + 1; // 1부터 시작하는 순위

    //        topRankingDatas.Add(new PersonalRankingData(iconName, name, score, rank));
    //    }

    //    // 생성시켜주기
    //    PersonalRankingData myRankingData = new PersonalRankingData((RankingIconName)1, "Meal", 10000000, 15);

    //    RankingData rankingData = new RankingData(topRankingDatas, myRankingData);
    //    return rankingData;
    //}

    public override async void OnStateEnter()
    {
        ScoreManager scoreManager = new ScoreManager();


        List<PersonalRankingData> topRankingDatas = new List<PersonalRankingData>();

        List<PlayerScoreDTO> scores = await scoreManager.GetTopWeeklyScoresAsync(10);
        for (int i = 0; i < scores.Count; i++)
        {
            topRankingDatas.Add(new PersonalRankingData(RankingIconName.Icon5, scores[i].Name, scores[i].Score, i + 1));
        }

        //PlayerManager playerManager = new PlayerManager();
        //playerManager.getpla();

        int score = await scoreManager.GetPlayerWeeklyScore("testId1");
        PersonalRankingData myRankingData = new PersonalRankingData(RankingIconName.Icon5, "meal", score, 15);

        // 생성시켜주기
        RankingData rankingData = new RankingData(topRankingDatas, myRankingData);

        for (int i = 0; i < rankingData.OtherRankingDatas.Count; i++)
        {
            SpawnableUI rankingUI = _rankingUIFactory.Create(rankingData.OtherRankingDatas[i]);
            _rankingPagePresenter.AddRakingItems(rankingUI);
        }

        SpawnableUI myRankingUI = _rankingUIFactory.Create(rankingData.MyRankingData);
        _rankingPagePresenter.AddMyRaking(myRankingUI);
        _rankingPagePresenter.ActiveContent(true); // home 닫아주기
    }

    public override void OnStateExit()
    {
        _rankingPagePresenter.DestroyRankingItems();
        _rankingPagePresenter.ActiveContent(false); // home 닫아주기
    }
}
