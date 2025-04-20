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
        [Header("Play")]
        [SerializeField] GameObject _playPanel;

        [Header("Top")]
        [SerializeField] TMP_Text _titleText;
        [SerializeField] GameObject _timerContent;
        [SerializeField] Image _timerSlider;
        [SerializeField] Button _pauseBtn;

        [SerializeField] TMP_Text _leftTimeText;
        [SerializeField] TMP_Text _totalTimeText;

        [SerializeField] TMP_Text _progressText;

        [SerializeField] GameObject _detailContent;
        [SerializeField] TMP_Text _hintUsageText;
        [SerializeField] TMP_Text _wrongCountText;

        [Header("Middle")]
        [SerializeField] GridLayoutGroup _dotGridContent;

        [Header("Bottom")]
        [SerializeField] ToggleGroup _penToggleGroup;
        [SerializeField] RectTransform _penContent;
        [SerializeField] RectTransform _bottomContent;
        [SerializeField] Button _skipBtn;

        [Header("Setting")]
        [SerializeField] GameObject _pausePanel;
        [SerializeField] Button _pauseExitBtn;
        [SerializeField] Button _gameExitBtn;

        [SerializeField] CustomSlider _bgmSlider;
        [SerializeField] TMP_Text _bgmLeftText;

        [SerializeField] CustomSlider _sfxSlider;
        [SerializeField] TMP_Text _sfxLeftText;

        [Header("ModeData")]
        //[SerializeField] Color[] _pickColors;
        [SerializeField] Vector2 _spacing;
        //[SerializeField] int _pickCount;
        //[SerializeField] Vector2Int _levelSize = new Vector2Int(5, 5); // row, col

        [Header("Hint")]
        [SerializeField] Button _goBackBtn;
        [SerializeField] GameObject _rememberPanel;
        [SerializeField] TMP_Text _hintInfoText;

        [Header("Clear")]
        [SerializeField] GameObject _gameClearPanel;
        [SerializeField] TMP_Text _clearTitleText;
        [SerializeField] TMP_Text _clearContentText;
        [SerializeField] Button _nextStageBtn;
        [SerializeField] Button _clearExitBtn;

        [Header("Result")]
        [SerializeField] GameObject _gameResultPanel;
        [SerializeField] TMP_Text _gameResultTitle;

        [SerializeField] ArtworkUI _artworkUI;
        [SerializeField] TMP_Text _hintUseCount;
        [SerializeField] TMP_Text _wrongCount;

        [SerializeField] Image _rankBackground;
        [SerializeField] Image _rankIcon;
        [SerializeField] TMP_Text _rankText;

        [SerializeField] CustomProgressUI _currentCollectRatio;
        [SerializeField] TMP_Text _currentCollectText;

        [SerializeField] CustomProgressUI _totalCollectRatio;
        [SerializeField] TMP_Text _totalCollectText;
        [SerializeField] Button _nextBtn;

        [SerializeField] Canvas _canvas;
        [SerializeField] RectTransform _nextPanel;

        CollectArtData.Section _section;
        MapData _mapData;
        Dot[,] _dots;
        Dot[] _colorPenDots;

        public class Data
        {
            string _title;
            float _memorizeDuration;
            int _myScore;

            Color[] _pickColors; // 색상 종류

            int _goBackCount; // 힌트 사용 개수
            int _wrongCount; // 틀린 개수

            public Data(Vector2Int sectionSize, int memorizeDuration, string title)
            {
                _myScore = 0;
                _memorizeDuration = memorizeDuration;
                _title = title;

                _pickColors = new Color[3];
                _goBackCount = 0;
                _wrongCount = 0;
            }

            public float MemorizeDuration { get => _memorizeDuration; set => _memorizeDuration = value; }
            public int MyScore { get => _myScore; set => _myScore = value; }
            public int GoBackCount { get => _goBackCount; set => _goBackCount = value; }
            public int WrongCount { get => _wrongCount; set => _wrongCount = value; }

            public Color[] PickColors { get => _pickColors; set => _pickColors = value; }
            public string Title { get => _title; set => _title = value; }
        }

        CollectMode.Data _modeData;

        public enum State
        {
            Initialize,
            Memorize,
            Paint,
            Clear,
            Result,
        }

        FSM<State> _fsm;

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

        void SetLevelData(Dot[,] dots, Dot[] colorPenDots, MapData mapData)
        {
            _dots = dots;
            _colorPenDots = colorPenDots;
            _mapData = mapData;
        }

        Tuple<Dot[,], Dot[], MapData> GetLevelData()
        {
            return new Tuple<Dot[,], Dot[], MapData>(_dots, _colorPenDots, _mapData);
        }

        CollectMode.Data GetCollectModeData()
        {
            return _modeData;
        }

        private void Update()
        {
            _fsm.OnUpdate();
        }

        public override void Initialize()
        {
            AddressableLoader addressableHandler = FindObjectOfType<AddressableLoader>();
            if (addressableHandler == null) return;

            Vector2 size = _canvas.gameObject.GetComponent<RectTransform>().sizeDelta;
            _nextPanel.sizeDelta = new Vector2(size.x, _nextPanel.sizeDelta.y); // 사이즈 맞춰주기

            SaveData saveData = ServiceLocater.ReturnSaveManager().GetSaveData();

            int artworkIndex = saveData.SelectedArtworkKey;
            CollectArtData artData = addressableHandler.CollectiveArtJsonAsserts[artworkIndex];

            _section = artData.Sections[saveData.SelectedArtworkSectionIndex.x][saveData.SelectedArtworkSectionIndex.y];

            Vector2Int index = new Vector2Int(_section.Blocks.Count, _section.Blocks[0].Count);
            _modeData = new Data(index, 5, addressableHandler.ArtworkJsonAsset.Data[artworkIndex].Title);

            CollectStageUIModel model = new CollectStageUIModel();
            CollectStageUIPresenter presenter = new CollectStageUIPresenter(model, () => { _fsm.SetState(State.Result); });
            CollectStageUIViewer viewer = new CollectStageUIViewer(
                _playPanel,
                _titleText,
                _timerContent,
                _timerSlider,
                _leftTimeText,
                _totalTimeText,
                _progressText,

                _detailContent,
                _hintUsageText,
                _wrongCountText,

                _bottomContent,
                _skipBtn,

                _rememberPanel,
                _hintInfoText,

                _gameClearPanel,
                _clearTitleText,
                _clearContentText,
                _nextStageBtn,
                _clearExitBtn,

                _pausePanel,
                _pauseBtn,
                _pauseExitBtn,
                _gameExitBtn,

                _bgmSlider,
                _bgmLeftText,

                _sfxSlider,
                _sfxLeftText,

                _gameResultPanel,
                _gameResultTitle,
                _artworkUI,
                _hintUseCount,
                _wrongCount,

                _rankBackground,
                _rankIcon,
                _rankText,

                _currentCollectRatio,
                _currentCollectText,

                _totalCollectRatio,
                _totalCollectText,
                presenter);

            presenter.InjectViewer(viewer);

            _skipBtn.onClick.AddListener(() => { _fsm.OnClickSkipBtn(); });

            _nextBtn.onClick.AddListener(() => { _fsm.OnClickNextBtn(); });

            _clearExitBtn.onClick.AddListener(() => { _fsm.OnClickExitBtn(); });
            _nextStageBtn.onClick.AddListener(() => { _fsm.OnClickNextStageBtn(); });

            _goBackBtn.onClick.AddListener(() => { _fsm.OnClickGoBackHint(); });

            _fsm = new FSM<State>();
            Dictionary<State, BaseState<State>> states = new Dictionary<State, BaseState<State>>()
            {
                {
                    State.Initialize,
                    new InitializeState
                    (
                        _fsm,
                        _modeData,
                        new EffectFactory(addressableHandler.EffectAssets),
                        new DotFactory(addressableHandler.DotAssets),
                        _dotGridContent,
                        _penContent,
                        _penToggleGroup,
                        artData,
                        presenter,
                        SetLevelData
                    )
                },

                { State.Memorize, new MemorizeState(_fsm, _modeData, presenter, GetLevelData) },
                { State.Paint, new PaintState(_fsm, _modeData, presenter, GetLevelData) },
                { State.Clear, new ClearState(
                    _fsm,
                    new ArtDataLoaderService(),
                    new ArtDataUpdaterService(),
                    _modeData, 
                    artData, 
                    presenter, 
                    GetLevelData, 
                    DestroyDots) },
                { State.Result, new ResultState(
                    _fsm,
                    new ArtDataLoaderService(),
                    new ArtDataUpdaterService(),
                    presenter,
                    _modeData) }
            };

            _fsm.Initialize(states, State.Initialize);
        }
    }
}