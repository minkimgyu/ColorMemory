using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingPageViewer
{
    GameObject _content;
    Transform _scrollContent;
    Transform _myRankingContent;

    public RankingPageViewer(GameObject content, Transform scrollContent, Transform myRankingContent)
    {
        _content = content;
        _scrollContent = scrollContent;
        _myRankingContent = myRankingContent;
    }

    public void ActiveContent(bool active)
    {
        _content.SetActive(active);
    }

    public void AddRankingItemToScroll(RankingUI rankingUI)
    {
        rankingUI.transform.SetParent(_scrollContent);
    }

    public void AddMyRankingItem(RankingUI rankingUI)
    {
        rankingUI.transform.SetParent(_myRankingContent);
        rankingUI.ResetPosition();
    }
}
