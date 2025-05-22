using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChallengeStageUIViewer
{
    void ChangePauseTitleText(string title);
    void ChangeGameExitText(string content);
    void ChangeSoundText(string bgmTitle, string sfxTitle, string leftText, string rightText);

    void ActivateBottomContent(bool active);
    void ActivateSkipBtn(bool active);
    void ActivateHint(bool oneColorHintActive, bool oneZoneHintActive);

    void ChangeHintCost(string oneColorHintCost, string oneZoneHintCost);

    void ChangeBGMSliderValue(float ratio, string leftSmallTxt, Color handleColor);
    void ChangeSFXSliderValue(float ratio, string leftSmallTxt, Color handleColor);

    void ChangeBGMSliderHandleColor(string leftSmallTxt, Color handleColor);
    void ChangeSFXSliderHandleColor(string leftSmallTxt, Color handleColor);

    void ActivatePausePanel(bool active);
    void ActivateStageOverPreviewPanel(bool active);

    void ChangeLastStagePattern(int currentStageCount, MapData data, Color[] pickColors);
    void ChangeStageOverInfo(string stageOverTitleText, string stageOverInfo1Text, string stageOverInfo2Text);

    void ActivatePlayPanel(bool active);
    void ChangeNowScore(int score);
    void ChangeBestScore(int score);

    void FillTimeSlider(float ratio);
    void ChangeTotalTime(string totalTime);
    void ChangeLeftTime(string leftTime, float ratio);

    void ChangeStageCount(int stageCount);

    void ActivateRememberPanel(bool active, string rememberTxt);
    void ActivateCoinPanel(bool active);
    void ChangeCoinCount(string coinCount);

    void ActivateHintPanel(bool active);
    void ActivateGameOverPanel(bool active);
    void ChangeClearStageCount(string clearStageCount, string resultScore);

    void AddClearPattern(SpawnableUI patternUI);
    void RemoveClearPattern();

    void ActivateGameResultPanel(bool active);
    void ChangeResultGoldCount(string goldCount);

    void AddRanking(SpawnableUI ranking, Vector3 size);
    void RemoveAllRanking();
}
