using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectStageUIModel
{
    public CollectStageUIModel()
    {
        _activePlayPanel = false;
        _hintInfo = "";
        _progress = 0;
        _bestScore = 0;
        _nowScore = 0;
        _leftTime = 0;
        _totalTime = 0;
        _timeRatio = 0;
        _activeRememberPanel = false;
        _activeHintPanel = false;
        _activeGameClearPanel = false;
        _clearStageCount = 0;
        _activeGameResultPanel = false;
        _goldCount = 0;

        _activePausePanel = false;
        _bgmRatio = 0;
        _sfxRatio = 0;


    }

    Sprite _artSprite;
    Sprite _artFrameSprite;
    int _hintUseCount; 
    int _wrongCount;
    int _totalCollectRatio;

    Color _rankColor;
    Sprite _rankIcon;
    string _rankName;


    public Sprite ArtSprite { get => _artSprite; set => _artSprite = value; }
    public Sprite ArtFrameSprite { get => _artFrameSprite; set => _artFrameSprite = value; }
    public int HintUseCount { get => _hintUseCount; set => _hintUseCount = value; }
    public int WrongCount { get => _wrongCount; set => _wrongCount = value; }
    public int TotalCollectRatio { get => _totalCollectRatio; set => _totalCollectRatio = value; }
    public Color RankColor { get => _rankColor; set => _rankColor = value; }
    public Sprite RankIcon { get => _rankIcon; set => _rankIcon = value; }
    public string RankName { get => _rankName; set => _rankName = value; }



    bool _activePausePanel;

    public bool ActivePausePanel
    {
        get => _activePausePanel;
        set => _activePausePanel = value;
    }

    float _bgmRatio;
    public float BgmRatio
    {
        get => _bgmRatio;
        set => _bgmRatio = value;
    }

    float _sfxRatio;
    public float SfxRatio
    {
        get => _sfxRatio;
        set => _sfxRatio = value;
    }



    bool _activePlayPanel;

    public bool ActivePlayPanel
    {
        get => _activePlayPanel;
        set => _activePlayPanel = value;
    }

    string _hintInfo;
    public string HintInfo
    {
        get => _hintInfo;
        set => _hintInfo = value;
    }

    int _progress;
    public int Progress
    {
        get => _progress;
        set => _progress = value;
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

    bool _activeTimerContent;
    public bool ActiveTimerContent
    {
        get => _activeTimerContent;
        set => _activeTimerContent = value;
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

    bool _activeGameClearPanel;

    public bool ActiveGameClearPanel
    {
        get => _activeGameClearPanel;
        set => _activeGameClearPanel = value;
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

    string _title;
    public string Title { get => _title; set => _title = value; }
}
