using NetworkService.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

public class SettingPagePresenter
{
    SettingPageModel _settingPageModel;
    SettingPageViewer _settingPageViewer;

    public SettingPagePresenter(SettingPageModel settingPageModel)
    {
        _settingPageModel = settingPageModel;
    }

    public void InjectViewer(SettingPageViewer settingPageViewer)
    {
        _settingPageViewer = settingPageViewer;
    }

    public void ChangeName(string name)
    {
        _settingPageModel.Name = name;
        _settingPageViewer.ChangeName(name);
    }

    public void OnPanelActivated(bool active)
    {
        if(active == true)
        {
            SaveData data = ServiceLocater.ReturnSaveManager().GetSaveData();
            OnBGMSliderValeChanged(data.BgmVolume);
            OnSFXSliderValeChanged(data.SfxVolume);
        }
        else
        {
            ServiceLocater.ReturnSaveManager().ChangeBGMVolume(_settingPageModel.BgmRatio);
            ServiceLocater.ReturnSaveManager().ChangeSFXVolume(_settingPageModel.SfxRatio);
        }
    }

    public void ActivateTogglePanel(int index)
    {
        _settingPageModel.CurrentToggleIndex = index;
        _settingPageViewer.ActivateTogglePanel(_settingPageModel.CurrentToggleIndex);
    }

    public void OnBGMSliderValeChanged(float ratio)
    {
        _settingPageModel.BgmRatio = ratio;
        ServiceLocater.ReturnSoundPlayer().SetBGMVolume(ratio);
    }

    public void OnSFXSliderValeChanged(float ratio)
    {
        _settingPageModel.SfxRatio = ratio;
        ServiceLocater.ReturnSoundPlayer().SetSFXVolume(ratio);
    }

    public async void OnProfileDone()
    {
        _settingPageModel.ProfileIndex = _settingPageModel.SelectedProfileIndex;
        bool isSuccess = await SendPlayerIconToServer();
        if (isSuccess == false) return;

        _settingPageViewer.ChangeProfileImg(_settingPageModel.ProfileSprites[_settingPageModel.ProfileIndex]);
    }

    //public async void ChangeProfileImgFromServer()
    //{
    //    int index = await GetPlayerIconFromServer();
    //    _settingPageModel.ProfileIndex = index;
    //    _settingPageViewer.ChangeProfileImg(_settingPageModel.ProfileSprites[_settingPageModel.ProfileIndex]);
    //}

    //async Task<bool> GetPlayerIconFromServer()
    //{
    //    PlayerManager playerManager = new PlayerManager();
    //    bool isSuccess = false;

    //    try
    //    {
    //        isSuccess = await playerManager.GetPlayerIconIdAsync("testId1", "meal", _settingPageModel.ProfileIndex);
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.Log(e);
    //        Debug.Log("서버로 데이터를 전달할 수 없음");
    //        return false;
    //    }

    //    return isSuccess;
    //}

    async Task<bool> SendPlayerIconToServer()
    {
        PlayerManager playerManager = new PlayerManager();
        bool isSuccess = false;

        try
        {
            isSuccess = await playerManager.SetPlayerIconIdAsync("testId1", "meal", _settingPageModel.ProfileIndex);
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
        if(on) _settingPageModel.SelectedProfileIndex = i;
    }
}
