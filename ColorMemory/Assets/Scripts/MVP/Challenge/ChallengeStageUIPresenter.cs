using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using Unity.VisualScripting;

public class ChallengeStageUIPresenter
{
    ChallengeStageUIModel _model;
    ChallengeStageUIViewer _viewer;

    public ChallengeStageUIPresenter(ChallengeStageUIModel model)
    {
        _model = model;
    }

    public void InjectViewer(ChallengeStageUIViewer viewer)
    {
        _viewer = viewer;
    }

    public void ActivatePausePanel(bool active)
    {
        if(active)
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


    public void ActivateStageOverPreviewPanel(bool active)
    {
        _model.ActiveStageOverPreviewPanel = active;
        _viewer.ActivateStageOverPreviewPanel(_model.ActiveStageOverPreviewPanel);
    }

    public void ChangeLastStagePattern(MapData data, Color[] pickColors)
    {
        _model.MapData = data;
        _model.PickColors = pickColors;
        _viewer.ChangeLastStagePattern(_model.StageCount, _model.MapData, _model.PickColors);
    }

    public void ChangeStageOverInfo()
    {
        _viewer.ChangeStageOverInfo(_model.StageCount);
    }




    public void ActivatePlayPanel(bool active)
    {
        _model.ActivePlayPanel = active;
        _viewer.ActivatePlayPanel(_model.ActivePlayPanel);
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

    public void ChangeBestScore(int bestScore)
    {
        _model.BestScore = bestScore;
        _viewer.ChangeBestScore(_model.BestScore);
    }

    public void ChangeNowScore(int nowScore)
    {
        _model.NowScore = nowScore;
        _viewer.ChangeNowScore(_model.NowScore);
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

    public void ChangeStageCount(int stageCount)
    {
        _model.StageCount = stageCount;
        _viewer.ChangeStageCount(_model.StageCount);
    }

    public void ActivateRememberPanel(bool active)
    {
        _model.ActiveRememberPanel = active;
        _viewer.ActivateRememberPanel(_model.ActiveRememberPanel);
    }

    public void ActivateHintPanel(bool active)
    {
        _model.ActiveHintPanel = active;
        _viewer.ActivateHintPanel(_model.ActiveHintPanel);
    }


    public void ActiveGoldPanel(bool active)
    {
        _model.ActiveCoinPanel = active;
        _viewer.ActivateCoinPanel(_model.ActiveCoinPanel);
    }

    public void ChangeCoinCount(int coinCount)
    {
        _model.CoinCount = coinCount;
        _viewer.ChangeCoinCount(coinCount);
    }


    public void ActivateGameOverPanel(bool active)
    {
        _model.ActiveGameOverPanel = active;
        _viewer.ActivateGameOverPanel(_model.ActiveGameOverPanel);
    }

    public void ChangeClearStageCount(int passedDuration, int clearStageCount)
    {
        _model.PassedDuration = passedDuration;
        _model.ClearStageCount = clearStageCount;
        _viewer.ChangeClearStageCount(_model.PassedDuration, _model.ClearStageCount);
    }

    public void AddClearPattern(SpawnableUI clearPattern)
    {
        _viewer.AddClearPattern(clearPattern);
    }

    public void RemoveClearPattern()
    {
        _viewer.RemoveClearPattern();
    }

    public void ActivateGameResultPanel(bool active)
    {
        _model.ActiveGameResultPanel = active;
        _viewer.ActivateGameResultPanel(_model.ActiveGameResultPanel);
    }

    public void ChangeGoldCount(int goldCount)
    {
        _model.GoldCount = goldCount;
        _viewer.ChangeGoldCount(_model.GoldCount);
    }

    public void AddRanking(SpawnableUI ranking, bool setToMiddle = false)
    {
        _viewer.AddRanking(ranking, setToMiddle);
    }

    public void RemoveAllRanking()
    {
        _viewer.RemoveAllRanking();
    }

    public void SetUpRankingScroll(int menuCount, int index)
    {
        _model.MenuCount = menuCount;
        _model.ScrollIndex = index;
        _viewer.ChangeRankingScrollValue(_model.MenuCount, _model.ScrollIndex);
    }
}
