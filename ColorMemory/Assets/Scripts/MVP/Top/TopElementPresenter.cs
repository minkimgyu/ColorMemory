using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TopElementPresenter
{
    TopElementViewer _viewer;
    TopElementModel _model;

    public Action OnClickShopBtn { get; set; }
    public Action OnClickHomeBtn { get; set; }

    public Action OnClickRankingBtn { get; set; }
    public Action OnClickSettingBtn { get; set; }


    public TopElementPresenter(TopElementModel topElementModel)
    {
        _model = topElementModel;
    }

    public void InjectViewer(TopElementViewer viewer)
    {
        _viewer = viewer;
    }

    public void ChangeGoldCount(int goldCount)
    {
        _model.GoldCount = goldCount;
        _viewer.ChangeGoldCount(_model.GoldCount);
    }
}
