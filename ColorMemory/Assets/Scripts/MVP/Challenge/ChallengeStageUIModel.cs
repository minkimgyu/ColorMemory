using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChallengeStageUIModel
{
    public ChallengeStageUIModel()
    {
        _activeStageOverPreviewPanel = false;
        _mapData = default;
        _pickColors = default;

        _activePausePanel = false;
        _bgmRatio = 0;
        _sfxRatio = 0;

        _oneColorHintActive = true;
        _oneZoneHintActive = true;

        _activePenContent = false;
        _activeSkipBtn = false;

        _oneColorHintCost = "";
        _oneZoneHintCost = "";

        _stageCount = 0;

        _activePlayPanel = false;
        _bestScore = 0;
        _nowScore = 0;
        _leftTime = "";
        _totalTime = "";
        _timeRatio = 0;
        _activeRememberPanel = false;
        _activeHintPanel = false;
        _activeGameOverPanel = false;
        _clearStageCount = "";
        _activeGameResultPanel = false;
        _goldCount = "";

        _colorOnBgmHandle = Color.white;
        _colorOnSfxHandle = Color.white;
    }

    Color _colorOnBgmHandle;
    Color _colorOnSfxHandle;

    string _rememberTxt;
    public string RememberTxt { get => _rememberTxt; set => _rememberTxt = value; }


    string _stageOverTitleText;
    string _stageOverInfo1TextFormat;
    string _stageOverInfo2Text;

    public string StageOverTitleText { get => _stageOverTitleText; set => _stageOverTitleText = value; }
    public string StageOverInfo1Text { get => _stageOverInfo1TextFormat; set => _stageOverInfo1TextFormat = value; }
    public string StageOverInfo2Text { get => _stageOverInfo2Text; set => _stageOverInfo2Text = value; }


    public Color ColorOnBgmHandle { get => _colorOnBgmHandle; set => _colorOnBgmHandle = value; }
    public Color ColorOnSfxHandle { get => _colorOnSfxHandle; set => _colorOnSfxHandle = value; }

    string _bgmleftTextInfo;
    public string BgmleftTextInfo { get => _bgmleftTextInfo; set => _bgmleftTextInfo = value; }

    string _sfxleftTextInfo;
    public string SfxleftTextInfo { get => _sfxleftTextInfo; set => _sfxleftTextInfo = value; }


    bool _oneColorHintActive;
    public bool OneColorHintActive { get => _oneColorHintActive; set => _oneColorHintActive = value; }

    bool _oneZoneHintActive;
    public bool OneZoneHintActive { get => _oneZoneHintActive; set => _oneZoneHintActive = value; }



    bool _activePenContent;
    public bool ActiveBottomContent { get => _activePenContent; set => _activePenContent = value; }

    bool _activeSkipBtn;
    public bool ActiveSkipBtn { get => _activeSkipBtn; set => _activeSkipBtn = value; }




    string _oneColorHintCost;
    public string OneColorHintCost { get => _oneColorHintCost; set => _oneColorHintCost = value; }

    string _oneZoneHintCost;
    public string OneZoneHintCost { get => _oneZoneHintCost; set => _oneZoneHintCost = value; }



    bool _activePausePanel;

    public bool ActivePausePanel
    {
        get => _activePausePanel;
        set => _activePausePanel = value;
    }

    string _pauseTitleText;
    public string PauseTitleText 
    { 
        get => _pauseTitleText; 
        set => _pauseTitleText = value; 
    }

    string _gameExitText;
    public string GameExitText
    {
        get => _gameExitText;
        set => _gameExitText = value;
    }

    string _bgmTitleText;
    public string BGMTitleText
    {
        get => _bgmTitleText;
        set => _bgmTitleText = value;
    }

    float _bgmRatio;
    public float BgmRatio
    {
        get => _bgmRatio;
        set => _bgmRatio = value;
    }

    string _bgmRightText;
    public string BgmRightText
    {
        get => _bgmRightText;
        set => _bgmRightText = value;
    }



    string _sfxTitleText;
    public string SfxTitleText
    {
        get => _sfxTitleText;
        set => _sfxTitleText = value;
    }

    float _sfxRatio;
    public float SfxRatio
    {
        get => _sfxRatio;
        set => _sfxRatio = value;
    }

    string _sfxRightText;
    public string SfxRightText
    {
        get => _sfxRightText;
        set => _sfxRightText = value;
    }


    string _soundLeftText;
    public string SoundLeftText
    {
        get => _soundLeftText;
        set => _soundLeftText = value;
    }

    string _soundRightText;
    public string SoundRightText
    {
        get => _soundRightText;
        set => _soundRightText = value;
    }



    bool _activeStageOverPreviewPanel;

    public bool ActiveStageOverPreviewPanel
    {
        get => _activeStageOverPreviewPanel;
        set => _activeStageOverPreviewPanel = value;
    }

    MapData _mapData;

    public MapData MapData 
    { 
        get => _mapData; 
        set => _mapData = value; 
    }

    Color[] _pickColors;

    public Color[] PickColors 
    { 
        get => _pickColors; 
        set => _pickColors = value; 
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


    string _leftTime;
    public string LeftTime
    {
        get => _leftTime;
        set => _leftTime = value;
    }

    string _totalTime;
    public string TotalTime
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

    string _coinCount;
    public string CoinCount
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

    string _resultScore;
    public string ResultScore
    {
        get => _resultScore;
        set => _resultScore = value;
    }

    string _clearStageCount;
    public string ClearStageCount
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

    string _goldCount;
    public string GoldCount
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