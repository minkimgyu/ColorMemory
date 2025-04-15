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

    public SettingPagePresenter(SettingPageModel settingPageModel)
    {
        _model = settingPageModel;
    }

    public void InjectViewer(SettingPageViewer settingPageViewer)
    {
        _viewer = settingPageViewer;
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
        //else
        //{
        //    ServiceLocater.ReturnSaveManager().ChangeBGMVolume(_settingPageModel.BgmRatio);
        //    ServiceLocater.ReturnSaveManager().ChangeSFXVolume(_settingPageModel.SfxRatio);
        //}
    }

    readonly Color _colorOnZeroValue = new Color(118f / 255f, 113f / 255f, 111f / 255f);
    readonly Color _colorOnBgmHandle = new Color(113f / 255f, 191f / 255f, 255f / 255f);
    readonly Color _colorOnSfxHandle = new Color(255f / 255f, 154f / 255f, 145f / 255f);

    void ChangeBGMModel(float volumn)
    {
        if (volumn == 0)
        {
            _model.BgmleftTextInfo = "음소거";
            _model.ColorOnBgmHandle = _colorOnZeroValue;
        }
        else
        {
            _model.BgmleftTextInfo = "작게";
            _model.ColorOnBgmHandle = _colorOnBgmHandle;
        }
    }

    void ChangeSFXModel(float volumn)
    {
        if (volumn == 0)
        {
            _model.SfxleftTextInfo = "음소거";
            _model.ColorOnSfxHandle = _colorOnZeroValue;
        }
        else
        {
            _model.SfxleftTextInfo = "작게";
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
        bool isSuccess = await SendPlayerIconToServer();
        if (isSuccess == false) return;
    }

    public async void ChangeProfileImgFromServer()
    {
        int index = await GetPlayerIconFromServer();
        _model.ProfileIndex = index;
        _viewer.ChangeProfileImg(_model.ProfileSprites[_model.ProfileIndex]);
        _viewer.ChangeProfileToggle(index);
    }

    async Task<int> GetPlayerIconFromServer()
    {
        PlayerManager playerManager = new PlayerManager();
        int iconIndex = -1;

        try
        {
            string userId = ServiceLocater.ReturnSaveManager().GetSaveData().UserId;
            iconIndex = await playerManager.GetPlayerIconIdAsync(userId);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            Debug.Log("서버로 데이터를 전달할 수 없음");
            return -1;
        }

        return iconIndex;
    }

    async Task<bool> SendPlayerIconToServer()
    {
        PlayerManager playerManager = new PlayerManager();
        bool isSuccess = false;

        try
        {
            SaveData data = ServiceLocater.ReturnSaveManager().GetSaveData();
            isSuccess = await playerManager.SetPlayerIconIdAsync(data.UserId, data.UserName, _model.ProfileIndex);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            Debug.Log("서버로 데이터를 전달할 수 없음");
            return false;
        }

        return isSuccess;
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
