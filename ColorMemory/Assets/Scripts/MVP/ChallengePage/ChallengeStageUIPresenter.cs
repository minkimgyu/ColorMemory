using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ChallengeStageUIPresenter
{
    ChallengeStageUIModel _model;
    ChallengeStageUIViewer _viewer;

    public ChallengeStageUIPresenter(
        ChallengeStageUIModel model,
        ChallengeStageUIViewer viewer)
    {
        _model = model;
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

    public void ActivateEndPanel(bool active)
    {
        _model.ActiveEndPanel = active;
        _viewer.ActivateEndPanel(_model.ActiveEndPanel);
    }
}
