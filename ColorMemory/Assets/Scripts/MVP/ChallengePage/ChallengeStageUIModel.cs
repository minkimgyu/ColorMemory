using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeStageUIModel
{
    //string _title;
    //public string Title
    //{
    //    get => _title;
    //    set
    //    {
    //        _title = value;
    //        _titleText.text = _title;
    //    }
    //}

    public ChallengeStageUIModel()
    {
        _leftTime = 0;
        _timeRatio = 0;
        _activeHintPanel = false;
        _activeEndPanel = false;
    }

    float _leftTime;
    public float LeftTime
    {
        get => _leftTime;
        set => _leftTime = value;
    }

    float _totalTime;
    public float TotalTime
    {
        get => _totalTime;
        set => _totalTime = value;
    }

    float _timeRatio;
    public float TimeRatio
    {
        get => _timeRatio;
        set => _timeRatio = value;
    }

    bool _activeRememberPanel;
    public bool ActiveRememberPanel
    {
        get => _activeRememberPanel;
        set => _activeRememberPanel = value;
    }

    bool _activeHintPanel;
    public bool ActiveHintPanel
    {
        get => _activeHintPanel;
        set => _activeHintPanel = value;
    }

    bool _activeEndPanel;

    public bool ActiveEndPanel
    {
        get => _activeEndPanel;
        set => _activeHintPanel = value;
    }
}
