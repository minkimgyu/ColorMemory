using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChallengeStageController : StageController
{
    [SerializeField] ToggleGroup _penToggleGroup;
    [SerializeField] RectTransform _penContent;
    [SerializeField] GridLayoutGroup _dotGridContent;

    [SerializeField] Color[] _pickColors;
    [SerializeField] Vector2 _spacing;
    [SerializeField] int _pickCount;
    [SerializeField] Vector2Int _levelSize = new Vector2Int(5, 5); // row, col


    [SerializeField] Button _randomHintBtn;
    [SerializeField] Button _revealSameColorHintBtn;

    [SerializeField] TMP_Text _bestScoreText;
    [SerializeField] TMP_Text _nowScoreText;

    [SerializeField] Image _timerSlider;

    [SerializeField] TMP_Text _leftTimeText;
    [SerializeField] TMP_Text _totalTimeText;

    //[SerializeField] TMP_Text _titleText;
    [SerializeField] GameObject _hintPanel;
    [SerializeField] GameObject _rememberPanel;

    [SerializeField] GameObject _gameOverPanel;
    [SerializeField] TMP_Text _clearStageCount;
    [SerializeField] GameObject _clearStageContent;

    [SerializeField] GameObject _gameResultPanel;
    [SerializeField] TMP_Text _resultScore;
    [SerializeField] TMP_Text _goldCount;
    [SerializeField] GameObject _rankingContent;
    [SerializeField] Button _tryAgainBtn;
    [SerializeField] Button _exitBtn;

    MapData _mapData;
    Dot[,] _dots;
    Dot[] _colorPenDots;

    public struct Data
    {
        int _myScore;
        int _clearStageCount;
        List<MapData> _clearStageData;

        public Data(int myScore, int clearStageCount, List<MapData> clearStageData)
        {
            _myScore = myScore;
            _clearStageCount = clearStageCount;
            _clearStageData = clearStageData;
        }

        public int MyScore { get => _myScore; set => _myScore = value; }
        public int ClearStageCount { get => _clearStageCount; set => _clearStageCount = value; }
        public List<MapData> ClearStageData { get => _clearStageData; }
    }

    ChallengeStageController.Data _stageControllerData;

    void DestroyDots()
    {
        for (int i = 0; i < _levelSize.x; i++)
        {
            for (int j = 0; j < _levelSize.y; j++)
            {
                Destroy(_dots[i, j].gameObject);
            }
        }

        for (int i = 0; i < _colorPenDots.Length; i++)
        {
            Destroy(_colorPenDots[i].gameObject);
        }

        _dots = null;
    }

    void SetLevelData(Dot[,] dots, Dot[] colorPenDots, MapData mapData)
    {
        _dots = dots;
        _colorPenDots = colorPenDots;
        _mapData = mapData;
        _stageControllerData.ClearStageData.Add(mapData);
    }

    Tuple<Dot[,], Dot[], MapData> GetLevelData() 
    {
        return new Tuple<Dot[,], Dot[], MapData>(_dots, _colorPenDots, _mapData); 
    }

    ChallengeStageController.Data GetChallengeModeData()
    {
        return _stageControllerData;
    }

    const int clearPoint = 100;
    const int pointWeight = 1;

    void OnStageClear(PaintState.Data data)
    {
        _stageControllerData.MyScore += (int)(clearPoint * (pointWeight + data.LeftDurationRatio));
        _stageControllerData.ClearStageCount += 1;
    }

    // state는 여기에 추가
    public enum State
    {
        Init,
        Memorize,
        Paint,
        Clear,
        //End,
        GameOver,
        GameResult,
    }

    FSM<State> _fsm;

    private void Update()
    {
        _fsm.OnUpdate();
    }

    public override void Initialize()
    {
        AddressableHandler addressableHandler = FindObjectOfType<AddressableHandler>();
        if (addressableHandler == null) return;

        ChallengeStageUIModel model = new ChallengeStageUIModel();
        ChallengeStageUIViewer viewer = new ChallengeStageUIViewer(
            _bestScoreText,
            _nowScoreText,
            _timerSlider,
            _leftTimeText,
            _totalTimeText,
            _hintPanel,
            _rememberPanel,
            _gameOverPanel,
            _clearStageCount,
            _clearStageContent,
            _gameResultPanel,
            _goldCount,
            _resultScore,
            _rankingContent,
            _tryAgainBtn,
            _exitBtn);

        ChallengeStageUIPresenter presenter = new ChallengeStageUIPresenter(model, viewer);

        _randomHintBtn.onClick.AddListener(() => { _fsm.OnClickRandomFillHint(); });
        _revealSameColorHintBtn.onClick.AddListener(() => { _fsm.OnClickRevealSameColorHint(); });

        _tryAgainBtn.onClick.AddListener(() => { SceneManager.LoadScene("ChallengeScene"); });
        _exitBtn.onClick.AddListener(() => { SceneManager.LoadScene("StartScene"); });

        _fsm = new FSM<State>();
        Dictionary<State, BaseState<State>> states = new Dictionary<State, BaseState<State>>()
        {
            { 
                State.Init, 
                new InitState
                (
                    _fsm, 
                    _pickColors, 
                    _pickCount,
                    _levelSize, 
                    new EffectFactory(addressableHandler.EffectAssets), 
                    new DotFactory(addressableHandler.DotAssets), 
                    _dotGridContent, 
                    _penContent,
                    _penToggleGroup,
                    presenter,
                    SetLevelData
                ) 
            },

            { State.Memorize, new MemorizeState(_fsm, _pickColors, 7f, presenter, GetLevelData) },
            { State.Paint, new PaintState(_fsm, _pickColors, 10f, presenter, GetLevelData) },
            { State.Clear, new ClearState(_fsm, presenter, GetLevelData, DestroyDots, OnStageClear) },
            { State.GameOver, new GameOverState(_fsm, presenter, _pickColors, GetChallengeModeData) },
            { State.GameResult, new GameResultState(_fsm, presenter, GetChallengeModeData) }
        };

        _fsm.Initialize(states, State.Init);
    }
}
