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

    GameObject _coinPanel;
    TMP_Text _coinTxt;

    GameObject _gameOverPanel;
    TMP_Text _clearStageCount;
    TMP_Text _resultScore;

    Transform _clearStageContent;

    GameObject _gameResultPanel;
    TMP_Text _goldCount;

    Transform _rankingContent;
    ScrollRect _rankingScrollRect;

    GameObject _stageOverPreviewPanel;
    ClearPatternUI _lastStagePattern;
    TMP_Text _stageOverInfoText;

    GameObject _pausePanel;
    Button _pauseBtn;
    Button _pauseExitBtn;
    Button _gameExitBtn;

    CustomSlider _bgmSlider;
    Image _bgmSliderHandle;
    TMP_Text _bgmMuteText;

    CustomSlider _sfxSlider;
    Image _sfxSliderHandle;
    TMP_Text _sfxMuteText;

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

        GameObject coinPanel,
        TMP_Text coinTxt,

        GameObject gameOverPanel,
        TMP_Text resultScore,
        TMP_Text clearStageCount,
        Transform clearStageContent,

        GameObject gameResultPanel,
        TMP_Text goldCount,
        Transform rankingContent,
        ScrollRect rankingScrollRect,

        GameObject stageOverPreviewPanel,
        ClearPatternUI lastStagePattern,
        TMP_Text stageOverInfoText,

        GameObject pausePanel,
        Button pauseBtn,
        Button pauseExitBtn,
        Button gameExitBtn,
        CustomSlider bgmSlider,
        TMP_Text bgmMuteText,

        CustomSlider sfxSlider,
        TMP_Text sfxMuteText,

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
        _oneColorHintBtn = oneColorHintBtn;

        _oneColorHintCostText = oneColorHintCostText;
        _oneZoneHintCostText = oneZoneHintCostText;

        _bottomContent = bottomContent;
        _skipBtn = skipBtn;

        _hintPanel = hintPanel;
        _rememberPanel = rememberPanel;

        _coinPanel = coinPanel;
        _coinTxt = coinTxt;

        _gameOverPanel = gameOverPanel;
        _clearStageCount = clearStageCount;
        _resultScore = resultScore;
        _clearStageContent = clearStageContent;

        _gameResultPanel = gameResultPanel;
        _goldCount = goldCount;
        _rankingContent = rankingContent;
        _rankingScrollRect = rankingScrollRect;

        _stageOverPreviewPanel = stageOverPreviewPanel;

        _lastStagePattern = lastStagePattern;
        _stageOverInfoText = stageOverInfoText;

        _pausePanel = pausePanel;
        _pauseBtn = pauseBtn;
        _gameExitBtn = gameExitBtn;
        _pauseExitBtn = pauseExitBtn;

        _bgmSlider = bgmSlider;
        _bgmSliderHandle = bgmSlider.handleRect.GetComponent<Image>();
        _bgmMuteText = bgmMuteText;

        _sfxSlider = sfxSlider;
        _sfxSliderHandle = sfxSlider.handleRect.GetComponent<Image>();
        _sfxMuteText = sfxMuteText;

        _bgmSlider.onHandlePointerUp += ((ratio) => { presenter.SaveBGMValue(); });
        _sfxSlider.onHandlePointerUp += ((ratio) => { presenter.SaveSFXValue(); });

        _gameExitBtn.onClick.AddListener(() => { presenter.OnClickGameExitBtn(); presenter.GoToEndState?.Invoke(); });
        _pauseBtn.onClick.AddListener(() => { presenter.ActivatePausePanel(true); });
        _pauseExitBtn.onClick.AddListener(() => { presenter.ActivatePausePanel(false); });
        _bgmSlider.onValueChanged.AddListener((ratio) => { presenter.OnBGMSliderValeChanged(ratio); });
        _sfxSlider.onValueChanged.AddListener((ratio) => { presenter.OnSFXSliderValeChanged(ratio); });
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
        _bgmMuteText.text = leftSmallTxt;
    }

    public void ChangeSFXSliderHandleColor(string leftSmallTxt, Color handleColor)
    {
        _sfxSliderHandle.color = handleColor;
        _sfxMuteText.text = leftSmallTxt;
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
        _stageOverInfoText.text = $"클리어하지 못한 {currentStageCount}번째 패턴이에요";
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
        _goldCount.text = $"{goldCount} 코인 획득!";
    }

    public void AddRanking(SpawnableUI ranking, Vector3 size)
    {
        ranking.transform.SetParent(_rankingContent);
        ranking.transform.localScale = size;

        //if (setToMiddle == true) ranking.transform.SetSiblingIndex(_rankingContent.childCount / 2);
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