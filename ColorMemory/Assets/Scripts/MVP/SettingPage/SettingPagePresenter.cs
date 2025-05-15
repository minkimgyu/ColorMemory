using NetworkService.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

public class SettingPagePresenter
{
    SettingPageModel _model;
    SettingPageViewer _viewer;

    IProfileService _iconService;

    public SettingPagePresenter(SettingPageModel settingPageModel, IProfileService iconService)
    {
        _model = settingPageModel;
        _iconService = iconService;
    }

    public void InjectViewer(SettingPageViewer settingPageViewer)
    {
        _viewer = settingPageViewer;

        _model.BGMTitleText = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.BGMTitle);
        _model.SfxTitleText = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.SFXTitle);
        _model.SoundLeftText = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.DecreaseSound);
        _model.SoundRightText = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.IncreaseSound);
        _viewer.ChangeSoundText(_model.BGMTitleText, _model.SfxTitleText, _model.SoundLeftText, _model.SoundRightText);
    }

    public void ChangeName(string name)
    {
        _model.Name = name;
        _viewer.ChangeName(name);
    }

    public void OnPanelActivated(bool active)
    {
        if(active == true)
        {
            SaveData data = ServiceLocater.ReturnSaveManager().GetSaveData();

            ChangeBGMModel(data.BgmVolume);
            ChangeSFXModel(data.SfxVolume);

            _viewer.ChangeBGMSliderValue(data.BgmVolume, _model.BgmleftTextInfo, _model.ColorOnBgmHandle);
            _viewer.ChangeSFXSliderValue(data.SfxVolume, _model.SfxleftTextInfo, _model.ColorOnSfxHandle);
        }
    }

    readonly Color _colorOnZeroValue = new Color(118f / 255f, 113f / 255f, 111f / 255f);
    readonly Color _colorOnBgmHandle = new Color(113f / 255f, 191f / 255f, 255f / 255f);
    readonly Color _colorOnSfxHandle = new Color(255f / 255f, 154f / 255f, 145f / 255f);

    void ChangeBGMModel(float volumn)
    {
        if (volumn == 0)
        {
            _model.BgmleftTextInfo = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.Mute);
            _model.ColorOnBgmHandle = _colorOnZeroValue;
        }
        else
        {
            _model.BgmleftTextInfo = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.DecreaseSound);
            _model.ColorOnBgmHandle = _colorOnBgmHandle;
        }
    }

    void ChangeSFXModel(float volumn)
    {
        if (volumn == 0)
        {
            _model.SfxleftTextInfo = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.Mute);
            _model.ColorOnSfxHandle = _colorOnZeroValue;
        }
        else
        {
            _model.SfxleftTextInfo = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.DecreaseSound);
            _model.ColorOnSfxHandle = _colorOnSfxHandle;
        }
    }

    public void OnBGMSliderValeChanged(float ratio)
    {
        _model.BgmRatio = ratio;

        ChangeBGMModel(_model.BgmRatio);
        _viewer.ChangeBGMSliderHandleColor(_model.BgmleftTextInfo, _model.ColorOnBgmHandle);
        ServiceLocater.ReturnSoundPlayer().SetBGMVolume(_model.BgmRatio);
    }

    public void OnSFXSliderValeChanged(float ratio)
    {
        _model.SfxRatio = ratio;

        ChangeSFXModel(_model.SfxRatio);
        _viewer.ChangeSFXSliderHandleColor(_model.SfxleftTextInfo, _model.ColorOnSfxHandle);
        ServiceLocater.ReturnSoundPlayer().SetSFXVolume(_model.SfxRatio);
    }

    public void ActivateTogglePanel(int index)
    {
        _model.CurrentToggleIndex = index;
        _viewer.ActivateTogglePanel(_model.CurrentToggleIndex);
    }

    public void SaveBGMValue()
    {
        ServiceLocater.ReturnSaveManager().ChangeBGMVolume(_model.BgmRatio);
    }

    public void SaveSFXValue()
    {
        ServiceLocater.ReturnSaveManager().ChangeSFXVolume(_model.SfxRatio);
    }

    public async void OnProfileDone()
    {
        _model.ProfileIndex = _model.SelectedProfileIndex;
        SaveData data = ServiceLocater.ReturnSaveManager().GetSaveData();

        bool isSuccess = await _iconService.SetPlayerIconId(data.UserId, data.UserName, _model.ProfileIndex);
        if (isSuccess == false) return;
    }

    public async void ChangeProfileImgFromServer()
    {
        string userId = ServiceLocater.ReturnSaveManager().GetSaveData().UserId;
        int index = await _iconService.GetPlayerIconId(userId);
        _model.ProfileIndex = index;
        _viewer.ChangeProfileImg(_model.ProfileSprites[_model.ProfileIndex]);
        _viewer.ChangeProfileToggle(index);
    }

    public void OnProfileSelected(bool on, int i)
    {
        if(on)
        {
            _model.SelectedProfileIndex = i;
            _viewer.ChangeProfileImg(_model.ProfileSprites[_model.SelectedProfileIndex]);
        }
    }
}
