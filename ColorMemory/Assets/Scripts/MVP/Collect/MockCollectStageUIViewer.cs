using System;
using UnityEngine;
using UnityEngine.UI;

public class MockCollectStageUIViewer : ICollectStageUIViewer
{
    // Texts
    public string TitleText { get; set; }
    public string LeftTimeText { get; set; }
    public string TotalTimeText { get; set; }
    public string ProgressText { get; set; }

    public string HintUsageTitle { get; set; }
    public string HintUsageText { get; set; }
    public string WrongCountTitle { get; set; }
    public string WrongCountText { get; set; }

    public string RememberTxt { get; set; }
    public string HintInfoText { get; set; }

    public string ClearTitleText { get; set; }
    public string ClearContentText { get; set; }

    public string PauseTitleText { get; set; }
    public string GameExitText { get; set; }

    public string BGMTitleText { get; set; }
    public string BGMLeftText { get; set; }
    public string BGMRightText { get; set; }

    public string SFXTitleText { get; set; }
    public string SFXLeftText { get; set; }
    public string SFXRightText { get; set; }

    public string GameResultTitle { get; set; }
    public string ArtworkTitle { get; set; }

    public string GetRankTitle { get; set; }
    public string TotalHintUsageTitle { get; set; }
    public string TotalWrongCountTitle { get; set; }
    public string TotalHintUseCount { get; set; }
    public string TotalWrongCount { get; set; }
    public string RankText { get; set; }

    public string MyCollectionTitle { get; set; }
    public string CurrentCollectTitle { get; set; }
    public string CurrentCollectText { get; set; }
    public string TotalCollectTitle { get; set; }
    public string TotalCollectText { get; set; }

    public string ShareTitle { get; set; }

    // Toggles (Active GameObjects)
    public bool PlayPanelActive { get; set; }
    public bool TimerContentActive { get; set; }
    public bool DetailContentActive { get; set; }
    public bool BottomContentActive { get; set; }
    public bool GameClearPanelActive { get; set; }
    public bool PausePanelActive { get; set; }
    public bool GameResultPanelActive { get; set; }
    public bool RememberPanelActive { get; set; }
    public bool SharePanelActive { get; set; }

    public bool SkipBtnActive { get; set; }
    public bool NextStageBtnActive { get; set; }
    public bool ClearExitBtnActive { get; set; }
    public bool OpenShareBtnActive { get; set; }
    public bool ShareBtnActive { get; set; }
    public bool ShareExitBtnActive { get; set; }

    // Sliders and Progress
    public float TimerSliderRatio { get; set; }
    public float CurrentCollectRatio { get; set; }
    public float TotalCollectRatio { get; set; }
    public float BGMSliderValue { get; set; }
    public float SFXSliderValue { get; set; }

    public void ChangeTitle(string title) => TitleText = title;
    public void ChangeLeftTime(string leftTime, float ratio)
    {
        LeftTimeText = leftTime;
        TimerSliderRatio = ratio;
    }
    public void ChangeTotalTime(string totalTime) => TotalTimeText = totalTime;
    public void ChangeProgressText(string progress) => ProgressText = progress;

    public void ChangeDetailTitle(string hintUsageTitle, string wrongCountTitle)
    {
        HintUsageTitle = hintUsageTitle;
        WrongCountTitle = wrongCountTitle;
    }

    public void ChangeCurrentHintUsage(string usage) => HintUsageText = usage;
    public void ChangeCurrentWrongCount(string wrongCount) => WrongCountText = wrongCount;

    public void ActivatePlayPanel(bool active) => PlayPanelActive = active;
    public void ActivateTimerContent(bool active) => TimerContentActive = active;
    public void ActivateDetailContent(bool active) => DetailContentActive = active;
    public void ActivateBottomContent(bool active) => BottomContentActive = active;
    public void ActivateGameClearPanel(bool active) => GameClearPanelActive = active;
    public void ActivatePausePanel(bool active) => PausePanelActive = active;
    public void ActivateGameResultPanel(bool active) => GameResultPanelActive = active;
    public void ActivateRememberPanel(bool active) => RememberPanelActive = active;
    public void ActivateSharePanel(bool active) => SharePanelActive = active;

    public void ActivateSkipBtn(bool active) => SkipBtnActive = active;
    public void ActivateNextStageBtn(bool active) => NextStageBtnActive = active;
    public void ActivateClearExitBtn(bool active) => ClearExitBtnActive = active;
    public void ActivateOpenShareBtnInteraction(bool active) => OpenShareBtnActive = active;
    public void ActivateShareBottomItems(bool active)
    {
        ShareBtnActive = active;
        ShareExitBtnActive = active;
    }

    public void ChangeClearTitleText(string title) => ClearTitleText = title;
    public void ChangeClearContentInfo(string content) => ClearContentText = content;
    public void ChangePauseTitleText(string title) => PauseTitleText = title;
    public void ChangeGameExitText(string content) => GameExitText = content;

    public void ChangeSoundText(string bgmTitle, string sfxTitle, string leftText, string rightText)
    {
        BGMTitleText = bgmTitle;
        SFXTitleText = sfxTitle;
        BGMLeftText = leftText;
        SFXLeftText = leftText;
        BGMRightText = rightText;
        SFXRightText = rightText;
    }

    public void ChangeGameResultTitle(string comment) => GameResultTitle = comment;
    public void ChangeArtworkTitle(string artworkTitle) => ArtworkTitle = artworkTitle;

    public void ChangeGetRankTitle(string rankTitle, string hintTitle, string wrongTitle)
    {
        GetRankTitle = rankTitle;
        TotalHintUsageTitle = hintTitle;
        TotalWrongCountTitle = wrongTitle;
    }

    public void ChangeGetRank(string hintUseCount, string wrongCount)
    {
        TotalHintUseCount = hintUseCount;
        TotalWrongCount = wrongCount;
    }

    public void ChangeMyCollectionTitle(string title, string currentTitle, string totalTitle)
    {
        MyCollectionTitle = title;
        CurrentCollectTitle = currentTitle;
        TotalCollectTitle = totalTitle;
    }

    public void ChangeCollectionRatio(float currentRatio, float totalRatio, string currentText, string totalText)
    {
        CurrentCollectRatio = currentRatio;
        CurrentCollectText = currentText;
        TotalCollectRatio = totalRatio;
        TotalCollectText = totalText;
    }

    public void ChangeHintInfoText(string infoText) => HintInfoText = infoText;
    public void ChangeShareTitle(string title) => ShareTitle = title;

    public void ChangeBGMSliderValue(float ratio, string leftSmallTxt, Color handleColor)
    {
        BGMSliderValue = ratio;
        BGMLeftText = leftSmallTxt;
    }

    public void ChangeSFXSliderValue(float ratio, string leftSmallTxt, Color handleColor)
    {
        SFXSliderValue = ratio;
        SFXLeftText = leftSmallTxt;
    }

    public void ChangeShareArtworks(Sprite[] shareArtSprites, ArtworkData[] shareArtworkDatas) { }

    public void ChangeArtworkPreview(Sprite artSprite, Sprite artFrameSprite, Sprite rankDecorationIcon) { }
    public void ChangeArtwork(Sprite artSprite, Sprite artFrameSprite, Sprite rankDecorationIcon, bool hasIt) { }

    public void ChangeRank(Sprite rankIcon, bool activeIcon, string rankName, Color rankBackgroundColor) { }

    public void FillTimeSlider(float ratio) { }

    public void ChangeBGMSliderHandleColor(string leftSmallTxt, Color handleColor) { }

    public void ChangeSFXSliderHandleColor(string leftSmallTxt, Color handleColor) { }
}