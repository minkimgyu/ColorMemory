using System.Collections;
using System.Collections.Generic;
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

        _oneColorHintCost = 0;
        _oneZoneHintCost = 0;

        _stageCount = 0;

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

        _colorOnBgmHandle = Color.white;
        _colorOnSfxHandle = Color.white;
    }

    Color _colorOnBgmHandle;
    Color _colorOnSfxHandle;

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




    int _oneColorHintCost;
    public int OneColorHintCost { get => _oneColorHintCost; set => _oneColorHintCost = value; }

    int _oneZoneHintCost;
    public int OneZoneHintCost { get => _oneZoneHintCost; set => _oneZoneHintCost = value; }

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

    bool _activeGameOverPanel;

    public bool ActiveGameOverPanel
    {
        get => _activeGameOverPanel;
        set => _activeGameOverPanel = value;
    }

    int _resultScore;
    public int ResultScore
    {
        get => _resultScore;
        set => _resultScore = value;
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
