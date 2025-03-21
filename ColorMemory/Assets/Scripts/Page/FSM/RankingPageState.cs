using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RankingPageState : BaseState<HomePage.InnerPageState>
{
    RankingPagePresenter _rankingPagePresenter;
    RankingFactory _factory;

    public RankingPageState(
        GameObject rankingContent,
        Transform scrollContent,
        Transform myRankingContent,
        RankingFactory factory,
        FSM<HomePage.InnerPageState> fsm) : base(fsm)
    {
        _factory = factory;

        RankingPageModel rankingPageModel = new RankingPageModel();
        _rankingPagePresenter = new RankingPagePresenter(rankingPageModel);
        RankingPageViewer rankingPageViewer = new RankingPageViewer(rankingContent, scrollContent, myRankingContent);
        _rankingPagePresenter.InjectViewer(rankingPageViewer);

        _rankingPagePresenter.ActiveContent(false);
    }

    List<RankingUI> _rankingUIs = new List<RankingUI>();
    RankingUI _myRankingUI;

    public override void OnClickHomeBtn()
    {
        _fsm.SetState(HomePage.InnerPageState.Main);
    }

    RankingData GetRankingData()
    {
        List<PersonalRankingData> topRankingDatas = new List<PersonalRankingData>();
        string[] names = { "Alice", "Bob", "Charlie", "David", "Eve", "Frank", "Grace", "Hank", "Ivy", "Jack" };

        int count = 10; // 생성할 데이터 개수
        for (int i = 0; i < count; i++)
        {
            RankingIconName iconName = (RankingIconName)UnityEngine.Random.Range(0, Enum.GetValues(typeof(RankingIconName)).Length);
            string name = names[i];
            int score = UnityEngine.Random.Range(0, 100000000);
            int rank = i + 1; // 1부터 시작하는 순위

            topRankingDatas.Add(new PersonalRankingData(iconName, name, score, rank));
        }

        // 생성시켜주기
        PersonalRankingData myRankingData = new PersonalRankingData((RankingIconName)1, "Meal", 10000000, 15);

        RankingData rankingData = new RankingData(topRankingDatas, myRankingData);
        return rankingData;
    }

    public override void OnStateEnter()
    {
        // 생성시켜주기
        RankingData rankingData = GetRankingData();

        for (int i = 0; i < rankingData.TopRankingDatas.Count; i++)
        {
            RankingUI rankingUI = _factory.Create(rankingData.TopRankingDatas[i]);
            _rankingUIs.Add(rankingUI);
            _rankingPagePresenter.AddRakingItems(rankingUI);
        }

        RankingUI myRankingUI = _factory.Create(rankingData.MyRankingData);
        _myRankingUI = myRankingUI;
        _rankingPagePresenter.AddMyRaking(myRankingUI);

        _rankingPagePresenter.ActiveContent(true); // home 닫아주기
    }

    public override void OnStateExit()
    {
        _myRankingUI.DestroyObject();

        // 파괴시켜주기
        for (int i = 0; i < _rankingUIs.Count; i++)
        {
            _rankingUIs[i].DestroyObject();
            _rankingUIs.RemoveAt(i);
            i--;
        }
        _rankingPagePresenter.ActiveContent(false); // home 닫아주기
    }
}
