using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingPagePresenter
{
    RankingPageViewer _rankingPageViewer;
    RankingPageModel _rankingPageModel;

    public RankingPagePresenter(RankingPageModel rankingPageModel)
    {
        _rankingPageModel = rankingPageModel;
    }

    public void InjectViewer(RankingPageViewer rankingPageViewer)
    {
        _rankingPageViewer = rankingPageViewer;
    }

    public void ActiveContent(bool active)
    {
        _rankingPageModel.ActiveContent = active;
        _rankingPageViewer.ActiveContent(_rankingPageModel.ActiveContent);
    }

    public void AddRakingItems(RankingUI rankingUI)
    {
        _rankingPageViewer.AddRankingItemToScroll(rankingUI);
    }

    public void AddMyRaking(RankingUI rankingUI)
    {
        _rankingPageViewer.AddMyRankingItem(rankingUI);
    }
}
