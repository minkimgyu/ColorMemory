using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Challenge
{
    public class ChallengeMode : GameMode
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
        [SerializeField] Transform _clearStageContent;
        [SerializeField] Button _nextBtn;

        [SerializeField] GameObject _gameResultPanel;
        [SerializeField] TMP_Text _resultScore;
        [SerializeField] TMP_Text _goldCount;

        [SerializeField] RankingScrollUI _rankingScrollUI;
        [SerializeField] Transform _rankingContent;
        [SerializeField] Button _tryAgainBtn;
        [SerializeField] Button _exitBtn;

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

        ChallengeMode.Data _modeData;

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
            _modeData.StageData.Add(mapData);
        }

        Tuple<Dot[,], Dot[], MapData> GetLevelData()
        {
            return new Tuple<Dot[,], Dot[], MapData>(_dots, _colorPenDots, _mapData);
        }

        ChallengeMode.Data GetChallengeModeData()
        {
            return _modeData;
        }

        const int clearPoint = 100;
        const int pointFixedWeight = 1;

        void OnStageClear(PaintState.Data data)
        {
            float weight = pointFixedWeight + data.LeftDurationRatio;
            _modeData.MyScore += Mathf.RoundToInt(clearPoint * weight);
            _modeData.ClearStageCount += 1;
        }

        // state는 여기에 추가
        public enum State
        {
            Initialize,
            Memorize,
            Paint,
            StageClear,
            GameOver,
            Result,
        }

        FSM<State> _fsm;

        private void Update()
        {
            _fsm.OnUpdate();
        }

        void OnClickNextBtn()
        {
            _fsm.SetState(State.Result);
        }

        void OnClickRetryBtn()
        {
            ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.ChallengeScene);
        }

        void OnClickExitBtn()
        {
            ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.HomeScene);
        }

        public override void Initialize()
        {
            AddressableHandler addressableHandler = FindObjectOfType<AddressableHandler>();
            if (addressableHandler == null) return;

            ClearPatternUIFactory clearPatternUIFactory = new ClearPatternUIFactory(
                addressableHandler.SpawnableUIAssets[SpawnableUI.Name.ClearPatternUI]);

            RankingUIFactory rankingFactory = new RankingUIFactory(
                addressableHandler.SpawnableUIAssets[SpawnableUI.Name.RankingUI],
                addressableHandler.RankingIconAssets);

            _modeData = new Data(0);

            ChallengeStageUIModel model = new ChallengeStageUIModel();

            ChallengeStageUIPresenter presenter = new ChallengeStageUIPresenter(model, OnClickNextBtn, OnClickRetryBtn, OnClickExitBtn);

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
                _nextBtn,
                _gameResultPanel,
                _resultScore,
                _goldCount,
                _rankingScrollUI,
                _rankingContent,
                _tryAgainBtn,
                _exitBtn,
                presenter);

            presenter.InjectViewer(viewer);

            _randomHintBtn.onClick.AddListener(() => { _fsm.OnClickRandomFillHint(); });
            _revealSameColorHintBtn.onClick.AddListener(() => { _fsm.OnClickRevealSameColorHint(); });

            _fsm = new FSM<State>();
            Dictionary<State, BaseState<State>> states = new Dictionary<State, BaseState<State>>()
        {
            {
                State.Initialize,
                new InitializeState
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

            { State.Memorize, new MemorizeState(_fsm, _pickColors, 3f, presenter, GetLevelData) },
            { State.Paint, new PaintState(_fsm, _pickColors, 5f, presenter, GetLevelData) },
            { State.StageClear, new StageClearState(_fsm, presenter, GetLevelData, DestroyDots, OnStageClear, GetChallengeModeData) },
            { State.GameOver, new GameOverState(_fsm, presenter, _pickColors, clearPatternUIFactory, GetChallengeModeData) },
            { State.Result, new ResultState(_fsm, rankingFactory, presenter, GetChallengeModeData) }
        };

            _fsm.Initialize(states, State.Initialize);
        }
    }
}