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



    public void ChangeArtwork(Sprite artSprite, Sprite artFrameSprite)
    {
        _model.ArtSprite = artSprite;
        _model.ArtFrameSprite = artFrameSprite;
        _viewer.ChangeArtwork(_model.ArtSprite, _model.ArtFrameSprite);
    }

    public void ChangeRank(Color rankColor, Sprite rankIcon, string rankName)
    {
        _model.RankColor = rankColor;
        _model.RankIcon = rankIcon;
        _model.RankName = rankName;

        _viewer.ChangeRank(_model.RankColor, _model.RankIcon, _model.RankName);
    }

    public void ChangeGetRank(int hintUseCount, int wrongCount)
    {
        _model.HintUseCount = hintUseCount;
        _model.WrongCount = wrongCount;

        _viewer.ChangeGetRank(_model.HintUseCount, _model.WrongCount);
    }

    public void ChangeCollectionRatio(int totalCollectRatio)
    {
        _model.TotalCollectRatio = totalCollectRatio;
        _viewer.ChangeCollectionRatio(_model.TotalCollectRatio);
    }




    public void ActivatePausePanel(bool active)
    {
        if (active)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }

        SaveData data = ServiceLocater.ReturnSaveManager().GetSaveData();
        OnBGMSliderValeChanged(data.BgmVolume);
        OnBGMSliderValeChanged(data.SfxVolume);

        _model.ActivePausePanel = active;
        _viewer.ActivatePausePanel(_model.ActivePausePanel);
    }

    public void OnBGMSliderValeChanged(float ratio)
    {
        _model.BgmRatio = ratio;
        ServiceLocater.ReturnSoundPlayer().SetBGMVolume(ratio);
    }

    public void OnSFXSliderValeChanged(float ratio)
    {
        _model.SfxRatio = ratio;
        ServiceLocater.ReturnSoundPlayer().SetSFXVolume(ratio);
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

    public void ActivateGameClearPanel(bool active)
    {
        _model.ActiveGameClearPanel = active;
        _viewer.ActivateGameClearPanel(_model.ActiveGameClearPanel);
    }

    public void ActivateGameResultPanel(bool active)
    {
        _model.ActiveGameResultPanel = active;
        _viewer.ActivateGameResultPanel(_model.ActiveGameResultPanel);
    }
}
