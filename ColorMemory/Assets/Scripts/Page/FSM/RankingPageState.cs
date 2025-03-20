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
        RankingFactory factory,
        FSM<HomePage.InnerPageState> fsm) : base(fsm)
    {
        _factory = factory;

        RankingPageModel rankingPageModel = new RankingPageModel();
        _rankingPagePresenter = new RankingPagePresenter(rankingPageModel);
        RankingPageViewer rankingPageViewer = new RankingPageViewer(rankingContent, scrollContent);
        _rankingPagePresenter.InjectViewer(rankingPageViewer);
    }

    List<RankingUI> _rankingUIs = new List<RankingUI>();

    public override void OnClickHomeBtn()
    {
        _fsm.SetState(HomePage.InnerPageState.Main);
    }

    List<RankingData> GetRankingDatas()
    {
        // ���������ֱ�
        List<RankingData> rankingDatas = new List<RankingData>();
        string[] names = { "Alice", "Bob", "Charlie", "David", "Eve", "Frank", "Grace", "Hank", "Ivy", "Jack" };

        int count = 10; // ������ ������ ����
        for (int i = 0; i < count; i++)
        {
            RankingIconName iconName = (RankingIconName)UnityEngine.Random.Range(0, Enum.GetValues(typeof(RankingIconName)).Length);
            string name = names[i];
            int score = UnityEngine.Random.Range(0, 100000000);
            int rank = i + 1; // 1���� �����ϴ� ����

            rankingDatas.Add(new RankingData(iconName, name, score, rank));
        }

        return rankingDatas;
    }

    public override void OnStateEnter()
    {
        // ���������ֱ�
        List<RankingData> rankingDatas = GetRankingDatas();

        for (int i = 0; i < rankingDatas.Count; i++)
        {
            RankingUI rankingUI = _factory.Create(rankingDatas[i]);
            _rankingUIs.Add(rankingUI);
            _rankingPagePresenter.AddRakingItems(rankingUI);
        }

        _rankingPagePresenter.ActiveContent(true); // home �ݾ��ֱ�
    }

    public override void OnStateExit()
    {
        // �ı������ֱ�
        for (int i = 0; i < _rankingUIs.Count; i++)
        {
            _rankingUIs[i].DestroyObject();
            _rankingUIs.RemoveAt(i);
            i--;
        }
        _rankingPagePresenter.ActiveContent(false); // home �ݾ��ֱ�
    }
}
