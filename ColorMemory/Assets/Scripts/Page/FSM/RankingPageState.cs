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

    public override void OnClickHomeBtn()
    {
        _fsm.SetState(HomePage.InnerPageState.Main);
    }

    RankingData GetRankingData()
    {
        List<PersonalRankingData> topRankingDatas = new List<PersonalRankingData>();
        string[] names = { "Alice", "Bob", "Charlie", "David", "Eve", "Frank", "Grace", "Hank", "Ivy", "Jack" };

        int count = 10; // ������ ������ ����
        for (int i = 0; i < count; i++)
        {
            RankingIconName iconName = (RankingIconName)UnityEngine.Random.Range(0, Enum.GetValues(typeof(RankingIconName)).Length);
            string name = names[i];
            int score = UnityEngine.Random.Range(0, 100000000);
            int rank = i + 1; // 1���� �����ϴ� ����

            topRankingDatas.Add(new PersonalRankingData(iconName, name, score, rank));
        }

        // ���������ֱ�
        PersonalRankingData myRankingData = new PersonalRankingData((RankingIconName)1, "Meal", 10000000, 15);

        RankingData rankingData = new RankingData(topRankingDatas, myRankingData);
        return rankingData;
    }

    public override void OnStateEnter()
    {
        // ���������ֱ�
        RankingData rankingData = GetRankingData();

        for (int i = 0; i < rankingData.OtherRankingDatas.Count; i++)
        {
            RankingUI rankingUI = _factory.Create(rankingData.OtherRankingDatas[i]);
            _rankingPagePresenter.AddRakingItems(rankingUI);
        }

        RankingUI myRankingUI = _factory.Create(rankingData.MyRankingData);
        _rankingPagePresenter.AddMyRaking(myRankingUI);
        _rankingPagePresenter.ActiveContent(true); // home �ݾ��ֱ�
    }

    public override void OnStateExit()
    {
        _rankingPagePresenter.DestroyRankingItems();
        _rankingPagePresenter.ActiveContent(false); // home �ݾ��ֱ�
    }
}
