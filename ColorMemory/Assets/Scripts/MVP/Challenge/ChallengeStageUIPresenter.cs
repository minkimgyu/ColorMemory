using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class ChallengeStageUIPresenter
{
    ChallengeStageUIModel _model;
    ChallengeStageUIViewer _viewer;

    Action OnClickNextBtn;
    Action OnClickRetryBtn;
    Action OnClickExitBtn;

    public ChallengeStageUIPresenter(ChallengeStageUIModel model, Action OnClickNextBtn, Action OnClickRetryBtn, Action OnClickExitBtn)
    {
        _model = model;
        this.OnClickNextBtn = OnClickNextBtn;
        this.OnClickRetryBtn = OnClickRetryBtn;
        this.OnClickExitBtn = OnClickExitBtn;
    }

    public void InjectViewer(ChallengeStageUIViewer viewer)
    {
        _viewer = viewer;
    }

    //public void ChangeTitle(string title)
    //{
    //    _model.Title = title;
    //}

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

    public void ActivateGameOverPanel(bool active)
    {
        _model.ActiveGameOverPanel = active;
        _viewer.ActivateGameOverPanel(_model.ActiveGameOverPanel);
    }

    public void ChangeClearStageCount(int clearStageCount)
    {
        _model.ClearStageCount = clearStageCount;
        _viewer.ChangeClearStageCount(_model.ClearStageCount);
    }

    public void AddClearPattern(SpawnableUI clearPattern)
    {
        _viewer.AddClearPattern(clearPattern);
    }

    public void RemoveClearPattern()
    {
        _viewer.RemoveClearPattern();
    }

    public void OnNextBtnClicked()
    {
        OnClickNextBtn?.Invoke();
    }

    public void OnExitBtnClicked()
    {
        OnClickExitBtn?.Invoke();
    }

    public void OnRetryBtnClicked()
    {
        OnClickRetryBtn?.Invoke();
    }

    public void ActivateGameResultPanel(bool active)
    {
        _model.ActiveGameResultPanel = active;
        _viewer.ActivateGameResultPanel(_model.ActiveGameResultPanel);
    }

    public void ChangeResultScore(int resultScore)
    {
        _model.ResultScore = resultScore;
        _viewer.ChangeResultScore(_model.ResultScore);
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
