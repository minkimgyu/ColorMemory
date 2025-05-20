using NetworkService.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ShopPageState : BaseState<HomePage.InnerPageState>
{
    ShopPagePresenter _shopPagePresenter;
    ShopBundleUIFactory _shopBundleUIFactory;

    TopElementPresenter _topElementPresenter;
    //MoneyManager _moneyManager;

    IAssetService _currencyService;

    public ShopPageState(
        GameObject shopContent,
        Button shopAdBtn,
        Transform scrollContent,
        ShopBundleUIFactory shopBundleUIFactory,
        TopElementPresenter topElementPresenter,
        IAssetService currencyService,
        FSM<HomePage.InnerPageState> fsm) : base(fsm)
    {
        _shopBundleUIFactory = shopBundleUIFactory;
        _topElementPresenter = topElementPresenter;

        _currencyService = currencyService;

        ShopPageModel shopPageModel = new ShopPageModel();
        ShopPageViewer shopPageViewer = new ShopPageViewer(shopContent, shopAdBtn, scrollContent);
        _shopPagePresenter = new ShopPagePresenter(shopPageModel, shopPageViewer);
        _shopPagePresenter.ActiveContent(false);
    }

    async void BuyItemFromServer(int earnMoney)
    {
        string userId = ServiceLocater.ReturnSaveManager().GetSaveData().UserId;

        bool canEarn = await _currencyService.EarnPlayerMoneyAsync(userId, earnMoney);
        if (canEarn == false) return;

        int money = await _currencyService.GetCurrency(userId);
        _topElementPresenter.ChangeGoldCount(money);
    }

    public override void ChangeLanguage()
    {
        OnStateExit();
        OnStateEnter();
    }

    public override void OnStateEnter()
    {
        string title1 = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.ShopBundle1Title);
        string content1 = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.ShopBundle1Content);

        string title2 = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.ShopBundle2Title);
        string content2 = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.ShopBundle2Content);

        string title3 = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.ShopBundle3Title);
        string content3 = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.ShopBundle3Content);

        SpawnableUI spawnableUI1 = _shopBundleUIFactory.Create(title1, content1, 1000, 2500);
        SpawnableUI spawnableUI2 = _shopBundleUIFactory.Create(title2, content2, 2000, 4500);
        SpawnableUI spawnableUI3 = _shopBundleUIFactory.Create(title3, content3, 4000, 8000);

        spawnableUI1.InjectClickEvent(() => { BuyItemFromServer(1000); });
        spawnableUI2.InjectClickEvent(() => { BuyItemFromServer(2000); });
        spawnableUI3.InjectClickEvent(() => { BuyItemFromServer(4000); });

        _shopPagePresenter.AddShopItem(spawnableUI1);
        _shopPagePresenter.AddShopItem(spawnableUI2);
        _shopPagePresenter.AddShopItem(spawnableUI3);

        string shopAdBtnInfo = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.ShopAdBtnInfo);
        _shopPagePresenter.ChangeShopAdBtnTxt(shopAdBtnInfo);

        _shopPagePresenter.ActiveContent(true); // home ╢щ╬фаж╠Б
    }

    public override void OnStateExit()
    {
        _shopPagePresenter.DestroyShopItems();
        _shopPagePresenter.ActiveContent(false); // home ╢щ╬фаж╠Б
    }
}
