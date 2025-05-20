using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeStageUIViewer
{
    GameObject _playPanel;

    TMP_Text _bestScoreText;
    TMP_Text _nowScoreText;

    Image _timerSlider;

    TMP_Text _leftTimeText;
    TMP_Text _totalTimeText;

    TMP_Text _stageText;

    Button _oneZoneHintBtn;
    Button _oneColorHintBtn;

    TMP_Text _oneZoneHintCostText;
    TMP_Text _oneColorHintCostText;

    RectTransform _bottomContent;
    Button _skipBtn;




    GameObject _hintPanel;
    GameObject _rememberPanel;
    TMP_Text _rememberTxt;

    GameObject _coinPanel;
    TMP_Text _coinTxt;

    GameObject _gameOverPanel;
    TMP_Text _clearStageCount;
    TMP_Text _resultScore;

    Transform _clearStageContent;
    Button _gameOverExitBtn;
    Button _nextBtn;

    GameObject _gameResultPanel;
    TMP_Text _goldCount;

    Transform _rankingContent;
    ScrollRect _rankingScrollRect;
    Button _tryAgainBtn;
    Button _gameResultExitBtn;

    GameObject _stageOverPreviewPanel;
    ClearPatternUI _lastStagePattern;

    TMP_Text _stageOverTitleText;
    TMP_Text _stageOverInfoText1;
    TMP_Text _stageOverInfoText2;
    Button _goToGameOverBtn;

    GameObject _pausePanel;
    TMP_Text _pauseTitleText;

    Button _pauseBtn;
    Button _pauseExitBtn;
    Button _gameExitBtn;
    TMP_Text _gameExitText;

    TMP_Text _bgmTitleText;
    CustomSlider _bgmSlider;
    Image _bgmSliderHandle;
    TMP_Text _bgmLeftText;
    TMP_Text _bgmRightText;

    TMP_Text _sfxTitleText;
    CustomSlider _sfxSlider;
    Image _sfxSliderHandle;
    TMP_Text _sfxLeftText;
    TMP_Text _sfxRightText;

    public ChallengeStageUIViewer(
        GameObject playPanel,
        TMP_Text bestScoreText,
        TMP_Text nowScoreText,
        Image timerSlider,
        TMP_Text leftTimeText,
        TMP_Text totalTimeText,
        TMP_Text stageText,

        Button oneZoneHintBtn,
        Button oneColorHintBtn,

        TMP_Text oneZoneHintCostText,
        TMP_Text oneColorHintCostText,

        RectTransform bottomContent,
        Button skipBtn,

        GameObject hintPanel,
        GameObject rememberPanel,
        TMP_Text rememberTxt,

        GameObject coinPanel,
        TMP_Text coinTxt,

        GameObject gameOverPanel,
        TMP_Text resultScore,
        TMP_Text clearStageCount,
        Transform clearStageContent,

        Button gameOverExitBtn,
        Button nextBtn,

        GameObject gameResultPanel,
        TMP_Text goldCount,
        Transform rankingContent,
        ScrollRect rankingScrollRect,

        Button tryAgainBtn,
        Button gameResultExitBtn,

        GameObject stageOverPreviewPanel,
        ClearPatternUI lastStagePattern,
        TMP_Text stageOverTitleText,

        TMP_Text stageOverInfoText1,
        TMP_Text stageOverInfoText2,
        Button goToGameOverBtn,

        GameObject pausePanel,
        TMP_Text pauseTitleText,

        Button pauseBtn,
        Button pauseExitBtn,
        Button gameExitBtn,

        CustomSlider bgmSlider,
        TMP_Text bgmTitleText,
        TMP_Text bgmLeftText,
        TMP_Text bgmRightText,

        CustomSlider sfxSlider,
        TMP_Text sfxTitleText,
        TMP_Text sfxLeftText,
        TMP_Text sfxRightText,

        ChallengeStageUIPresenter presenter)
    {
        _playPanel = playPanel;

        _bestScoreText = bestScoreText;
        _nowScoreText = nowScoreText;
        _timerSlider = timerSlider;
        _leftTimeText = leftTimeText;
        _totalTimeText = totalTimeText;
        _stageText = stageText;

        _oneZoneHintBtn = oneZoneHintBtn;
        _oneZoneHintBtn.onClick.AddListener(() => { presenter.OnClickOneZoneHint(); });

        _oneColorHintBtn = oneColorHintBtn;
        _oneColorHintBtn.onClick.AddListener(() => { presenter.OnClickOneColorHint(); });

        _oneColorHintCostText = oneColorHintCostText;
        _oneZoneHintCostText = oneZoneHintCostText;

        _bottomContent = bottomContent;
        _skipBtn = skipBtn;
        _skipBtn.onClick.AddListener(() => { presenter.OnClickSkipBtn?.Invoke(); });

        _hintPanel = hintPanel;
        _rememberPanel = rememberPanel;
        _rememberTxt = rememberTxt;

        _coinPanel = coinPanel;
        _coinTxt = coinTxt;

        _gameOverPanel = gameOverPanel;
        _clearStageCount = clearStageCount;
        _resultScore = resultScore;
        _clearStageContent = clearStageContent;

        _gameOverExitBtn = gameOverExitBtn;
        _gameOverExitBtn.onClick.AddListener(() => { presenter.OnClickGameOverExitBtn?.Invoke(); });

        _nextBtn = nextBtn;
        _nextBtn.onClick.AddListener(() => { presenter.OnClickNextBtn?.Invoke(); });


        _gameResultPanel = gameResultPanel;
        _goldCount = goldCount;
        _rankingContent = rankingContent;
        _rankingScrollRect = rankingScrollRect;

        _tryAgainBtn = tryAgainBtn;
        _tryAgainBtn.onClick.AddListener(() => { presenter.OnClickRetryBtn?.Invoke(); });

        _gameResultExitBtn = gameResultExitBtn;
        _gameResultExitBtn.onClick.AddListener(() => { presenter.OnClickExitBtn?.Invoke(); });

        _stageOverPreviewPanel = stageOverPreviewPanel;

        _lastStagePattern = lastStagePattern;

        _stageOverTitleText = stageOverTitleText;
        _stageOverInfoText1 = stageOverInfoText1;
        _stageOverInfoText2 = stageOverInfoText2;
        _goToGameOverBtn = goToGameOverBtn;
        _goToGameOverBtn.onClick.AddListener(() => { presenter.OnClickGoToGameOverBtn?.Invoke(); });

        _pausePanel = pausePanel;
        _pauseTitleText = pauseTitleText;

        _pauseBtn = pauseBtn;
        _gameExitBtn = gameExitBtn;
        _gameExitText = _gameExitBtn.GetComponentInChildren<TMP_Text>();

        _pauseExitBtn = pauseExitBtn;

        _bgmTitleText = bgmTitleText;
        _bgmSlider = bgmSlider;
        _bgmSliderHandle = bgmSlider.handleRect.GetComponent<Image>();
        _bgmLeftText = bgmLeftText;
        _bgmRightText = bgmRightText;

        _sfxTitleText = sfxTitleText;
        _sfxSlider = sfxSlider;
        _sfxSliderHandle = sfxSlider.handleRect.GetComponent<Image>();
        _sfxLeftText = sfxLeftText;
        _sfxRightText = sfxRightText;

        _bgmSlider.onHandlePointerUp += ((ratio) => { presenter.SaveBGMValue(); });
        _sfxSlider.onHandlePointerUp += ((ratio) => { presenter.SaveSFXValue(); });

        _gameExitBtn.onClick.AddListener(() => { presenter.ActivatePausePanel(false); presenter.OnClickPauseGameExitBtn?.Invoke(); });

        _pauseBtn.onClick.AddListener(() =>
        {
            ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.BtnClick);
            presenter.ActivatePausePanel(true);
        });

        _pauseExitBtn.onClick.AddListener(() =>
        {
            presenter.ActivatePausePanel(false);
            ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.BtnClick);
        });

        _bgmSlider.onValueChanged.AddListener((ratio) => { presenter.OnBGMSliderValeChanged(ratio); });
        _sfxSlider.onValueChanged.AddListener((ratio) => { presenter.OnSFXSliderValeChanged(ratio); });
    }

    public void ChangePauseTitleText(string title)
    {
        _pauseTitleText.text = title;
    }

    public void ChangeGameExitText(string content)
    {
        _gameExitText.text = content;
    }

    public void ChangeSoundText(string bgmTitle, string sfxTitle, string leftText, string rightText)
    {
        _bgmTitleText.text = bgmTitle;
        _sfxTitleText.text = sfxTitle;

        _bgmLeftText.text = leftText;
        _sfxLeftText.text = leftText;

        _bgmRightText.text = rightText;
        _sfxRightText.text = rightText;
    }

    public void ActivateBottomContent(bool active)
    {
        _bottomContent.gameObject.SetActive(active);
    }

    public void ActivateSkipBtn(bool active)
    {
        _skipBtn.gameObject.SetActive(active);
    }

    public void ActivateHint(bool oneColorHintActive, bool oneZoneHintActive)
    {
        _oneColorHintBtn.interactable = oneColorHintActive;
        _oneZoneHintBtn.interactable = oneZoneHintActive;
    }

    public void ChangeHintCost(int oneColorHintCost, int oneZoneHintCost)
    {
        _oneColorHintCostText.text = $"-{oneColorHintCost.ToString("N0")}";
        _oneZoneHintCostText.text = $"-{oneZoneHintCost.ToString("N0")}";

        LayoutRebuilder.ForceRebuildLayoutImmediate(_oneColorHintCostText.transform.parent.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(_oneZoneHintCostText.transform.parent.GetComponent<RectTransform>());
    }

    public void ChangeBGMSliderValue(float ratio, string leftSmallTxt, Color handleColor)
    {
        _bgmSlider.value = ratio;
        ChangeBGMSliderHandleColor(leftSmallTxt, handleColor);
    }

    public void ChangeSFXSliderValue(float ratio, string leftSmallTxt, Color handleColor)
    {
        _sfxSlider.value = ratio;
        ChangeSFXSliderHandleColor(leftSmallTxt, handleColor);
    }

    public void ChangeBGMSliderHandleColor(string leftSmallTxt, Color handleColor)
    {
        _bgmSliderHandle.color = handleColor;
        _bgmLeftText.text = leftSmallTxt;
    }

    public void ChangeSFXSliderHandleColor(string leftSmallTxt, Color handleColor)
    {
        _sfxSliderHandle.color = handleColor;
        _sfxLeftText.text = leftSmallTxt;
    }

    public void ActivatePausePanel(bool active)
    {
        _pausePanel.SetActive(active);
    }

    public void ActivateStageOverPreviewPanel(bool active)
    {
        _stageOverPreviewPanel.SetActive(active);
    }

    public void ChangeLastStagePattern(int currentStageCount, MapData data, Color[] pickColors)
    {
        _lastStagePattern.Initialize(currentStageCount, data, pickColors);
    }

    public void ChangeStageOverInfo(int currentStageCount)
    {
        _stageOverTitleText.text = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.PreviewTitle);

        string content1 = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.PreviewContent1);
        _stageOverInfoText1.text = string.Format(content1, currentStageCount);

        string content2 = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.PreviewContent2);
        _stageOverInfoText2.text = content2;
    }

    public void ActivatePlayPanel(bool active)
    {
        _playPanel.SetActive(active);
    }

    public void ChangeNowScore(int score)
    {
        _nowScoreText.text = score.ToString();
    }

    public void ChangeBestScore(int score)
    {
        _bestScoreText.text = score.ToString();
    }

    public void FillTimeSlider(float ratio)
    {
        _timerSlider.fillAmount = ratio;
    }

    public void ChangeTotalTime(float totalTime)
    {
        int intPart = (int)totalTime;      // 정수 부분
        float decimalPart = totalTime % 1; // 소수점 이하

        // 정수 부분이 1자리면 D2로 맞추고, 그렇지 않으면 그대로 출력
        string formattedIntPart = intPart < 10 ? $"{intPart:D2}" : $"{intPart}";

        // 소수점 이하 두 자리 유지
        _totalTimeText.text = $"{formattedIntPart}.{(decimalPart * 100):00}";
    }

    public void ChangeLeftTime(float leftTime, float ratio)
    {
        int intPart = (int)leftTime;
        float decimalPart = leftTime % 1;

        string formattedIntPart = intPart < 10 ? $"{intPart:D2}" : $"{intPart}";

        _leftTimeText.text = $"{formattedIntPart}.{(decimalPart * 100):00}";
        FillTimeSlider(ratio);
    }

    public void ChangeStageCount(int stageCount)
    {
        _stageText.text = stageCount.ToString();
    }

    public void ActivateRememberPanel(bool active)
    {
        _rememberPanel.SetActive(active);
        _rememberTxt.text = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.RememberTitle);
    }

    public void ActivateCoinPanel(bool active)
    {
        _coinPanel.SetActive(active);
    }

    public void ChangeCoinCount(int coin)
    {
        _coinTxt.text = coin.ToString("N0"); // "9,000"
        LayoutRebuilder.ForceRebuildLayoutImmediate(_coinTxt.transform.parent.GetComponent<RectTransform>());
    }


    public void ActivateHintPanel(bool active)
    {
        _hintPanel.SetActive(active);
    }

    public void ActivateGameOverPanel(bool active)
    {
        _gameOverPanel.SetActive(active);
    }

    public void ChangeClearStageCount(int clearStageCount, int resultScore)
    {
        _clearStageCount.text = clearStageCount.ToString("N0");
        _resultScore.text = resultScore.ToString("N0");
    }

    public void AddClearPattern(SpawnableUI patternUI)
    {
        patternUI.transform.SetParent(_clearStageContent);
        patternUI.transform.localScale = Vector3.one;
    }

    public void RemoveClearPattern()
    {
        for (int i = _clearStageContent.childCount - 1; i >= 0; i--)
        {
            SpawnableUI spawnableUI = _clearStageContent.GetChild(i).GetComponent<SpawnableUI>();
            if (spawnableUI == null) continue;

            spawnableUI.DestroyObject();
        }
    }

    public void ActivateGameResultPanel(bool active)
    {
        _gameResultPanel.SetActive(active);
    }

    public void ChangeResultGoldCount(int goldCount)
    {
        string getCoinTxt = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.GetCoin);
        _goldCount.text = string.Format(getCoinTxt, goldCount);
    }

    public void AddRanking(SpawnableUI ranking, Vector3 size)
    {
        ranking.transform.SetParent(_rankingContent);
        ranking.transform.localScale = size;
    }

    public void RemoveAllRanking()
    {
        for (int i = _rankingContent.childCount - 1; i >= 0; i--)
        {
            SpawnableUI spawnableUI = _rankingContent.GetChild(i).GetComponent<SpawnableUI>();
            if(spawnableUI == null) continue;

            spawnableUI.DestroyObject();
        }
    }

    public void ChangeRankingScrollValue(int menuCount, int scrollIndex)
    {
        //_rankingScrollRect.verticalNormalizedPosition = (float)scrollIndex / (float)menuCount;
    }
}