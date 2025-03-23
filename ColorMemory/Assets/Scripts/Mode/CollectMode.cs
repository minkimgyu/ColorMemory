using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Collect
{
    public class CollectMode : GameMode
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

        [SerializeField] Image _timerSlider;

        [SerializeField] TMP_Text _leftTimeText;
        [SerializeField] TMP_Text _totalTimeText;

        [SerializeField] GameObject _hintPanel;
        [SerializeField] GameObject _rememberPanel;

        MapData _mapData;
        Dot[,] _dots;
        Dot[] _colorPenDots;

        public struct Data
        {
            int _myScore;
            int _clearStageCount;
            List<MapData> _stageData;

            public Data(int myScore = 0)
            {
                _myScore = myScore;
                _clearStageCount = 0;
                _stageData = new List<MapData>();
            }

            public int MyScore { get => _myScore; set => _myScore = value; }
            public int ClearStageCount { get => _clearStageCount; set => _clearStageCount = value; }
            public List<MapData> StageData { get => _stageData; }
        }

        CollectMode.Data _collectModeData;

        public enum State
        {
            Initialize,
            Memorize,
            Paint,
            Clear,
            Result,
        }

        FSM<State> _fsm;

    //    void DestroyDots()
    //    {
    //        for (int i = 0; i < _levelSize.x; i++)
    //        {
    //            for (int j = 0; j < _levelSize.y; j++)
    //            {
    //                Destroy(_dots[i, j].gameObject);
    //            }
    //        }

    //        for (int i = 0; i < _colorPenDots.Length; i++)
    //        {
    //            Destroy(_colorPenDots[i].gameObject);
    //        }

    //        _dots = null;
    //    }

    //    void SetLevelData(Dot[,] dots, Dot[] colorPenDots, MapData mapData)
    //    {
    //        _dots = dots;
    //        _colorPenDots = colorPenDots;
    //        _mapData = mapData;
    //    }

    //    Tuple<Dot[,], Dot[], MapData> GetLevelData()
    //    {
    //        return new Tuple<Dot[,], Dot[], MapData>(_dots, _colorPenDots, _mapData);
    //    }

    //    CollectMode.Data GetCollectModeData()
    //    {
    //        return _collectModeData;
    //    }

        public override void Initialize()
        {
    //        AddressableHandler addressableHandler = FindObjectOfType<AddressableHandler>();
    //        if (addressableHandler == null) return;

    //        _collectModeData = new Data(0);

    //        CollectStageUIModel model = new CollectStageUIModel();

    //        CollectStageUIPresenter presenter = new CollectStageUIPresenter(model, OnClickNextBtn, OnClickRetryBtn, OnClickExitBtn);

    //        CollectStageUIViewer viewer = new CollectStageUIViewer(
    //            _bestScoreText,
    //            _nowScoreText,
    //            _timerSlider,
    //            _leftTimeText,
    //            _totalTimeText,
    //            _hintPanel,
    //            _rememberPanel,
    //            _gameOverPanel,
    //            _clearStageCount,
    //            _clearStageContent,
    //            _nextBtn,
    //            _gameResultPanel,
    //            _goldCount,
    //            _resultScore,
    //            _rankingContent,
    //            _tryAgainBtn,
    //            _exitBtn,
    //            presenter);

    //        presenter.InjectViewer(viewer);

    //        _randomHintBtn.onClick.AddListener(() => { _fsm.OnClickRandomFillHint(); });
    //        _revealSameColorHintBtn.onClick.AddListener(() => { _fsm.OnClickRevealSameColorHint(); });

    //        _fsm = new FSM<State>();
    //        Dictionary<State, BaseState<State>> states = new Dictionary<State, BaseState<State>>()
    //    {
    //        {
    //            State.Init,
    //            new InitState
    //            (
    //                _fsm,
    //                _pickColors,
    //                _pickCount,
    //                _levelSize,
    //                new EffectFactory(addressableHandler.EffectAssets),
    //                new DotFactory(addressableHandler.DotAssets),
    //                _dotGridContent,
    //                _penContent,
    //                _penToggleGroup,
    //                presenter,
    //                SetLevelData
    //            )
    //        },

    //        { State.Memorize, new MemorizeState(_fsm, _pickColors, 3f, presenter, GetLevelData) },
    //        { State.Paint, new PaintState(_fsm, _pickColors, 5f, presenter, GetLevelData) },
    //        { State.Clear, new ClearState(_fsm, presenter, GetLevelData, DestroyDots, OnStageClear, GetChallengeModeData) },
    //        { State.GameOver, new GameOverState(_fsm, presenter, _pickColors, clearPatternFactory, GetChallengeModeData) },
    //        { State.GameResult, new GameResultState(_fsm, presenter, GetChallengeModeData) }
    //    };

    //        _fsm.Initialize(states, State.Init);
        }
    }
}