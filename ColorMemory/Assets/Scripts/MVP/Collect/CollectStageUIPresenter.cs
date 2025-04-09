using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using Unity.VisualScripting;

public class CollectStageUIPresenter
{
    CollectStageUIModel _model;
    CollectStageUIViewer _viewer;

    public CollectStageUIPresenter(CollectStageUIModel model)
    {
        _model = model;
    }

    public void InjectViewer(CollectStageUIViewer viewer)
    {
        _viewer = viewer;
    }



    public void ActivateGameClearPanel(bool active)
    {
        _model.ActiveGameClearPanel = active;
        _viewer.ActivateGameClearPanel(_model.ActiveGameClearPanel);
    }

    public void ChangeClearTitleInfo(string title)
    {
        _model.ClearTitleInfo = title;
        _viewer.ChangeClearTitleText(_model.ClearTitleInfo);
    }

    public void ChangeClearContentInfo(string title)
    {
        _model.ClearContentInfo = title;
        _viewer.ChangeClearContentInfo(_model.ClearContentInfo);
    }

    public void ActivateNextStageBtn(bool active)
    {
        _model.ActiveNextStageBtn = active;
        _viewer.ActivateNextStageBtn(_model.ActiveNextStageBtn);
    }

    public void ActivateClearExitBtn(bool active)
    {
        _model.ActiveClearExitBtn = active;
        _viewer.ActivateClearExitBtn(_model.ActiveClearExitBtn);
    }




    public void ActivateDetailContent(bool active)
    {
        _model.ActiveDetailContent = active;
        _viewer.ActivateDetailContent(_model.ActiveDetailContent);
    }

    public void ChangeCurrentHintUsage(int usage)
    {
        _model.CurrentHintUsage = usage;
        _viewer.ChangeCurrentHintUsage(_model.CurrentHintUsage);
    }

    public void ChangeCurrentWrongCount(int wrongCount)
    {
        _model.CurrentWrongCount = wrongCount;
        _viewer.ChangeCurrentWrongCount(_model.CurrentWrongCount);
    }





    public void ChangeArtwork(Sprite artSprite, Sprite artFrameSprite, Sprite rankDecorationIcon)
    {
        _model.ArtSprite = artSprite;
        _model.ArtFrameSprite = artFrameSprite;
        _model.RankDecorationIconSprite = rankDecorationIcon;
        _viewer.ChangeArtwork(_model.ArtSprite, _model.ArtFrameSprite, _model.RankDecorationIconSprite);
    }

    public void ChangeRank(Sprite rankIcon, string rankName)
    {
        _model.RankIcon = rankIcon;
        _model.RankName = rankName;

        _viewer.ChangeRank(_model.RankIcon, _model.RankName);
    }

    public void ChangeGetRank(int hintUseCount, int wrongCount)
    {
        _model.HintUseCount = hintUseCount;
        _model.WrongCount = wrongCount;

        _viewer.ChangeGetRank(_model.HintUseCount, _model.WrongCount);
    }

    public void ChangeCollectionRatio(float totalCollectRatio)
    {
        _model.TotalCollectRatio = totalCollectRatio;
        _viewer.ChangeCollectionRatio(_model.TotalCollectRatio);
    }

    public void OnClickGameExitBtn()
    {
        // 데이터 저장
        ServiceLocater.ReturnSaveManager().ChangeBGMVolume(_model.BgmRatio);
        ServiceLocater.ReturnSaveManager().ChangeSFXVolume(_model.SfxRatio);

        ServiceLocater.ReturnTimeController().Start();
        ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.HomeScene);
    }

    public void ActivatePausePanel(bool active)
    {
        if (active)
        {
            ServiceLocater.ReturnTimeController().Stop();

            // 데이터 불러와서 반영
            SaveData data = ServiceLocater.ReturnSaveManager().GetSaveData();
            _viewer.ChangeBGMSliderValue(data.BgmVolume, _model.ColorOnBgmHandle, _model.ColorOnZeroValue);
            _viewer.ChangeSFXSliderValue(data.SfxVolume, _model.ColorOnSfxHandle, _model.ColorOnZeroValue);
        }
        else
        {
            ServiceLocater.ReturnTimeController().Start();

            //// 데이터 저장
            //ServiceLocater.ReturnSaveManager().ChangeBGMVolume(_model.BgmRatio);
            //ServiceLocater.ReturnSaveManager().ChangeSFXVolume(_model.SfxRatio);
        }

        _model.ActivePausePanel = active;
        _viewer.ActivatePausePanel(_model.ActivePausePanel);
    }

    public void SaveBGMValue()
    {
        ServiceLocater.ReturnSaveManager().ChangeBGMVolume(_model.BgmRatio);
    }

    public void SaveSFXValue()
    {
        ServiceLocater.ReturnSaveManager().ChangeSFXVolume(_model.SfxRatio);
    }


    public void OnBGMSliderValeChanged(float ratio)
    {
        _model.BgmRatio = ratio;
        _viewer.ChangeBGMSliderHandleColor(_model.BgmRatio, _model.ColorOnBgmHandle, _model.ColorOnZeroValue);
        ServiceLocater.ReturnSoundPlayer().SetBGMVolume(_model.BgmRatio);
    }

    public void OnSFXSliderValeChanged(float ratio)
    {
        _model.SfxRatio = ratio;
        _viewer.ChangeSFXSliderHandleColor(_model.SfxRatio, _model.ColorOnSfxHandle, _model.ColorOnZeroValue);
        ServiceLocater.ReturnSoundPlayer().SetSFXVolume(_model.SfxRatio);
    }

    public void ActivatePlayPanel(bool active)
    {
        _model.ActivePlayPanel = active;
        _viewer.ActivatePlayPanel(_model.ActivePlayPanel);
    }

    public void ChangeHintInfoText(string infoText)
    {
        _model.HintInfo = infoText;
        _viewer.ChangeHintInfoText(_model.HintInfo);
    }

    public void ChangeProgressText(int progress)
    {
        _model.Progress = progress;
        _viewer.ChangeProgressText(_model.Progress);
    }

    public void ChangeTitle(string title)
    {
        _model.Title = title;
        _viewer.ChangeTitle(_model.Title);
    }

    public void FillTimeSlider(float duration)
    {
        DOVirtual.Float(_model.TimeRatio, 1, duration, 
            ((ratio) => 
            { 
                _model.TimeRatio = ratio; 
                _viewer.FillTimeSlider(_model.TimeRatio);
            }
        ));
    }

    public void ChangeTotalTime(float totalTime)
    {
        _model.TotalTime = totalTime;
        _viewer.ChangeTotalTime(_model.TotalTime);
    }

    public void ChangeLeftTime(float leftTime, float ratio)
    {
        _model.LeftTime = leftTime;
        _model.TimeRatio = ratio;

        _viewer.ChangeLeftTime(_model.LeftTime, _model.TimeRatio);
    }

    public void ActivateTimerContent(bool active)
    {
        _model.ActiveTimerContent = active;
        _viewer.ActivateTimerContent(_model.ActiveTimerContent);
    }

    public void ActivateRememberPanel(bool active)
    {
        _model.ActiveRememberPanel = active;
        _viewer.ActivateRememberPanel(_model.ActiveRememberPanel);
    }

   

    public void ActivateGameResultPanel(bool active)
    {
        _model.ActiveGameResultPanel = active;
        _viewer.ActivateGameResultPanel(_model.ActiveGameResultPanel);
    }
}
