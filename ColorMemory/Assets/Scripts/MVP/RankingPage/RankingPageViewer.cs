using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingPageViewer
{
    GameObject _content;
    Transform _scrollContent;

    public RankingPageViewer(GameObject content, Transform scrollContent)
    {
        _content = content;
        _scrollContent = scrollContent;
    }

    public void ActiveContent(bool active)
    {
        _content.SetActive(active);
    }

    public void AddItem(RankingUI rankingUI)
    {
        rankingUI.transform.SetParent(_scrollContent);
    }
}
