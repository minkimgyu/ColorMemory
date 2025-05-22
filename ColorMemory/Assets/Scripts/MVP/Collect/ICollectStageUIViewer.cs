using UnityEngine;

public interface ICollectStageUIViewer
{
    void ChangeShareTitle(string title);
    void ActivateOpenShareBtnInteraction(bool activeOpenShareBtn);
    void ChangeShareArtworks(Sprite[] shareArtSprites, ArtworkData[] shareArtworkDatas);
    void ActivateSharePanel(bool activeSharePanel);
    void ActivateShareBottomItems(bool activeShareBottomItems);

    void ChangeArtworkTitle(string artworkTitle);
    void ChangeDetailTitle(string hintUsageTitle, string wrongCountTitle);
    void ChangeGetRankTitle(string getRankTitle, string hintUsageTitle, string wrongCountTitle);
    void ChangeMyCollectionTitle(string myCollectionTitle, string currentCollectTitle, string totalCollectTitle);

    void ChangePauseTitleText(string title);
    void ChangeGameExitText(string content);
    void ChangeSoundText(string bgmTitle, string sfxTitle, string leftText, string rightText);
    void ChangeGameResultTitle(string comment);

    void ActivateBottomContent(bool active);
    void ActivateSkipBtn(bool active);
    void ActivateGameClearPanel(bool active);
    void ChangeClearTitleText(string title);
    void ChangeClearContentInfo(string content);
    void ActivateNextStageBtn(bool active);
    void ActivateClearExitBtn(bool active);

    void ActivateDetailContent(bool active);
    void ChangeCurrentHintUsage(string usage);
    void ChangeCurrentWrongCount(string wrongCount);

    void ChangeArtworkPreview(Sprite artSprite, Sprite artFrameSprite, Sprite rankDecorationIcon);
    void ChangeArtwork(Sprite artSprite, Sprite artFrameSprite, Sprite rankDecorationIcon, bool hasIt);
    void ChangeRank(Sprite rankIcon, bool activeIcon, string rankName, Color rankBackgroundColor);
    void ChangeGetRank(string hintUseCount, string wrongCount);

    void ChangeCollectionRatio(float currentCollectRatio, float totalCollectRatio, string currentCollectRatioString, string totalCollectRatioString);

    void ActivatePausePanel(bool active);
    void ActivatePlayPanel(bool active);

    void ChangeTitle(string title);
    void ChangeHintInfoText(string infoText);
    void ChangeProgressText(string progress);

    void FillTimeSlider(float ratio);
    void ChangeTotalTime(string totalTime);
    void ChangeLeftTime(string leftTime, float ratio);
    void ActivateTimerContent(bool active);

    void ActivateRememberPanel(bool active);
    void ActivateGameResultPanel(bool active);

    void ChangeBGMSliderValue(float ratio, string leftSmallTxt, Color handleColor);
    void ChangeSFXSliderValue(float ratio, string leftSmallTxt, Color handleColor);
    void ChangeBGMSliderHandleColor(string leftSmallTxt, Color handleColor);
    void ChangeSFXSliderHandleColor(string leftSmallTxt, Color handleColor);
}
