using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Challenge
{
    public class ChallengeMode : GameMode
    {
        [Header("Play")]
        [SerializeField] GameObject _playPanel;

        [Header("Top")]
        [SerializeField] TMP_Text _bestScoreText;
        [SerializeField] TMP_Text _nowScoreText;

        [SerializeField] Image _timerSlider;
        [SerializeField] TMP_Text _leftTimeText;
        [SerializeField] TMP_Text _totalTimeText;

        [SerializeField] TMP_Text _stageText;

        [Header("Middle")]
        [SerializeField] GridLayoutGroup _dotGridContent;

        [Header("Bottom")]
        [SerializeField] ToggleGroup _penToggleGroup;
        [SerializeField] RectTransform _penContent;

        [Header("ModeData")]
        [SerializeField] Color[] _pickColors;
        [SerializeField] Vector2 _spacing;
        //[SerializeField] int _pickCount;
        //[SerializeField] Vector2Int _levelSize = new Vector2Int(5, 5); // row, col

        [Header("Hint")]
        [SerializeField] Button _randomHintBtn;
        [SerializeField] Button _revealSameColorHintBtn;
        [SerializeField] GameObject _hintPanel;
        [SerializeField] GameObject _rememberPanel;
        [SerializeField] GameObject _coinPanel;
        [SerializeField] TMP_Text _coinTxt;

        [Header("GameOver")]
        [SerializeField] GameObject _gameOverPanel;
        [SerializeField] TMP_Text _clearStageCount;
        [SerializeField] Transform _clearStageContent;
        [SerializeField] Button _nextBtn;

        [Header("GameResult")]
        [SerializeField] GameObject _gameResultPanel;
        //[SerializeField] TMP_Text _resultScore;
        [SerializeField] TMP_Text _goldCount;

        [SerializeField] Transform _rankingContent;
        [SerializeField] ScrollRect _rankingScrollRect;
        [SerializeField] Button _tryAgainBtn;
        [SerializeField] Button _exitBtn;

        #region Stage

        MapData _mapData;
        Dot[,] _dots;
        Dot[] _colorPenDots;

        void DestroyDots()
        {
            int row = _dots.GetLength(0);
            int col = _dots.GetLength(1);

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    Destroy(_dots[i, j].gameObject);
                }
            }

            for (int i = 0; i < _colorPenDots.Length; i++)
            {
                Destroy(_colorPenDots[i].gameObject);
            }

            _dots = null;
            _colorPenDots = null;
        }

        Tuple<Dot[,], Dot[], MapData> GetStage()
        {
            return new Tuple<Dot[,], Dot[], MapData>(_dots, _colorPenDots, _mapData);
        }

        void SetStage(Dot[,] dots, Dot[] colorPenDots, MapData mapData)
        {
            _dots = dots;
            _colorPenDots = colorPenDots;
            _mapData = mapData;
        }

        #endregion 

        public struct StageData
        {
            [Newtonsoft.Json.JsonProperty] int _mapSize;
            [Newtonsoft.Json.JsonProperty] int _colorCount;
            [Newtonsoft.Json.JsonProperty] int _randomPointCount;

            public StageData(int mapSize, int colorCount, int randomPointCount)
            {
                _mapSize = mapSize;
                _colorCount = colorCount;
                _randomPointCount = randomPointCount;
            }

            [Newtonsoft.Json.JsonIgnore] public int MapSize { get => _mapSize; }
            [Newtonsoft.Json.JsonIgnore] public int ColorCount { get => _colorCount; }
            [Newtonsoft.Json.JsonIgnore] public int RandomPointCount { get => _randomPointCount; }
        }

        public struct StageDataWrapper
        {
            [Newtonsoft.Json.JsonProperty] List<StageData> stageDatas;

            public StageDataWrapper(List<StageData> stageDatas)
            {
                this.stageDatas = stageDatas;
            }

            [Newtonsoft.Json.JsonIgnore] public List<StageData> StageDatas { get => stageDatas; }
        }

        public class ModeData
        {
            float _memorizeDuration;
            float _playDuration;

            float _passedDuration; // 플레이하면서 지나간 시간

            float _decreaseDurationOnMiss; // 틀릴 시 감소 시간
            float _increaseDurationOnClear; // 클리어 시 증가 시간

            int _stageCount;
            int _myScore;
            int _clearStageCount;
            List<MapData> _stageData;

            public ModeData(float memorizeDuration = 0, float leftDuration = 0, float decreaseDurationOnMiss = 0, float increaseDurationOnClear = 0)
            {
                _memorizeDuration = memorizeDuration;
                _playDuration = leftDuration;
                _decreaseDurationOnMiss = decreaseDurationOnMiss;
                _increaseDurationOnClear = increaseDurationOnClear;

                _stageCount = 0;
                _myScore = 0;
                _clearStageCount = 0;
                _stageData = new List<MapData>();
            }

            public int StageCount { get => _stageCount; set => _stageCount = value; }
            public int MyScore { get => _myScore; set => _myScore = value; }
            public int ClearStageCount { get => _clearStageCount; set => _clearStageCount = value; }
            public List<MapData> StageData { get => _stageData; set => _stageData = value; }
            public float MemorizeDuration { get => _memorizeDuration; set => _memorizeDuration = value; }
            public float PlayDuration { get => _playDuration; set => _playDuration = value; }
            public float DecreaseDurationOnMiss { get => _decreaseDurationOnMiss; set => _decreaseDurationOnMiss = value; }
            public float IncreaseDurationOnClear { get => _increaseDurationOnClear; set => _increaseDurationOnClear = value; }
            public float PassedDuration { get => _passedDuration; set => _passedDuration = value; }
        }

        ModeData _modeData;
      

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

        public override void Initialize()
        {
            AddressableHandler addressableHandler = FindObjectOfType<AddressableHandler>();
            if (addressableHandler == null) return;

            RandomLevelGenerator randomLevelGenerator = new RandomLevelGenerator(addressableHandler.ChallengeStageDataWrapper.StageDatas);


            ClearPatternUIFactory clearPatternUIFactory = new ClearPatternUIFactory(
                addressableHandler.SpawnableUIAssets[SpawnableUI.Name.ClearPatternUI]);

            RankingUIFactory rankingFactory = new RankingUIFactory(
                addressableHandler.SpawnableUIAssets[SpawnableUI.Name.RankingUI],
                addressableHandler.RankingIconAssets);

            _modeData = new ModeData(5, 10, 1, 3);

            ChallengeStageUIModel model = new ChallengeStageUIModel();
            ChallengeStageUIViewer viewer = new ChallengeStageUIViewer(
                _playPanel,
                _bestScoreText,
                _nowScoreText,
                _timerSlider,
                _leftTimeText,
                _totalTimeText,
                _stageText,
                _hintPanel,
                _rememberPanel,

                _coinPanel,
                _coinTxt,

                _gameOverPanel,
                _clearStageCount,
                _clearStageContent,
                _gameResultPanel,
                _goldCount,
                _rankingContent,
                _rankingScrollRect);

            ChallengeStageUIPresenter presenter = new ChallengeStageUIPresenter(model, viewer);

            _nextBtn.onClick.AddListener(() => 
            { 
                _fsm.OnClickNextBtn(); 
            });

            _tryAgainBtn.onClick.AddListener(() => { _fsm.OnClickRetryBtn(); });
            _exitBtn.onClick.AddListener(() => { _fsm.OnClickExitBtn(); });

            _randomHintBtn.onClick.AddListener(() => { _fsm.OnClickRandomFillHint(); });
            _revealSameColorHintBtn.onClick.AddListener(() => { _fsm.OnClickRevealSameColorHint(); });

            presenter.ActivatePlayPanel(true);

            _fsm = new FSM<State>();
            Dictionary<State, BaseState<State>> states = new Dictionary<State, BaseState<State>>()
            {
                {
                    State.Initialize,
                    new InitializeState
                    (
                        _fsm,
                        _pickColors,
                        new EffectFactory(addressableHandler.EffectAssets),
                        new DotFactory(addressableHandler.DotAssets),
                        _dotGridContent,
                        _penContent,
                        _penToggleGroup,
                        randomLevelGenerator,
                        presenter,
                        _modeData,
                        SetStage
                    )
                },

                { State.Memorize, new MemorizeState(_fsm, _pickColors, _modeData, presenter, GetStage) },
                { State.Paint, new PaintState(_fsm, _pickColors, _modeData, presenter, GetStage) },
                { State.StageClear, new StageClearState(_fsm, presenter, _modeData, GetStage, DestroyDots) },
                { State.GameOver, new GameOverState(_fsm, presenter, _pickColors, clearPatternUIFactory, _modeData) },
                { State.Result, new ResultState(_fsm, rankingFactory, presenter, _modeData) }
            };

            _fsm.Initialize(states, State.Initialize);
        }
    }
}