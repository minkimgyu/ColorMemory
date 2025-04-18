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

        _gameResultTitle = "";

        _activePenContent = false;
        _activeSkipBtn = false;

        _activeNextStageBtn = false;

        _activePausePanel = false;
        _bgmRatio = 0;
        _sfxRatio = 0;

        _activeDetailContent = false;
        _currentHintUsage = 0;
        _currentWrongCount = 0;

        _colorOnBgmHandle = Color.white;
        _colorOnSfxHandle = Color.white;
    }

    string _gameResultTitle;
    public string GameResultTitle { get => _gameResultTitle; set => _gameResultTitle = value; }

    bool _activeDetailContent;
    int _currentHintUsage;
    int _currentWrongCount;

    public bool ActiveDetailContent { get => _activeDetailContent; set => _activeDetailContent = value; }
    public int CurrentHintUsage { get => _currentHintUsage; set => _currentHintUsage = value; }
    public int CurrentWrongCount { get => _currentWrongCount; set => _currentWrongCount = value; }


    bool _activePenContent;
    public bool ActiveBottomContent { get => _activePenContent; set => _activePenContent = value; }

    bool _activeSkipBtn;
    public bool ActiveSkipBtn { get => _activeSkipBtn; set => _activeSkipBtn = value; }


    Sprite _artSprite;
    Sprite _artFrameSprite;
    Sprite _rankDecorationIconSprite;
    bool _isLock;

    int _hintUseCount; 
    int _wrongCount;
    float _currentCollectRatio;
    float _totalCollectRatio;

    Color _rankBackgroundColor;
    Sprite _rankIcon;
    bool _activeIcon;
    string _rankName;

    public Color RankBackgroundColor { get => _rankBackgroundColor; set => _rankBackgroundColor = value; }


    Color _colorOnBgmHandle;
    Color _colorOnSfxHandle;

    string _bgmleftTextInfo;
    public string BgmleftTextInfo { get => _bgmleftTextInfo; set => _bgmleftTextInfo = value; }

    string _sfxleftTextInfo;
    public string SfxleftTextInfo { get => _sfxleftTextInfo; set => _sfxleftTextInfo = value; }


    public Color ColorOnBgmHandle { get => _colorOnBgmHandle; set => _colorOnBgmHandle = value; }
    public Color ColorOnSfxHandle { get => _colorOnSfxHandle; set => _colorOnSfxHandle = value; }


    public Sprite ArtSprite { get => _artSprite; set => _artSprite = value; }
    public Sprite ArtFrameSprite { get => _artFrameSprite; set => _artFrameSprite = value; }
    public Sprite RankDecorationIconSprite { get => _rankDecorationIconSprite; set => _rankDecorationIconSprite = value; }
    public bool HasIt { get => _isLock; set => _isLock = value; }


    public int HintUseCount { get => _hintUseCount; set => _hintUseCount = value; }
    public int WrongCount { get => _wrongCount; set => _wrongCount = value; }
    public float CurrentCollectRatio { get => _currentCollectRatio; set => _currentCollectRatio = value; }
    public float TotalCollectRatio { get => _totalCollectRatio; set => _totalCollectRatio = value; }
    public Sprite RankIcon { get => _rankIcon; set => _rankIcon = value; }
    public bool ActiveIcon { get => _activeIcon; set => _activeIcon = value; }
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

    string _clearTitleInfo;
    public string ClearTitleInfo
    {
        get => _clearTitleInfo;
        set => _clearTitleInfo = value;
    }

    string _clearContentInfo;
    public string ClearContentInfo
    {
        get => _clearContentInfo;
        set => _clearContentInfo = value;
    }


    bool _activeNextStageBtn;
    public bool ActiveNextStageBtn 
    { 
        get => _activeNextStageBtn; 
        set => _activeNextStageBtn = value; 
    }

    bool _activeClearExitBtn;
    public bool ActiveClearExitBtn
    {
        get => _activeClearExitBtn;
        set => _activeClearExitBtn = value;
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
