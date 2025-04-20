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
    TMP_Text _hintUsageText;
    TMP_Text _wrongCountText;

    RectTransform _bottomContent;
    Button _skipBtn;

    GameObject _rememberPanel;
    TMP_Text _hintInfoText;

    GameObject _gameClearPanel;
    TMP_Text _clearTitleText;
    TMP_Text _clearContentText;
    Button _nextStageBtn;
    Button _gameClearExitBtn;

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

    GameObject _gameResultPanel;
    TMP_Text _gameResultTitle;

    ArtworkUI _artworkUI;
    TMP_Text _hintUseCount;
    TMP_Text _wrongCount;

    Image _rankBackground;
    Image _rankIcon;
    TMP_Text _rankText;

    CustomProgressUI _currentCollectRatio;
    TMP_Text _currentCollectText;

    CustomProgressUI _totalCollectRatio;
    TMP_Text _totalCollectText;

    public CollectStageUIViewer(
         GameObject playPanel,
        TMP_Text titleText,
        GameObject timerContent,
        Image timerSlider,
        TMP_Text leftTimeText,
        TMP_Text totalTimeText,

        TMP_Text progressText,

        GameObject detailContent,
        TMP_Text hintUsageText,
        TMP_Text wrongCountText,

        RectTransform bottomContent,
        Button skipBtn,

        GameObject rememberPanel,
        TMP_Text hintInfoText,

        GameObject gameClearPanel,
        TMP_Text clearTitleText,
        TMP_Text clearContentText,
        Button nextStageBtn,
        Button clearExitBtn,

        GameObject pausePanel,
        Button pauseBtn,
        Button pauseExitBtn,
        Button gameExitBtn,

        CustomSlider bgmSlider,
        TMP_Text bgmMuteText,

        CustomSlider sfxSlider,
        TMP_Text sfxMuteText,

        GameObject gameResultPanel,
        TMP_Text gameResultTitle,

        ArtworkUI artworkUI,
        TMP_Text hintUseCount,
        TMP_Text wrongCount,

        Image rankBackground,
        Image rankIcon,
        TMP_Text rankText,

        CustomProgressUI currentCollectRatio,
        TMP_Text currentCollectText,

        CustomProgressUI totalCollectRatio,
        TMP_Text totalCollectText,

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
        _hintUsageText = hintUsageText;
        _wrongCountText = wrongCountText;

        _bottomContent = bottomContent;
        _skipBtn = skipBtn;

        _rememberPanel = rememberPanel;
        _hintInfoText = hintInfoText;

        _gameClearPanel = gameClearPanel;
        _clearTitleText = clearTitleText;
        _clearContentText = clearContentText;
        _nextStageBtn = nextStageBtn;
        _gameClearExitBtn = clearExitBtn;

        _pausePanel = pausePanel;
        _pauseBtn = pauseBtn;
        _gameExitBtn = gameExitBtn;
        _pauseExitBtn = pauseExitBtn;

        _gameResultPanel = gameResultPanel;
        _gameResultTitle = gameResultTitle;

        _artworkUI = artworkUI;
        _hintUseCount = hintUseCount;
        _wrongCount = wrongCount;
        _rankBackground = rankBackground;
        _rankIcon = rankIcon;
        _rankText = rankText;

        _currentCollectRatio = currentCollectRatio;
        _currentCollectText = currentCollectText;

        _totalCollectRatio = totalCollectRatio;
        _totalCollectText = totalCollectText;

        _bgmSlider = bgmSlider;
        _bgmMuteText = bgmMuteText;
        _bgmSliderHandle = bgmSlider.handleRect.GetComponent<Image>();

        _sfxSlider = sfxSlider;
        _sfxMuteText = sfxMuteText;
        _sfxSliderHandle = sfxSlider.handleRect.GetComponent<Image>();

        _bgmSlider.onHandlePointerUp += ((ratio) => { presenter.SaveBGMValue(); });
        _sfxSlider.onHandlePointerUp += ((ratio) => { presenter.SaveSFXValue(); });

        _gameExitBtn.onClick.AddListener(() => { presenter.OnClickGameExitBtn(); presenter.GoToResultState?.Invoke(); });
        _pauseBtn.onClick.AddListener(() => { presenter.ActivatePausePanel(true); });
        _pauseExitBtn.onClick.AddListener(() => { presenter.ActivatePausePanel(false); });
        _bgmSlider.onValueChanged.AddListener((ratio) => { presenter.OnBGMSliderValeChanged(ratio); });
        _sfxSlider.onValueChanged.AddListener((ratio) => { presenter.OnSFXSliderValeChanged(ratio); });
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
        _hintUsageText.text = $"{usage}회";
    }

    public void ChangeCurrentWrongCount(int wrongCount)
    {
        _wrongCountText.text = $"{wrongCount}회";
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
        _hintUseCount.text = hintUseCount.ToString();
        _wrongCount.text = wrongCount.ToString();
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

    public void ChangeTitle(string title)
    {
        _titleText.text = title;
    }

    public void ChangeHintInfoText(string infoText)
    {
        _hintInfoText.text = infoText;
    }

    public void ChangeProgressText(int progress)
    {
        _progressText.text = $"{progress}%";
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
        _bgmMuteText.text = leftSmallTxt;
    }

    public void ChangeSFXSliderHandleColor(string leftSmallTxt, Color handleColor)
    {
        _sfxSliderHandle.color = handleColor;
        _sfxMuteText.text = leftSmallTxt;
    }
}
