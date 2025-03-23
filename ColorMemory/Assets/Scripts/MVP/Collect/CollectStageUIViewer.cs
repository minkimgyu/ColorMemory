using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectStageUIViewer
{
    TMP_Text _bestScoreText;
    TMP_Text _nowScoreText;

    Image _timerSlider;

    TMP_Text _leftTimeText;
    TMP_Text _totalTimeText;

    GameObject _hintPanel;
    GameObject _rememberPanel;

    GameObject _gameOverPanel;
    TMP_Text _clearStageCount;
    Transform _clearStageContent;
    Button _nextBtn;

    GameObject _gameResultPanel;
    TMP_Text _resultScore;
    TMP_Text _goldCount;
    Transform _rankingContent;
    Button _tryAgainBtn;
    Button _exitBtn;

    public CollectStageUIViewer(
        TMP_Text bestScoreText,
        TMP_Text nowScoreText,
        Image timerSlider,
        TMP_Text leftTimeText,
        TMP_Text totalTimeText,
        GameObject hintPanel,
        GameObject rememberPanel,

        GameObject gameOverPanel,
        TMP_Text clearStageCount,
        Transform clearStageContent,
        Button nextBtn,

        GameObject gameResultPanel,
        TMP_Text resultScore,
        TMP_Text goldCount,
        Transform rankingContent,
        Button tryAgainBtn, // -> Reload Scene
        Button exitBtn, // -> Go to home Scene

        ChallengeStageUIPresenter presenter)
    {
        _bestScoreText = bestScoreText;
        _nowScoreText = nowScoreText;
        _timerSlider = timerSlider;
        _leftTimeText = leftTimeText;
        _totalTimeText = totalTimeText;
        _hintPanel = hintPanel;
        _rememberPanel = rememberPanel;

        _gameOverPanel = gameOverPanel;
        _clearStageCount = clearStageCount;
        _clearStageContent = clearStageContent;
        _nextBtn = nextBtn;

        _gameResultPanel = gameResultPanel;
        _resultScore = resultScore;
        _goldCount = goldCount;
        _rankingContent = rankingContent;
        _tryAgainBtn = tryAgainBtn;
        _exitBtn = exitBtn;

        _nextBtn.onClick.AddListener(() => presenter.OnNextBtnClicked());
        _tryAgainBtn.onClick.AddListener(() => presenter.OnRetryBtnClicked());
        _exitBtn.onClick.AddListener(() => presenter.OnExitBtnClicked());
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

    public void ActivateRememberPanel(bool active)
    {
        _rememberPanel.SetActive(active);
    }

    public void ActivateHintPanel(bool active)
    {
        _hintPanel.SetActive(active);
    }

    public void ActivateGameOverPanel(bool active)
    {
        _gameOverPanel.SetActive(active);
    }

    public void ChangeClearStageCount(int stageCount)
    {
        _clearStageCount.text = stageCount.ToString();
    }

    public void AddClearPattern(ClearPatternUI patternUI)
    {
        patternUI.transform.SetParent(_clearStageContent);
    }

    public void ActivateGameResultPanel(bool active)
    {
        _gameResultPanel.SetActive(active);
    }

    public void ChangeResultScore(int resultScore)
    {
        _resultScore.text = resultScore.ToString();
    }

    public void ChangeGoldCount(int goldCount)
    {
        _goldCount.text = goldCount.ToString();
    }

    public void AddRanking(RankingUI ranking)
    {
        ranking.transform.SetParent(_rankingContent);
    }
}
