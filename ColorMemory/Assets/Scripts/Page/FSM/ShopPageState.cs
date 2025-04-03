using NetworkService.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ShopPageState : BaseState<HomePage.InnerPageState>
{
    ShopPagePresenter _shopPagePresenter;
    ShopBundleUIFactory _shopBundleUIFactory;

    TopElementPresenter _topElementPresenter;
    MoneyManager _moneyManager;

    public ShopPageState(
        GameObject shopContent,
        Transform scrollContent,
        ShopBundleUIFactory shopBundleUIFactory,
        TopElementPresenter topElementPresenter,
        FSM<HomePage.InnerPageState> fsm) : base(fsm)
    {
        _shopBundleUIFactory = shopBundleUIFactory;
        _topElementPresenter = topElementPresenter;

        _moneyManager = new MoneyManager();

        ShopPageModel shopPageModel = new ShopPageModel();
        ShopPageViewer shopPageViewer = new ShopPageViewer(shopContent, scrollContent);
        _shopPagePresenter = new ShopPagePresenter(shopPageModel, shopPageViewer);
        _shopPagePresenter.ActiveContent(false);
    }

    public override void OnClickHomeBtn()
    {
        _fsm.SetState(HomePage.InnerPageState.Main);
    }

    public override void OnClickRankingBtn()
    {
        _fsm.SetState(HomePage.InnerPageState.Ranking);
    }

    async void BuyItemFromServer(int earnMoney)
    {
        bool canEarn = false;

        try
        {
            canEarn = await _moneyManager.EarnPlayerMoneyAsync("testId1", earnMoney);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            Debug.Log("서버로부터 데이터를 받아오지 못함");
            return;
        }

        if (canEarn == false) return;
        int money;

        try
        {
            money = await _moneyManager.GetMoneyAsync("testId1");
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            Debug.Log("서버로부터 데이터를 받아오지 못함");
            return;
        }

        _topElementPresenter.ChangeGoldCount(money);
    }

    public override void OnStateEnter()
    {
        SpawnableUI spawnableUI1 = _shopBundleUIFactory.Create("번들1", "가장 저렴한 초보자 코인 상품!", 1000, 2500);
        SpawnableUI spawnableUI2 = _shopBundleUIFactory.Create("번들2", "번들 1보다 조금 더~\n주는 코인 세트", 2000, 4500);
        SpawnableUI spawnableUI3 = _shopBundleUIFactory.Create("번들3", "코인 쓰고 챌린지 1등 해보자!", 4000, 8000);

        spawnableUI1.InjectClickEvent(() => { BuyItemFromServer(1000); });
        spawnableUI2.InjectClickEvent(() => { BuyItemFromServer(2000); });
        spawnableUI3.InjectClickEvent(() => { BuyItemFromServer(4000); });

        _shopPagePresenter.AddShopItem(spawnableUI1);
        _shopPagePresenter.AddShopItem(spawnableUI2);
        _shopPagePresenter.AddShopItem(spawnableUI3);

        _shopPagePresenter.ActiveContent(true); // home 닫아주기
    }

    public override void OnStateExit()
    {
        _shopPagePresenter.DestroyShopItems();
        _shopPagePresenter.ActiveContent(false); // home 닫아주기
    }
}
