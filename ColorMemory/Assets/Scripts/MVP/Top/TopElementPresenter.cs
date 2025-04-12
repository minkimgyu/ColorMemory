using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopElementPresenter
{
    TopElementViewer _topElementViewer;
    TopElementModel _topElementModel;

    public TopElementPresenter(TopElementViewer topElementViewer, TopElementModel topElementModel)
    {
        _topElementViewer = topElementViewer;
        _topElementModel = topElementModel;
    }

    public void ChangeGoldCount(int goldCount)
    {
        _topElementModel.GoldCount = goldCount;
        _topElementViewer.ChangeGoldCount(_topElementModel.GoldCount);
    }
}
