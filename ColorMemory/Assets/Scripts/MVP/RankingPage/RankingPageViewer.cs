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
        for (int i = _scrollContent.childCount - 1; i >= 0; i--)
        {
            _scrollContent.GetChild(i).GetComponent<SpawnableUI>().DestroyObject();
        }

        for (int i = _myRankingContent.childCount - 1; i >= 0; i--)
        {
            _myRankingContent.GetChild(i).GetComponent<SpawnableUI>().DestroyObject();
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
