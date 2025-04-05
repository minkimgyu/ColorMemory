using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void DestoryShopItems()
    {
        for (int i = _scrollContent.childCount - 1; i >= 0; i--)
        {
            SpawnableUI spawnableUI = _scrollContent.GetChild(i).GetComponent<SpawnableUI>();
            if (spawnableUI == null) continue;

            spawnableUI.DestroyObject();
        }
    }

    public void AddShopItem(SpawnableUI rankingUI)
    {
        rankingUI.transform.SetParent(_scrollContent);
        rankingUI.transform.localScale = Vector3.one;
    }
}
