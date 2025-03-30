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

    public CollectStageUIPresenter(CollectStageUIModel model, CollectStageUIViewer viewer)
    {
        _model = model;
        _viewer = viewer;
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

    public void ChangeGoldCount(int goldCount)
    {
        _model.GoldCount = goldCount;
        _viewer.ChangeGoldCount(_model.GoldCount);
    }
}
