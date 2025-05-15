using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectStageUIViewer
{
    GameObject _playPanel;

    TMP_Text _titleText;

    GameObject _timerContent;
    Image _timerSlider;
    TMP_Text _leftTimeText;
    TMP_Text _totalTimeText;

    TMP_Text _progressText;

    GameObject _detailContent;

    TMP_Text _hintUsageTitle;
    TMP_Text _hintUsageText;

    TMP_Text _wrongCountTitle;
    TMP_Text _wrongCountText;

    RectTransform _bottomContent;
    Button _skipBtn;

    GameObject _rememberPanel;
    TMP_Text _rememberTxt;
    TMP_Text _hintInfoText;

    GameObject _gameClearPanel;
    TMP_Text _clearTitleText;
    TMP_Text _clearContentText;
    Button _nextStageBtn;
    Button _gameClearExitBtn;

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

    GameObject _gameResultPanel;
    TMP_Text _gameResultTitle;

    ArtworkUI _artworkUI;

    TMP_Text _artworkTitle;

    TMP_Text _getRankTitle;
    TMP_Text _totalHintUsageTitle;
    TMP_Text _totalWrongCountTitle;

    TMP_Text _totalHintUseCount;
    TMP_Text _totalWrongCount;

    Image _rankBackground;
    Image _rankIcon;
    TMP_Text _rankText;

    TMP_Text _myCollectionTitle;

    TMP_Text _currentCollectTitle;
    CustomProgressUI _currentCollectRatio;
    TMP_Text _currentCollectText;

    TMP_Text _totalCollectTitle;
    CustomProgressUI _totalCollectRatio;
    TMP_Text _totalCollectText;

    Button _openShareBtn;

    GameObject _sharePanel;
    Button _shareBtn;
    Button _shareExitBtn;

    public CollectStageUIViewer(
        GameObject playPanel,
        TMP_Text titleText,
        GameObject timerContent,
        Image timerSlider,
        TMP_Text leftTimeText,
        TMP_Text totalTimeText,

        TMP_Text progressText,

        GameObject detailContent,
        TMP_Text hintUsageTitle,
        TMP_Text hintUsageText,

        TMP_Text wrongCountTitle,
        TMP_Text wrongCountText,

        RectTransform bottomContent,
        Button skipBtn,

        GameObject rememberPanel,
        TMP_Text rememberTxt,
        TMP_Text hintInfoText,

        GameObject gameClearPanel,
        TMP_Text clearTitleText,
        TMP_Text clearContentText,
        Button nextStageBtn,
        Button clearExitBtn,

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

        GameObject gameResultPanel,
        TMP_Text gameResultTitle,

        ArtworkUI artworkUI,

        TMP_Text artworkTitle,

        TMP_Text getRankTitle,
        TMP_Text totalHintUsageTitle,
        TMP_Text totalWrongCountTitle,

        TMP_Text totalHintUseCount,
        TMP_Text totalWrongCount,

        Image rankBackground,
        Image rankIcon,
        TMP_Text rankText,

        TMP_Text myCollectionTitle,

        TMP_Text currentCollectTitle,
        CustomProgressUI currentCollectRatio,
        TMP_Text currentCollectText,

        TMP_Text totalCollectTitle,
        CustomProgressUI totalCollectRatio,
        TMP_Text totalCollectText,

        Button openShareBtn,

        GameObject sharePanel,
        Button shareBtn,
        Button shareExitBtn,

        CollectStageUIPresenter presenter)
    {
        _playPanel = playPanel;

        _titleText = titleText;
        _timerContent = timerContent;
        _timerSlider = timerSlider;
        _leftTimeText = leftTimeText;
        _totalTimeText = totalTimeText;

        _progressText = progressText;

        _detailContent = detailContent;
        _hintUsageTitle = hintUsageTitle;
        _hintUsageText = hintUsageText;

        _wrongCountTitle = wrongCountTitle;
        _wrongCountText = wrongCountText;

        _bottomContent = bottomContent;
        _skipBtn = skipBtn;

        _rememberPanel = rememberPanel;
        _rememberTxt = rememberTxt;
        _hintInfoText = hintInfoText;

        _gameClearPanel = gameClearPanel;
        _clearTitleText = clearTitleText;
        _clearContentText = clearContentText;
        _nextStageBtn = nextStageBtn;
        _gameClearExitBtn = clearExitBtn;

        _pausePanel = pausePanel;
        _pauseTitleText = pauseTitleText;
        _pauseBtn = pauseBtn;
        _gameExitBtn = gameExitBtn;
        _gameExitText = _gameExitBtn.GetComponentInChildren<TMP_Text>();

        _pauseExitBtn = pauseExitBtn;

        _gameResultPanel = gameResultPanel;
        _gameResultTitle = gameResultTitle;

        _artworkUI = artworkUI;
        _artworkTitle = artworkTitle;

        _getRankTitle = getRankTitle;

        _totalHintUsageTitle = totalHintUsageTitle;
        _totalWrongCountTitle = totalWrongCountTitle;

        _totalHintUseCount = totalHintUseCount;
        _totalWrongCount = totalWrongCount;
        _rankBackground = rankBackground;
        _rankIcon = rankIcon;
        _rankText = rankText;

        _myCollectionTitle = myCollectionTitle;

        _currentCollectTitle = currentCollectTitle;
        _currentCollectRatio = currentCollectRatio;
        _currentCollectText = currentCollectText;

        _totalCollectTitle = totalCollectTitle;
        _totalCollectRatio = totalCollectRatio;
        _totalCollectText = totalCollectText;

        _openShareBtn = openShareBtn;
        _openShareBtn.onClick.AddListener(() => { presenter.ActivateSharePanel(true); });

        _sharePanel = sharePanel;
        _shareBtn = shareBtn;
        _shareExitBtn = shareExitBtn;

        _shareExitBtn.onClick.AddListener(() => { presenter.ActivateSharePanel(false); });
        _shareBtn.onClick.AddListener(() => presenter.OnClickShare());

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

        _gameExitBtn.onClick.AddListener(() => { presenter.OnClickGameExitBtn(); presenter.GoToResultState?.Invoke(); });
        _pauseBtn.onClick.AddListener(() => { presenter.ActivatePausePanel(true); });
        _pauseExitBtn.onClick.AddListener(() => { presenter.ActivatePausePanel(false); });
        _bgmSlider.onValueChanged.AddListener((ratio) => { presenter.OnBGMSliderValeChanged(ratio); });
        _sfxSlider.onValueChanged.AddListener((ratio) => { presenter.OnSFXSliderValeChanged(ratio); });
    }

    public void ActivateSharePanel(bool activeSharePanel)
    {
        _sharePanel.SetActive(activeSharePanel);
    }

    public void ActivateShareBottomItems(bool activeShareBottomItems)
    {
        _shareBtn.gameObject.SetActive(activeShareBottomItems);
        _shareExitBtn.gameObject.SetActive(activeShareBottomItems);
    }


    public void ChangeArtworkTitle(string artworkTitle)
    {
        _artworkTitle.text = artworkTitle;
    }

    public void ChangeDetailTitle(string hintUsageTitle, string wrongCountTitle)
    {
        _hintUsageTitle.text = hintUsageTitle;
        _wrongCountTitle.text = wrongCountTitle;
    }

    public void ChangeGetRankTitle(string getRankTitle, string hintUsageTitle, string wrongCountTitle)
    {
        _getRankTitle.text = getRankTitle;
        _totalHintUsageTitle.text = hintUsageTitle;
        _totalWrongCountTitle.text = wrongCountTitle;
    }


    public void ChangeMyCollectionTitle(string myCollectionTitle, 
        string currentCollectTitle, 
        string totalCollectTitle)
    {
        _myCollectionTitle.text = myCollectionTitle;
        _currentCollectTitle.text = currentCollectTitle;
        _totalCollectTitle.text = totalCollectTitle;
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

    public void ChangeGameResultTitle(string comment)
    {
        _gameResultTitle.text = comment; // "축하해요! 새로운 명화를 획득했어요!";
    }

    public void ActivateBottomContent(bool active)
    {
        _bottomContent.gameObject.SetActive(active);
    }

    public void ActivateSkipBtn(bool active)
    {
        _skipBtn.gameObject.SetActive(active);
    }

    public void ActivateGameClearPanel(bool active)
    {
        _gameClearPanel.SetActive(active);
    }

    public void ChangeClearTitleText(string title)
    {
        _clearTitleText.text = title;
    }

    public void ChangeClearContentInfo(string content)
    {
        _clearContentText.text = content;
    }

    public void ActivateNextStageBtn(bool active)
    {
        _nextStageBtn.gameObject.SetActive(active);
    }

    public void ActivateClearExitBtn(bool active)
    {
        _gameClearExitBtn.gameObject.SetActive(active);
    }


    public void ActivateDetailContent(bool active)
    {
        _detailContent.SetActive(active);
    }

    public void ChangeCurrentHintUsage(int usage)
    {
        string usageFormat;
        if (usage > 1) usageFormat = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.Counts);
        else usageFormat = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.Count);

        _hintUsageText.text = string.Format(usageFormat, usage);
    }

    public void ChangeCurrentWrongCount(int wrongCount)
    {
        string wrongFormat;
        if (wrongCount > 1) wrongFormat = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.Counts);
        else wrongFormat = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.Count);

        _wrongCountText.text = string.Format(wrongFormat, wrongCount);
    }



    public void ChangeArtwork(Sprite artSprite, Sprite artFrameSprite, Sprite rankDecorationIcon, bool hasIt)
    {
        _artworkUI.Initialize(artSprite, artFrameSprite, rankDecorationIcon, hasIt);
    }

    public void ChangeRank(Sprite rankIcon, bool activeIcon, string rankName, Color rankBackgroundColor)
    {
        _rankIcon.gameObject.SetActive(activeIcon);
        _rankIcon.sprite = rankIcon;
        _rankText.text = rankName;
        _rankBackground.color = rankBackgroundColor;
    }

    public void ChangeGetRank(int hintUseCount, int wrongCount)
    {
        string usageFormat;
        if (hintUseCount > 1) usageFormat = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.Counts);
        else usageFormat = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.Count);

        string wrongFormat;
        if (wrongCount > 1) wrongFormat = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.Counts);
        else wrongFormat = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.Count);

        _totalHintUseCount.text = string.Format(usageFormat, hintUseCount);
        _totalWrongCount.text = string.Format(wrongFormat, wrongCount);
    }

    public void ChangeCollectionRatio(float currentCollectRatio, float totalCollectRatio)
    {
        _currentCollectRatio.FillValue = currentCollectRatio;
        _currentCollectText.text = $"{Mathf.RoundToInt(currentCollectRatio * 100)}%";

        _totalCollectRatio.FillValue = totalCollectRatio;
        _totalCollectText.text = $"{Mathf.RoundToInt(totalCollectRatio * 100)}%";
    }


    public void ActivatePausePanel(bool active)
    {
        _pausePanel.SetActive(active);
    }


    public void ActivatePlayPanel(bool active)
    {
        _playPanel.SetActive(active);
    }

    const int maxTitleSize = 13;
    const int dotSize = 3;

    public void ChangeTitle(string title, int currentSection, int totalSectionSize)
    {
        if (title.Length > maxTitleSize) title = title.Substring(0, maxTitleSize - dotSize) + "...";
        _titleText.text = title + " (" + currentSection + "/" + totalSectionSize + ")";
    }

    public void ChangeHintInfoText(string infoText)
    {
        _hintInfoText.text = infoText;
    }

    public void ChangeProgressText(int progress)
    {
        _progressText.text = $"{ progress }%";
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

    public void ActivateTimerContent(bool active)
    {
        _timerContent.SetActive(active);
    }

    public void ActivateRememberPanel(bool active)
    {
        _rememberPanel.SetActive(active);
        _rememberTxt.text = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.RememberTitle);
    }

    public void ActivateGameResultPanel(bool active)
    {
        _gameResultPanel.SetActive(active);
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
}
