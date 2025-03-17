using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomePagePresenter
{
    HomePageViewer _homePageViewer;
    HomePageModel _homePageModel;

    public HomePagePresenter(HomePageModel homePageModel)
    {
        _homePageModel = homePageModel;
    }

    public void InjectViewer(HomePageViewer homePageViewer)
    {
        _homePageViewer = homePageViewer;
    }

    public void OnModeTypeChanged(bool isOn)
    {
        if (isOn)
        {
            _homePageModel.ModeType = GameMode.Type.Challenge;
        }
        else
        {
            _homePageModel.ModeType = GameMode.Type.Collect;
        }

        _homePageViewer.ChangeTitleImage(_homePageModel.TitleImages[_homePageModel.ModeType]);
        _homePageViewer.ChangeDotColors(_homePageModel.DotColors[_homePageModel.ModeType]);
        _homePageViewer.ChangePlayBtnColor(_homePageModel.PlayBtnColors[_homePageModel.ModeType]);

    }
}
