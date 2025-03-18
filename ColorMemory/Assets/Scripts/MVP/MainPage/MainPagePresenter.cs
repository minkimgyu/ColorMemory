using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPagePresenter
{
    MainPageViewer _mainPageViewer;
    MainPageModel _mainPageModel;

    public MainPagePresenter(MainPageModel mainPageModel)
    {
        _mainPageModel = mainPageModel;
    }

    public void InjectViewer(MainPageViewer mainPageViewer)
    {
        _mainPageViewer = mainPageViewer;
    }

    public void ActiveContent(bool active)
    {
        _mainPageModel.ActiveContent = active;
        _mainPageViewer.ActiveContent(_mainPageModel.ActiveContent);
    }

    public void OnModeTypeChanged(bool isOn)
    {
        if (isOn)
        {
            _mainPageModel.ModeType = GameMode.Type.Challenge;
        }
        else
        {
            _mainPageModel.ModeType = GameMode.Type.Collect;
        }

        _mainPageViewer.ChangeTitleImage(_mainPageModel.TitleImages[_mainPageModel.ModeType]);
        _mainPageViewer.ChangeDotColors(_mainPageModel.DotColors[_mainPageModel.ModeType]);
        _mainPageViewer.ChangePlayBtnColor(_mainPageModel.PlayBtnColors[_mainPageModel.ModeType]);

    }
}
