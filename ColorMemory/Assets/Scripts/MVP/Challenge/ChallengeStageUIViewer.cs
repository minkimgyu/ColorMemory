using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeStageUIViewer
{
    TMP_Text _bestScoreText;
    TMP_Text _nowScoreText;

    Image _timerSlider;

    TMP_Text _leftTimeText;
    TMP_Text _totalTimeText;

    TMP_Text _stageText;

    GameObject _hintPanel;
    GameObject _rememberPanel;

    GameObject _gameOverPanel;
    TMP_Text _clearStageCount;
    Transform _clearStageContent;

    GameObject _gameResultPanel;
    TMP_Text _goldCount;
    RankingScrollUI _rankingScrollRect;
    Transform _rankingContent;

    public ChallengeStageUIViewer(
        TMP_Text bestScoreText,
        TMP_Text nowScoreText,
        Image timerSlider,
        TMP_Text leftTimeText,
        TMP_Text totalTimeText,
        TMP_Text stageText,

        GameObject hintPanel,
        GameObject rememberPanel,

        GameObject gameOverPanel,
        TMP_Text clearStageCount,
        Transform clearStageContent,

        GameObject gameResultPanel,
        TMP_Text goldCount,
        RankingScrollUI rankingScrollRect)
    {
        _bestScoreText = bestScoreText;
        _nowScoreText = nowScoreText;
        _timerSlider = timerSlider;
        _leftTimeText = leftTimeText;
        _totalTimeText = totalTimeText;
        _stageText = stageText;

        _hintPanel = hintPanel;
        _rememberPanel = rememberPanel;

        _gameOverPanel = gameOverPanel;
        _clearStageCount = clearStageCount;
        _clearStageContent = clearStageContent;

        _gameResultPanel = gameResultPanel;
        _goldCount = goldCount;
        _rankingScrollRect = rankingScrollRect;
    }

    public void ChangeRankingScrollValue(int menuCount, int index)
    {
        _rankingScrollRect.SetUp(menuCount, index);
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

    public void ActivateHintPanel(bool active)
    {
        _hintPanel.SetActive(active);
    }

    public void ActivateGameOverPanel(bool active)
    {
        _gameOverPanel.SetActive(active);
    }

    public void ChangeClearStageCount(int passedDuration, int stageCount)
    {
        _clearStageCount.text = $"{passedDuration}초 동안 {stageCount}개의 패턴 클리어!";
    }

    public void AddClearPattern(SpawnableUI patternUI)
    {
        patternUI.transform.SetParent(_clearStageContent);
    }

    public void RemoveClearPattern()
    {
        for (int i = _clearStageContent.childCount - 1; i >= 0; i--)
        {
            _clearStageContent.GetChild(i).GetComponent<SpawnableUI>().DestroyObject();
        }
    }

    public void ActivateGameResultPanel(bool active)
    {
        _gameResultPanel.SetActive(active);
    }

    public void ChangeGoldCount(int goldCount)
    {
        _goldCount.text = $"{goldCount} 코인 획득!";
    }

    public void AddRanking(SpawnableUI ranking, bool setToMiddle = false)
    {
        _rankingScrollRect.AddItem(ranking.transform, setToMiddle);
    }

    public void RemoveAllRanking()
    {
        _rankingScrollRect.DestroyItems();
    }
}
