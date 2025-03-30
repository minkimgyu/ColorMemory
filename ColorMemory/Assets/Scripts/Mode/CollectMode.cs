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

        [SerializeField] TMP_Text _leftTimeText;
        [SerializeField] TMP_Text _totalTimeText;

        [SerializeField] TMP_Text _progressText;

        [Header("Middle")]
        [SerializeField] GridLayoutGroup _dotGridContent;

        [Header("Bottom")]
        [SerializeField] ToggleGroup _penToggleGroup;
        [SerializeField] RectTransform _penContent;

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
        [SerializeField] Image _cropArtworkImg;
        [SerializeField] Button _nextStageBtn;
        [SerializeField] Button _gameClearExitBtn;

        [Header("Result")]
        [SerializeField] GameObject _gameResultPanel;
        [SerializeField] TMP_Text _goldCount;
        [SerializeField] Button _exitBtn;


        CollectiveArtData.Section _section;
        MapData _mapData;
        Dot[,] _dots;
        Dot[] _colorPenDots;

        public class Data
        {
            float _memorizeDuration;
            int _myScore;

            Color[] _pickColors; // 색상 종류

            bool[,] _isPlayed; // 플레이 여부
            int[,] _goBackCount;

            public Data(Vector2Int sectionSize, int memorizeDuration, int myScore = 0)
            {
                _myScore = myScore;
                _memorizeDuration = memorizeDuration;

                _pickColors = new Color[3];
                _isPlayed = new bool[sectionSize.x, sectionSize.y];
                _goBackCount = new int[sectionSize.x, sectionSize.y];
            }

            public float MemorizeDuration { get => _memorizeDuration; set => _memorizeDuration = value; }
            public int MyScore { get => _myScore; set => _myScore = value; }
            public bool[,] IsPlayed { get => _isPlayed; set => _isPlayed = value; }
            public int[,] GoBackCount { get => _goBackCount; set => _goBackCount = value; }
            public Color[] PickColors { get => _pickColors; set => _pickColors = value; }
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

        void OnClickExitBtn()
        {
            ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.HomeScene);
        }

        private void Update()
        {
            _fsm.OnUpdate();
        }

        public override void Initialize()
        {
            AddressableHandler addressableHandler = FindObjectOfType<AddressableHandler>();
            if (addressableHandler == null) return;

            SaveData saveData = ServiceLocater.ReturnSaveManager().GetSaveData();

            ArtName artName = (ArtName)Enum.Parse(typeof(ArtName), saveData.SelectedArtworkName);
            Vector2Int sectionIndex = saveData.SelectedArtworkSectionIndex;

            CollectiveArtData artData = addressableHandler.CollectiveArtJsonAsserts[artName];

            _section = artData.Sections[sectionIndex.x][sectionIndex.y];

            Vector2Int index = new Vector2Int(_section.Blocks.Count, _section.Blocks[0].Count);
            _modeData = new Data(index, 5);


            CollectStageUIModel model = new CollectStageUIModel();
            CollectStageUIViewer viewer = new CollectStageUIViewer(
                _playPanel,
                _titleText,
                _timerContent,
                _timerSlider,
                _leftTimeText,
                _totalTimeText,
                _progressText,

                _rememberPanel,
                _hintInfoText,

                _gameClearPanel,
                _cropArtworkImg,
                _gameResultPanel,
                _goldCount);

            CollectStageUIPresenter presenter = new CollectStageUIPresenter(model, viewer);

            _gameClearExitBtn.onClick.AddListener(() => { _fsm.OnClickExitBtn(); });
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
                { State.Clear, new ClearState(_fsm, _modeData, artData, presenter, GetLevelData, DestroyDots) },
                { State.Result, new GameResultState(_fsm, presenter, _modeData) }
            };

            _fsm.Initialize(states, State.Initialize, sectionIndex);
        }
    }
}