using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeStageUIModel
{
    public ChallengeStageUIModel()
    {
        _activePlayPanel = false;
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

    bool _activePlayPanel;

    public bool ActivePlayPanel
    {
        get => _activePlayPanel;
        set => _activePlayPanel = value;
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

    int _stageCount;
    public int StageCount
    {
        get => _stageCount;
        set => _stageCount = value;
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




    bool _activeCoinPanel;
    public bool ActiveCoinPanel
    {
        get => _activeCoinPanel;
        set => _activeCoinPanel = value;
    }

    int _coinCount;
    public int CoinCount
    {
        get => _coinCount;
        set => _coinCount = value;
    }





    bool _activeGameOverPanel;

    public bool ActiveGameOverPanel
    {
        get => _activeGameOverPanel;
        set => _activeGameOverPanel = value;
    }

    int _passedDuration;
    public int PassedDuration
    {
        get => _passedDuration;
        set => _passedDuration = value;
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

    int _goldCount;
    public int GoldCount
    {
        get => _goldCount;
        set => _goldCount = value;
    }

    int _menuCount;
    public int MenuCount
    {
        get => _menuCount;
        set => _menuCount = value;
    }

    int _scrollIndex;
    public int ScrollIndex
    {
        get => _scrollIndex;
        set => _scrollIndex = value;
    }
}
