using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPageViewer
{
    GameObject _content;
    Transform _scrollContent;

    public ShopPageViewer(GameObject content, Transform scrollContent)
    {
        _content = content;
        _scrollContent = scrollContent;
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
    }

    public void AddRankingItemToScroll(SpawnableUI rankingUI)
    {
        rankingUI.transform.SetParent(_scrollContent);
    }
}
