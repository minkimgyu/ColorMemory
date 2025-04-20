using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPagePresenter
{
    ShopPageViewer _shopPageViewer;
    ShopPageModel _shopPageModel;

    public ShopPagePresenter(ShopPageModel shopPageModel, ShopPageViewer shopPageViewer)
    {
        _shopPageModel = shopPageModel;
        _shopPageViewer = shopPageViewer;
    }

    public void ActiveContent(bool active)
    {
        _shopPageModel.ActiveContent = active;
        _shopPageViewer.ActiveContent(_shopPageModel.ActiveContent);
    }

    public void DestroyShopItems()
    {
        _shopPageViewer.DestoryShopItems();
    }

    public void AddShopItem(SpawnableUI rankingUI)
    {
        _shopPageViewer.AddShopItem(rankingUI);
    }
}
