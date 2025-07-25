using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectStageUIViewer : ICollectStageUIViewer
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

    Button _goBackBtn;
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
    Button _pauseGameExitBtn;
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

    ArtworkPreviewUI _artworkPreviewUI;
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
    Button _nextBtn;

    Button _openShareBtn;

    GameObject _sharePanel;
    TMP_Text _shareTitle;
    Button _shareBtn;
    Button _shareExitBtn;

    ShareArtworkUI[] _shareArtworks;

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

        Button goBackBtn,
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
        Button pauseGameExitBtn,

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

        ArtworkPreviewUI artworkPreviewUI,
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
        Button nextBtn,

        Button openShareBtn,

        GameObject sharePanel,
        TMP_Text shareTitle,
        Button shareBtn,
        Button shareExitBtn,

        ShareArtworkUI[] shareArtworks,
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
        _skipBtn.onClick.AddListener(() => { presenter.OnClickSkipBtn?.Invoke(); });

        _rememberPanel = rememberPanel;
        _rememberTxt = rememberTxt;
        _hintInfoText = hintInfoText;
        _goBackBtn = goBackBtn;
        _goBackBtn.onClick.AddListener(() => { presenter.OnClickGoBackHint?.Invoke(); });

        _gameClearPanel = gameClearPanel;
        _clearTitleText = clearTitleText;
        _clearContentText = clearContentText;

        _nextStageBtn = nextStageBtn;
        _nextStageBtn.onClick.AddListener(() => { presenter.OnClickNextStageBtn?.Invoke(); });

        _gameClearExitBtn = clearExitBtn;
        _gameClearExitBtn.onClick.AddListener(() => { presenter.OnClickClearExitBtn?.Invoke(); });

        _pausePanel = pausePanel;
        _pauseTitleText = pauseTitleText;
        _pauseBtn = pauseBtn;
        _pauseGameExitBtn = pauseGameExitBtn;
        _gameExitText = _pauseGameExitBtn.GetComponentInChildren<TMP_Text>();

        _pauseExitBtn = pauseExitBtn;

        _gameResultPanel = gameResultPanel;
        _gameResultTitle = gameResultTitle;

        _artworkPreviewUI = artworkPreviewUI;
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
        _nextBtn = nextBtn;
        _nextBtn.onClick.AddListener(() => { presenter.OnClickNextBtn?.Invoke(); });

        _openShareBtn = openShareBtn;
        _openShareBtn.onClick.AddListener(() => { presenter.GoToShareState?.Invoke(); });

        _sharePanel = sharePanel;
        _shareTitle = shareTitle;
        _shareBtn = shareBtn;
        _shareExitBtn = shareExitBtn;

        _shareArtworks = shareArtworks;

        _shareExitBtn.onClick.AddListener(() => { presenter.ExitShareState?.Invoke(); });
        _shareBtn.onClick.AddListener(() => presenter.OnShareButtonClick?.Invoke());

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

        _pauseGameExitBtn.onClick.AddListener((() => { presenter.ActivatePausePanel(false); presenter.OnClickPauseGameExitBtn?.Invoke(); }));
        
        _pauseBtn.onClick.AddListener(() => 
        { 
            presenter.ActivatePausePanel(true); 
        });

        _pauseExitBtn.onClick.AddListener(() => 
        {
            presenter.ActivatePausePanel(false);
        });

        _bgmSlider.onValueChanged.AddListener((ratio) => { presenter.OnBGMSliderValeChanged(ratio); });
        _sfxSlider.onValueChanged.AddListener((ratio) => { presenter.OnSFXSliderValeChanged(ratio); });
    }

    public void ChangeShareTitle(string title)
    {
        _shareTitle.text = title;
    }

    public void ActivateOpenShareBtnInteraction(bool activeOpenShareBtn)
    {
        _openShareBtn.interactable = activeOpenShareBtn;
    }

    public void ChangeShareArtworks(Sprite[] shareArtSprites, ArtworkData[] shareArtworkDatas)
    {
        for (int i = 0; i < shareArtworkDatas.Length; i++)
        {
            _shareArtworks[i].Initialize(shareArtSprites[i], shareArtworkDatas[i].Title, shareArtworkDatas[i].Description);
        }
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

    public void ChangeCurrentHintUsage(string usage)
    {
        _hintUsageText.text = usage;
    }

    public void ChangeCurrentWrongCount(string wrongCount)
    {
        _wrongCountText.text = wrongCount;
    }

    public void ChangeArtworkPreview(Sprite artSprite, Sprite artFrameSprite, Sprite rankDecorationIcon)
    {
        _artworkPreviewUI.Initialize(artSprite, artFrameSprite, rankDecorationIcon);
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

    public void ChangeGetRank(string hintUseCount, string wrongCount)
    {
        _totalHintUseCount.text = hintUseCount;
        _totalWrongCount.text = wrongCount;
    }

    public void ChangeCollectionRatio(
        float currentCollectRatio, 
        float totalCollectRatio,
        string currentCollectRatioString,
        string totalCollectRatioString)
    {
        _currentCollectRatio.FillValue = currentCollectRatio;
        _currentCollectText.text = currentCollectRatioString;

        _totalCollectRatio.FillValue = totalCollectRatio;
        _totalCollectText.text = totalCollectRatioString;
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

    public void ChangeProgressText(string progress)
    {
        _progressText.text = progress;
    }

    public void FillTimeSlider(float ratio)
    {
        _timerSlider.fillAmount = ratio;
    }

    public void ChangeTotalTime(string totalTime)
    {
        _totalTimeText.text = totalTime;
    }

    public void ChangeLeftTime(string leftTime, float ratio)
    {
        _leftTimeText.text = leftTime;
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
