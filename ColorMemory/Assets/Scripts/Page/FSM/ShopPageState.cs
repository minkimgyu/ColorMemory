using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPageState : BaseState<HomePage.InnerPageState>
{
    ShopPagePresenter _shopPagePresenter;

    public ShopPageState(
        GameObject shopContent,
        Transform scrollContent,
        FSM<HomePage.InnerPageState> fsm) : base(fsm)
    {
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

    public override void OnStateEnter()
    {
        _shopPagePresenter.ActiveContent(true); // home ╢щ╬фаж╠Б
    }

    public override void OnStateExit()
    {
        _shopPagePresenter.ActiveContent(false); // home ╢щ╬фаж╠Б
    }
}
