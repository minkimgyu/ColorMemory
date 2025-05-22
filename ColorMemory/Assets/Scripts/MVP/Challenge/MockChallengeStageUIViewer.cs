using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MockChallengeStageUIViewer : IChallengeStageUIViewer
{
    // Text UI를 대체하는 문자열 속성
    public string BestScore { get; private set; }
    public string NowScore { get; private set; }
    public string LeftTime { get; private set; }
    public string TotalTime { get; private set; }
    public string StageText { get; private set; }
    public string OneZoneHintCost { get; private set; }
    public string OneColorHintCost { get; private set; }
    public string PauseTitle { get; private set; }
    public string GameExitText { get; private set; }
    public string BGMTitle { get; private set; }
    public string SFXTitle { get; private set; }
    public string BGMLeftText { get; private set; }
    public string BGMRightText { get; private set; }
    public string SFXLeftText { get; private set; }
    public string SFXRightText { get; private set; }
    public string RememberText { get; private set; }
    public string CoinCount { get; private set; }
    public string ClearStageCount { get; private set; }
    public string ResultScore { get; private set; }
    public string ResultGoldCount { get; private set; }
    public string StageOverTitle { get; private set; }
    public string StageOverInfo1 { get; private set; }
    public string StageOverInfo2 { get; private set; }

    // GameObject UI를 대체하는 활성화 상태 플래그
    public bool IsPlayPanelActive { get; private set; }
    public bool IsBottomContentActive { get; private set; }
    public bool IsSkipBtnActive { get; private set; }
    public bool IsHintPanelActive { get; private set; }
    public bool IsRememberPanelActive { get; private set; }
    public bool IsCoinPanelActive { get; private set; }
    public bool IsGameOverPanelActive { get; private set; }
    public bool IsGameResultPanelActive { get; private set; }
    public bool IsStageOverPreviewPanelActive { get; private set; }
    public bool IsPausePanelActive { get; private set; }

    // Slider fill amount
    public float TimerRatio { get; private set; }
    public float BGMSliderRatio { get; private set; }
    public float SFXSliderRatio { get; private set; }

    // Hint 버튼 활성화 여부
    public bool IsOneColorHintActive { get; private set; }
    public bool IsOneZoneHintActive { get; private set; }

    public void ChangePauseTitleText(string title)
    {
        PauseTitle = title;
    }

    public void ChangeGameExitText(string content)
    {
        GameExitText = content;
    }

    public void ChangeSoundText(string bgmTitle, string sfxTitle, string leftText, string rightText)
    {
        BGMTitle = bgmTitle;
        SFXTitle = sfxTitle;
        BGMLeftText = leftText;
        SFXLeftText = leftText;
        BGMRightText = rightText;
        SFXRightText = rightText;
    }

    public void ActivateBottomContent(bool active)
    {
        IsBottomContentActive = active;
    }

    public void ActivateSkipBtn(bool active)
    {
        IsSkipBtnActive = active;
    }

    public void ActivateHint(bool oneColorHintActive, bool oneZoneHintActive)
    {
        IsOneColorHintActive = oneColorHintActive;
        IsOneZoneHintActive = oneZoneHintActive;
    }

    public void ChangeHintCost(string oneColorHintCost, string oneZoneHintCost)
    {
        OneColorHintCost = oneColorHintCost;
        OneZoneHintCost = oneZoneHintCost;
    }

    public void ChangeBGMSliderValue(float ratio, string leftSmallTxt, UnityEngine.Color handleColor)
    {
        BGMSliderRatio = ratio;
        BGMLeftText = leftSmallTxt;
    }

    public void ChangeSFXSliderValue(float ratio, string leftSmallTxt, UnityEngine.Color handleColor)
    {
        SFXSliderRatio = ratio;
        SFXLeftText = leftSmallTxt;
    }

    public void ChangeBGMSliderHandleColor(string leftSmallTxt, UnityEngine.Color handleColor)
    {
        BGMLeftText = leftSmallTxt;
    }

    public void ChangeSFXSliderHandleColor(string leftSmallTxt, UnityEngine.Color handleColor)
    {
        SFXLeftText = leftSmallTxt;
    }

    public void ActivatePausePanel(bool active)
    {
        IsPausePanelActive = active;
    }

    public void ActivateStageOverPreviewPanel(bool active)
    {
        IsStageOverPreviewPanelActive = active;
    }

    public void ChangeLastStagePattern(int currentStageCount, MapData data, UnityEngine.Color[] pickColors) { }

    public void ChangeStageOverInfo(string title, string info1, string info2)
    {
        StageOverTitle = title;
        StageOverInfo1 = info1;
        StageOverInfo2 = info2;
    }

    public void ActivatePlayPanel(bool active)
    {
        IsPlayPanelActive = active;
    }

    public void ChangeNowScore(int score)
    {
        NowScore = score.ToString();
    }

    public void ChangeBestScore(int score)
    {
        BestScore = score.ToString();
    }

    public void FillTimeSlider(float ratio)
    {
        TimerRatio = ratio;
    }

    public void ChangeTotalTime(string totalTime)
    {
        TotalTime = totalTime;
    }

    public void ChangeLeftTime(string leftTime, float ratio)
    {
        LeftTime = leftTime;
        TimerRatio = ratio;
    }

    public void ChangeStageCount(int stageCount)
    {
        StageText = stageCount.ToString();
    }

    public void ActivateRememberPanel(bool active, string rememberTxt)
    {
        IsRememberPanelActive = active;
        RememberText = rememberTxt;
    }

    public void ActivateCoinPanel(bool active)
    {
        IsCoinPanelActive = active;
    }

    public void ChangeCoinCount(string coinCount)
    {
        CoinCount = coinCount;
    }

    public void ActivateHintPanel(bool active)
    {
        IsHintPanelActive = active;
    }

    public void ActivateGameOverPanel(bool active)
    {
        IsGameOverPanelActive = active;
    }

    public void ChangeClearStageCount(string clearStageCount, string resultScore)
    {
        ClearStageCount = clearStageCount;
        ResultScore = resultScore;
    }

    public void AddClearPattern(SpawnableUI patternUI) { }
    public void RemoveClearPattern() { }

    public void ActivateGameResultPanel(bool active)
    {
        IsGameResultPanelActive = active;
    }

    public void ChangeResultGoldCount(string goldCount)
    {
        ResultGoldCount = goldCount;
    }

    public void AddRanking(SpawnableUI ranking, UnityEngine.Vector3 size) { }
    public void RemoveAllRanking() { }
}
