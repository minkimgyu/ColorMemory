using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPageViewer
{
    GameObject _content;
    Button _shopAdBtn;
    TMPro.TMP_Text _shopAdBtnTxt;
    Transform _scrollContent;

    public ShopPageViewer(GameObject content, Button shopAdBtn, Transform scrollContent)
    {
        _content = content;
        _shopAdBtn = shopAdBtn;
        _shopAdBtnTxt = _shopAdBtn.GetComponentInChildren<TMPro.TMP_Text>();
        _scrollContent = scrollContent;
    }

    public void ActiveContent(bool active)
    {
        _content.SetActive(active);
    }

    public void ChangeShopAdBtnTxt(string content)
    {
        _shopAdBtnTxt.text = content;
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
