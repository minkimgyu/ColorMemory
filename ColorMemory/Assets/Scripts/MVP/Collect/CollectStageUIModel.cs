using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectStageUIModel
{
    public CollectStageUIModel()
    {
        _bestScore = 0;
        _nowScore = 0;
        _leftTime = 0;
        _totalTime = 0;
        _timeRatio = 0;
        _activeRememberPanel = false;
        _activeHintPanel = false;
        _activeGameOverPanel = false;
        _clearStageCount = 0;
        _activeGameResultPanel = false;
        _goldCount = 0;
    }

    int _bestScore;
    public int BestScore
    {
        get => _bestScore;
        set => _bestScore = value;
    }

    int _nowScore;
    public int NowScore
    {
        get => _nowScore;
        set => _nowScore = value;
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

    bool _activeGameOverPanel;

    public bool ActiveGameOverPanel
    {
        get => _activeGameOverPanel;
        set => _activeGameOverPanel = value;
    }

    int _clearStageCount;
    public int ClearStageCount
    {
        get => _clearStageCount;
        set => _clearStageCount = value;
    }

    bool _activeGameResultPanel;

    public bool ActiveGameResultPanel
    {
        get => _activeGameResultPanel;
        set => _activeGameResultPanel = value;
    }

    int _resultScore;
    public int ResultScore
    {
        get => _resultScore;
        set => _resultScore = value;
    }

    int _goldCount;
    public int GoldCount
    {
        get => _goldCount;
        set => _goldCount = value;
    }
}
