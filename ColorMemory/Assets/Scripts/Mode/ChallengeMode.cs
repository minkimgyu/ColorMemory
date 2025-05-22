using NetworkService.Manager;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        [SerializeField] Button _pauseBtn;

        [SerializeField] Image _timerSlider;
        [SerializeField] TMP_Text _leftTimeText;
        [SerializeField] TMP_Text _totalTimeText;

        [SerializeField] TMP_Text _stageText;

        [Header("Middle")]
        [SerializeField] GridLayoutGroup _dotGridContent;

        [Header("Bottom")]
        [SerializeField] ToggleGroup _penToggleGroup;
        [SerializeField] RectTransform _bottomContent;
        [SerializeField] RectTransform _penContent;
        [SerializeField] Button _skipBtn;

        [Header("Setting")]
        [SerializeField] GameObject _pausePanel;
        [SerializeField] TMP_Text _pauseTitleText;

        [SerializeField] Button _pauseExitBtn;
        [SerializeField] Button _gameExitBtn;

        [SerializeField] CustomSlider _bgmSlider;
        [SerializeField] TMP_Text _bgmTitleText;
        [SerializeField] TMP_Text _bgmLeftText;
        [SerializeField] TMP_Text _bgmRightText;

        [SerializeField] CustomSlider _sfxSlider;
        [SerializeField] TMP_Text _sfxTitleText;
        [SerializeField] TMP_Text _sfxLeftText;
        [SerializeField] TMP_Text _sfxRightText;

        [Header("Preview")]
        [SerializeField] GameObject _stageOverPreviewPanel;
        [SerializeField] ClearPatternUI _lastStagePattern;
        [SerializeField] TMP_Text _stageOverTitleText;

        [SerializeField] TMP_Text _stageOverInfoText1;
        [SerializeField] TMP_Text _stageOverInfoText2;
        [SerializeField] Button _goToGameOverBtn;

        [Header("Hint")]
        [SerializeField] Button _oneZoneHintBtn;
        [SerializeField] Button _oneColorHintBtn;

        [SerializeField] TMP_Text _oneZoneHintCostText;
        [SerializeField] TMP_Text _oneColorHintCostText;

        [SerializeField] GameObject _hintPanel;
        [SerializeField] GameObject _rememberPanel;
        [SerializeField] TMP_Text _rememberTxt;

        [SerializeField] GameObject _coinPanel;
        [SerializeField] TMP_Text _coinTxt;

        [Header("GameOver")]
        [SerializeField] GameObject _gameOverPanel;

        [SerializeField] TMP_Text _clearStageCount;
        [SerializeField] TMP_Text _resultScore;

        [SerializeField] Transform _clearStageContent;
        [SerializeField] Button _gameOverExitBtn;
        [SerializeField] Button _nextBtn;

        [Header("GameResult")]
        [SerializeField] GameObject _gameResultPanel;
        
        [SerializeField] TMP_Text _goldCount;

        [SerializeField] Transform _rankingContent;
        [SerializeField] ScrollRect _rankingScrollRect;
        [SerializeField] Button _tryAgainBtn;
        [SerializeField] Button _gameResultExitBtn;

        IAssetService _challengeModeDataService;

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



        public class ModeData
        {
            int _goldCount;
            int _oneColorHintCost;
            int _oneZoneHintCost;

            float _playDuration;

            float _passedDuration; // 플레이하면서 지나간 시간

            float _decreaseDurationOnMiss; // 틀릴 시 감소 시간
            float _increaseDurationOnClear; // 클리어 시 증가 시간

            int _stageCount;
            int _myScore;
            int _maxScore;
            int _clearStageCount;
            List<MapData> _stageData;

            public ModeData(
                int goldCount,
                int oneColorHintCost,
                int oneZoneHintCost,
                int maxScore,

                float playDuration = 0,
                float decreaseDurationOnMiss = 0,
                float increaseDurationOnClear = 0)
            {
                _goldCount = goldCount;
                _oneColorHintCost = oneColorHintCost;
                _oneZoneHintCost = oneZoneHintCost;
                _maxScore = maxScore;

                _playDuration = playDuration;
                _decreaseDurationOnMiss = decreaseDurationOnMiss;
                _increaseDurationOnClear = increaseDurationOnClear;

                _stageCount = 0;
                _myScore = 0;
                _clearStageCount = 0;
                _stageData = new List<MapData>();
            }

            public int GoldCount { get => _goldCount; set => _goldCount = value; }
            public int StageCount { get => _stageCount; set => _stageCount = value; }
            public int ClearStageCount { get => _clearStageCount; set => _clearStageCount = value; }
            public List<MapData> StageData { get => _stageData; set => _stageData = value; }
            public float PlayDuration { get => _playDuration; set => _playDuration = value; }
            public float DecreaseDurationOnMiss { get => _decreaseDurationOnMiss; set => _decreaseDurationOnMiss = value; }
            public float IncreaseDurationOnClear { get => _increaseDurationOnClear; set => _increaseDurationOnClear = value; }
            public float PassedDuration { get => _passedDuration; set => _passedDuration = value; }


            public int OneColorHintCost { get => _oneColorHintCost; }
            public int OneZoneHintCost { get => _oneZoneHintCost; }

            public int MyScore { get => _myScore; set => _myScore = value; } // 현재 내 점수
            public int MaxScore { get => _maxScore; set => _maxScore = value; } // 서버에서 불러온 최대 점수

            public int BestScore // 위 둘을 비교했을 때 최대 점수
            {
                get
                {
                    return Mathf.Max(MyScore, MaxScore);
                }
            }
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
            Clear,
            GameOver,
            Result,
        }

        FSM<State> _fsm;
        bool _nowInitialized = false;

        private void Update()
        {
            if (_nowInitialized == false) return;
            _fsm.OnUpdate();
        }

        ChallengeStageUIModel _model;
        ChallengeStageUIPresenter _presenter;
        ChallengeStageUIViewer _viewer;

        public ChallengeStageUIModel Model { get => _model; }
        public ChallengeStageUIPresenter Presenter { get => _presenter; }
        public ChallengeStageUIViewer Viewer { get => _viewer; }

        public override async void Initialize()
        {
            string userId = ServiceLocater.ReturnSaveManager().GetSaveData().UserId;

            _challengeModeDataService = new ChallengeModeDataService();
            _modeData = await _challengeModeDataService.GetChallengeModeData(userId, 6, 2, 3);
            if (_modeData == null) return;

            AddressableLoader addressableHandler = FindObjectOfType<AddressableLoader>();
            if (addressableHandler == null) return;

            ServiceLocater.ReturnSoundPlayer().PlayBGM(ISoundPlayable.SoundName.ChallengeBGM);

            Color[] pickColors = addressableHandler.ColorPaletteDataWrapper.RandomColorPaletteData.Colors;

            RandomLevelGenerator randomLevelGenerator = new RandomLevelGenerator(addressableHandler.ChallengeStageJsonDataAsset.StageDatas);

            ClearPatternUIFactory clearPatternUIFactory = new ClearPatternUIFactory(
                addressableHandler.SpawnableUIAssets[SpawnableUI.Name.ClearPatternUI]);

            RankingUIFactory rankingFactory = new RankingUIFactory(
                addressableHandler.SpawnableUIAssets[SpawnableUI.Name.RankingUI],
                addressableHandler.RectProfileIconAssets);

            _model = new ChallengeStageUIModel();
            _presenter = new ChallengeStageUIPresenter(_model);
            _viewer = new ChallengeStageUIViewer(
                _playPanel,
                _bestScoreText,
                _nowScoreText,
                _timerSlider,
                _leftTimeText,
                _totalTimeText,
                _stageText,

                _oneZoneHintBtn,
                _oneColorHintBtn,

                _oneZoneHintCostText,
                _oneColorHintCostText,

                _bottomContent,
                _skipBtn,


                _hintPanel,
                _rememberPanel,
                _rememberTxt,

                _coinPanel,
                _coinTxt,

                _gameOverPanel,
                _resultScore,
                _clearStageCount,

                _clearStageContent,
                _gameOverExitBtn,
                _nextBtn,

                _gameResultPanel,
                _goldCount,
                _rankingContent,
                _rankingScrollRect,
                _tryAgainBtn,
                _gameResultExitBtn,

                _stageOverPreviewPanel,
                _lastStagePattern,
                _stageOverTitleText,
                _stageOverInfoText1,
                _stageOverInfoText2,
                _goToGameOverBtn,

                _pausePanel,
                _pauseTitleText,
                _pauseBtn,
                _pauseExitBtn,
                _gameExitBtn,
                _bgmSlider,
                _bgmTitleText,
                _bgmLeftText,
                _bgmRightText,
                _sfxSlider,
                _sfxTitleText,
                _sfxLeftText,
                _sfxRightText,
                _presenter);

            _presenter.InjectViewer(_viewer);

            //_goToGameOverBtn.onClick.AddListener(() =>
            //{
            //    _fsm.OnClickGoToGameOver();
            //});

            //_nextBtn.onClick.AddListener(() => 
            //{ 
            //    _fsm.OnClickNextBtn(); 
            //});

            //_gameOverExitBtn.onClick.AddListener(() =>
            //{
            //    _fsm.OnClickExitBtn();
            //});

            //_skipBtn.onClick.AddListener(() => { _fsm.OnClickSkipBtn(); });

            //_tryAgainBtn.onClick.AddListener(() => { _fsm.OnClickRetryBtn(); });
            //_gameResultExitBtn.onClick.AddListener(() => { _fsm.OnClickExitBtn(); });

            //_oneZoneHintBtn.onClick.AddListener(() => { _fsm.OnClickOneZoneHint(); });
            //_oneColorHintBtn.onClick.AddListener(() => { _fsm.OnClickOneColorHint(); });

            _presenter.ActivatePlayPanel(true);
            _presenter.ChangeHintCost(_modeData.OneColorHintCost, _modeData.OneZoneHintCost);
            _presenter.ChangeNowScore(_modeData.MyScore);
            _presenter.ChangeBestScore(_modeData.MaxScore);

            _fsm = new FSM<State>();
            Dictionary<State, BaseState<State>> states = new Dictionary<State, BaseState<State>>()
            {
                {
                    State.Initialize,
                    new InitializeState
                    (
                        _fsm,
                        pickColors,
                        new EffectFactory(addressableHandler.EffectAssets),
                        new DotFactory(addressableHandler.DotAssets),
                        _dotGridContent,
                        _penContent,
                        _penToggleGroup,
                        randomLevelGenerator,
                        _presenter,
                        _modeData,
                        SetStage
                    )
                },

                { State.Memorize, new MemorizeState(_fsm, pickColors, _modeData, addressableHandler.ChallengeStageJsonDataAsset.StageDatas, _presenter, GetStage) },
                { State.Paint, new PaintState(_fsm, pickColors, _modeData, _presenter, GetStage) },
                { State.Clear, new ClearState(_fsm, _presenter, _modeData, GetStage, DestroyDots) },
                { State.GameOver, new EndState(_fsm, _presenter, pickColors, clearPatternUIFactory, _modeData) },
                { State.Result, new ResultState(
                    _fsm,
                    new NearRankingService(),
                    new WeeklyScoreUpdateService(),
                    new TransactionService(),
                    rankingFactory,
                    _presenter,
                    _modeData) }
            };

            _fsm.Initialize(states, State.Initialize);
            _nowInitialized = true;
        }
    }
}