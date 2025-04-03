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

    GameObject _rememberPanel;
    TMP_Text _hintInfoText;

    GameObject _gameClearPanel;
    Image _cropArtworkImg;
    Button _nextStageBtn;
    Button _gameClearExitBtn;

    GameObject _pausePanel;
    Button _pauseBtn;
    Button _pauseExitBtn;
    Button _gameExitBtn;
    Slider _bgmSlider;
    Slider _sfxSlider;

    GameObject _gameResultPanel;
    ArtworkUI _artworkUI;
    TMP_Text _hintUseCount;
    TMP_Text _wrongCount;

    Image _rankBackground;
    Image _rankIcon;
    TMP_Text _rankText;

    Image _totalCollectRatio;
    TMP_Text _totalCollectText;


    public CollectStageUIViewer(
         GameObject playPanel,
        TMP_Text titleText,
        GameObject timerContent,
        Image timerSlider,
        TMP_Text leftTimeText,
        TMP_Text totalTimeText,

        TMP_Text progressText,

        GameObject rememberPanel,
        TMP_Text hintInfoText,

        GameObject gameClearPanel,
        Image cropArtworkImg,

        GameObject pausePanel,
        Button pauseBtn,
        Button pauseExitBtn,
        Button gameExitBtn,
        Slider bgmSlider,
        Slider sfxSlider,

        GameObject gameResultPanel,
        ArtworkUI artworkUI,
        TMP_Text hintUseCount,
        TMP_Text wrongCount,

        Image rankBackground,
        Image rankIcon,
        TMP_Text rankText,

        Image totalCollectRatio,
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

        _rememberPanel = rememberPanel;
        _hintInfoText = hintInfoText;

        _gameClearPanel = gameClearPanel;
        _cropArtworkImg = cropArtworkImg;

        _pausePanel = pausePanel;
        _pauseBtn = pauseBtn;
        _gameExitBtn = gameExitBtn;
        _pauseExitBtn = pauseExitBtn;
        _bgmSlider = bgmSlider;
        _sfxSlider = sfxSlider;

        _gameResultPanel = gameResultPanel;
        _artworkUI = artworkUI;
        _hintUseCount = hintUseCount;
        _wrongCount = wrongCount;
        _rankBackground = rankBackground;
        _rankIcon = rankIcon;
        _rankText = rankText;
        _totalCollectRatio = totalCollectRatio;
        _totalCollectText = totalCollectText;

        _gameExitBtn.onClick.AddListener(() =>
        {
            ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.HomeScene);
        });
        _pauseBtn.onClick.AddListener(() => { presenter.ActivatePausePanel(true); });
        _pauseExitBtn.onClick.AddListener(() => { presenter.ActivatePausePanel(false); });
        _bgmSlider.onValueChanged.AddListener((ratio) => { presenter.OnBGMSliderValeChanged(ratio); });
        _sfxSlider.onValueChanged.AddListener((ratio) => { presenter.OnSFXSliderValeChanged(ratio); });
    }

    public void ChangeArtwork(Sprite artSprite, Sprite artFrameSprite)
    {
        _artworkUI.Initialize(artSprite, artFrameSprite);
    }

    public void ChangeRank(Color rankColor, Sprite rankIcon, string rankName)
    {
        _rankBackground.color = rankColor;
        _rankIcon.sprite = rankIcon;
        _rankText.text = rankName;
    }

    public void ChangeGetRank(int hintUseCount, int wrongCount)
    {
        _hintUseCount.text = hintUseCount.ToString();
        _wrongCount.text = wrongCount.ToString();
    }

    public void ChangeCollectionRatio(int totalCollectRatio)
    {
        _totalCollectRatio.fillAmount = totalCollectRatio;
        _totalCollectText.text = ((int)totalCollectRatio * 100).ToString();
    }





    public void ActivatePausePanel(bool active)
    {
        _pausePanel.SetActive(active);
    }


    public void ActivatePlayPanel(bool active)
    {
        _playPanel.SetActive(active);
    }

    public void ChangeTitle(string title, string comment)
    {
        _titleText.text = $"<color=#1a1817>{title}</color>{comment}";
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

    public void ActivateGameClearPanel(bool active)
    {
        _gameClearPanel.SetActive(active);
    }

    public void ChangeCropArtworkImg(Sprite artSprite)
    {
        _cropArtworkImg.sprite = artSprite;
    }

    public void ActivateGameResultPanel(bool active)
    {
        _gameResultPanel.SetActive(active);
    }

    public void ChangeBGMSliderValue(float ratio)
    {
        _bgmSlider.value = ratio;
    }

    public void ChangeSFXSliderValue(float ratio)
    {
        _sfxSlider.value = ratio;
    }
}
