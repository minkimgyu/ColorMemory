using Challenge;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestChallengeMode : GameMode
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

    ChallengeStageUIModel _model;
    ChallengeStageUIPresenter _presenter;
    ChallengeStageUIViewer _viewer;

    public ChallengeStageUIModel Model { get => _model; }
    public ChallengeStageUIPresenter Presenter { get => _presenter; }
    public ChallengeStageUIViewer Viewer { get => _viewer; }

    public override void Initialize()
    {
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
    }
}
