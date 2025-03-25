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

    public void DestoryRankingItems()
    {
        for (int i = 0; i < _scrollContent.childCount; i++)
        {
            _scrollContent.GetChild(i--).GetComponent<RankingUI>().DestroyObject();
        }

        for (int i = 0; i < _myRankingContent.childCount; i++)
        {
            _myRankingContent.GetChild(i--).GetComponent<RankingUI>().DestroyObject();
        }
    }

    public void AddRankingItemToScroll(SpawnableUI rankingUI)
    {
        rankingUI.transform.SetParent(_scrollContent);
    }

    public void AddMyRankingItem(SpawnableUI rankingUI)
    {
        rankingUI.transform.SetParent(_myRankingContent);
        rankingUI.ResetPosition();
    }
}
