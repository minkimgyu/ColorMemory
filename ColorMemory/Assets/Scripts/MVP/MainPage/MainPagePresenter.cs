using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPagePresenter
{
    MainPageViewer _mainPageViewer;
    MainPageModel _mainPageModel;
    public System.Action<GameMode.Type> OnPlayBtnClicked { get; set; }

    public MainPagePresenter(MainPageModel mainPageModel)
    {
        _mainPageModel = mainPageModel;
    }

    public void InjectViewer(MainPageViewer mainPageViewer)
    {
        _mainPageViewer = mainPageViewer;

        // 토글 위치를 초기화해줌
        switch (_mainPageModel.ModeType)
        {
            case GameMode.Type.Collect:
                _mainPageViewer.ChangeToggleState(false);
                break;
            case GameMode.Type.Challenge:
                _mainPageViewer.ChangeToggleState(true);
                break;
            default:
                break;
        }
    }

    public void OnClickPlayBtn()
    {
        OnPlayBtnClicked?.Invoke(_mainPageModel.ModeType);
    }

    public void ActiveContent(bool active)
    {
        _mainPageModel.ActiveContent = active;
        _mainPageViewer.ActiveContent(_mainPageModel.ActiveContent);
    }

    public void OnModeTypeChanged(bool isUIInput, bool isOn)
    {
        if(isUIInput)
        {
            ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.BtnClick);
        }

        if (isOn)
        {
            _mainPageModel.ModeType = GameMode.Type.Challenge;
        }
        else
        {
            _mainPageModel.ModeType = GameMode.Type.Collect;
        }

        ServiceLocater.ReturnSaveManager().ChangeGameModeType(_mainPageModel.ModeType);

        _mainPageViewer.ChangeTitleImage(_mainPageModel.TitleImages[_mainPageModel.ModeType]);
        _mainPageViewer.ChangeDotColors(_mainPageModel.DotColors[_mainPageModel.ModeType]);
        _mainPageViewer.ChangePlayBtnColor(_mainPageModel.PlayBtnColors[_mainPageModel.ModeType]);
        _mainPageViewer.ChangePlayBtnTxt(_mainPageModel.PlayBtnTxt[_mainPageModel.ModeType]);
    }
}
