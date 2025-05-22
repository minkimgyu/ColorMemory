using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class CollectStageUIPresenter
{
    CollectStageUIModel _model;
    ICollectStageUIViewer _viewer;

    public Action OnClickPauseGameExitBtn { get; set; }
    public Action GoToShareState { get; set; }
    public Action ExitShareState { get; set; }
    public Action OnShareButtonClick { get; set; }

    public Action OnClickSkipBtn { get; set; }
    public Action OnClickNextBtn { get; set; }

    public Action OnClickClearExitBtn { get; set; }
    public Action OnClickNextStageBtn { get; set; }
    public Action OnClickGoBackHint { get; set; }


    public CollectStageUIPresenter(
        CollectStageUIModel model)
    {
        _model = model;

        OnClickPauseGameExitBtn += () => { ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.BtnClick); };
        GoToShareState += () => { ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.BtnClick); };
        ExitShareState += () => { ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.BtnClick); };
        OnShareButtonClick += () => { ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.BtnClick); };
        
        OnClickNextBtn += () => { ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.BtnClick); };
        
        OnClickClearExitBtn += () => { ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.BtnClick); };
        OnClickNextStageBtn += () => { ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.BtnClick); };

        OnClickSkipBtn += () => { ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.HintClick); };
        OnClickGoBackHint += () => { ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.HintClick); };
    }

    public void InjectViewer(ICollectStageUIViewer viewer)
    {
        _viewer = viewer;

        _model.PauseTitleText = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.GameSettingTitle);
        _viewer.ChangePauseTitleText(_model.PauseTitleText);

        _model.GameExitText = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.GameExitBtn);
        _viewer.ChangeGameExitText(_model.GameExitText);

        _model.BGMTitleText = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.BGMTitle);
        _model.SfxTitleText = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.SFXTitle);
        _model.SoundLeftText = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.DecreaseSound);
        _model.SoundRightText = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.IncreaseSound);
        _viewer.ChangeSoundText(_model.BGMTitleText, _model.SfxTitleText, _model.SoundLeftText, _model.SoundRightText);

        _model.HintUsageTitle = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.HintUsage);
        _model.WrongCountTitle = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.WrongCount);
        _viewer.ChangeDetailTitle(_model.HintUsageTitle, _model.WrongCountTitle);
    }

    public void ChangeShareTitle(string title)
    {
        _model.ShareTitle = title;
        _viewer.ChangeShareTitle(_model.ShareTitle);
    }

    public void ChangeShareArtworks(Sprite[] shareArtSprites, ArtworkData[] shareArtworkDatas)
    {
        _model.ShareArtSprites = shareArtSprites;
        _model.ShareArtworkDatas = shareArtworkDatas;
        _viewer.ChangeShareArtworks(shareArtSprites, shareArtworkDatas);
    }

    public void ActivateSharePanel(bool activeSharePanel)
    {
        _model.ActiveSharePanel = activeSharePanel;
        _viewer.ActivateSharePanel(_model.ActiveSharePanel);
    }

    public void ActivateShareBottomItems(bool activeSharePanel)
    {
        _model.ActiveShareBottomItems = activeSharePanel;
        _viewer.ActivateShareBottomItems(_model.ActiveShareBottomItems);
    }

    public void ChangeArtworkTitle(string artworkTitle)
    {
        _model.ArtworkTitle = artworkTitle;
        _viewer.ChangeArtworkTitle(_model.ArtworkTitle);
    }
    public void ChangeGetRankTitle(string getRankTitle, string hintUsageTitle, string wrongCountTitle)
    {
        _model.GetRankTitle = getRankTitle;
        _model.TotalHintUsageTitle = hintUsageTitle;
        _model.TotalWrongCountTitle = wrongCountTitle;
        _viewer.ChangeGetRankTitle(_model.GetRankTitle, _model.TotalHintUsageTitle, _model.TotalWrongCountTitle);
    }


    public void ChangeMyCollectionTitle(string myCollectionTitle,
        string currentCollectTitle,
        string totalCollectTitle)
    {
        _model.MyCollectionTitle = myCollectionTitle;
        _model.CurrentCollectTitle = currentCollectTitle;
        _model.TotalCollectTitle = totalCollectTitle;
        _viewer.ChangeMyCollectionTitle(_model.MyCollectionTitle, _model.CurrentCollectTitle, _model.TotalCollectTitle);
    }

    public void ActivateOpenShareBtnInteraction(bool activeOpenShareBtn)
    {
        _model.ActiveOpenShareBtn = activeOpenShareBtn;
        _viewer.ActivateOpenShareBtnInteraction(_model.ActiveOpenShareBtn);
    }

    public void ChangeGameResultTitle(string resultTitle)
    {
        _model.GameResultTitle = resultTitle;
        _viewer.ChangeGameResultTitle(_model.GameResultTitle);
    }


    public void ActivateBottomContent(bool active)
    {
        _model.ActiveBottomContent = active;
        _viewer.ActivateBottomContent(_model.ActiveBottomContent);
    }

    public void ActivateSkipBtn(bool active)
    {
        _model.ActiveSkipBtn = active;
        _viewer.ActivateSkipBtn(_model.ActiveSkipBtn);
    }


    public void ActivateGameClearPanel(bool active)
    {
        _model.ActiveGameClearPanel = active;
        _viewer.ActivateGameClearPanel(_model.ActiveGameClearPanel);
    }

    public void ChangeClearTitleInfo(string title)
    {
        _model.ClearTitleInfo = title;
        _viewer.ChangeClearTitleText(_model.ClearTitleInfo);
    }

    public void ChangeClearContentInfo(string title)
    {
        _model.ClearContentInfo = title;
        _viewer.ChangeClearContentInfo(_model.ClearContentInfo);
    }

    public void ActivateNextStageBtn(bool active)
    {
        _model.ActiveNextStageBtn = active;
        _viewer.ActivateNextStageBtn(_model.ActiveNextStageBtn);
    }

    public void ActivateClearExitBtn(bool active)
    {
        _model.ActiveClearExitBtn = active;
        _viewer.ActivateClearExitBtn(_model.ActiveClearExitBtn);
    }




    public void ActivateDetailContent(bool active)
    {
        _model.ActiveDetailContent = active;
        _viewer.ActivateDetailContent(_model.ActiveDetailContent);
    }

    public void ChangeCurrentHintUsage(int usage, string format)
    {
        _model.CurrentHintUsage = string.Format(format, usage);
        _viewer.ChangeCurrentHintUsage(_model.CurrentHintUsage);
    }

    public void ChangeCurrentWrongCount(int wrongCount, string format)
    {
        _model.CurrentWrongCount = string.Format(format, wrongCount);
        _viewer.ChangeCurrentWrongCount(_model.CurrentWrongCount);
    }


    public void ChangeArtworkPreview(Sprite artSprite, Sprite artFrameSprite, Sprite rankDecorationIcon)
    {
        _model.PreviewArtSprite = artSprite;
        _model.PreviewArtFrameSprite = artFrameSprite;
        _model.PreviewRankDecorationIconSprite = rankDecorationIcon;
        _viewer.ChangeArtworkPreview(_model.PreviewArtSprite, _model.PreviewArtFrameSprite, _model.PreviewRankDecorationIconSprite);
    }


    public void ChangeArtwork(Sprite artSprite, Sprite artFrameSprite, Sprite rankDecorationIcon, bool hasIt)
    {
        _model.ArtSprite = artSprite;
        _model.ArtFrameSprite = artFrameSprite;
        _model.RankDecorationIconSprite = rankDecorationIcon;
        _model.HasIt = hasIt;
        _viewer.ChangeArtwork(_model.ArtSprite, _model.ArtFrameSprite, _model.RankDecorationIconSprite, _model.HasIt);
    }

    public void ChangeRank(Sprite rankIcon, bool activeIcon, string rankName, UnityEngine.Color rankBackgroundColor)
    {
        _model.RankIcon = rankIcon;
        _model.ActiveIcon = activeIcon;
        _model.RankName = rankName;
        _model.RankBackgroundColor = rankBackgroundColor;
        _viewer.ChangeRank(_model.RankIcon, _model.ActiveIcon, _model.RankName, _model.RankBackgroundColor);
    }

    public void ChangeGetRank(int hintUseCount, int wrongCount, string usageFormat, string wrongFormat)
    {
        _model.TotalHintUseCount = string.Format(usageFormat, hintUseCount);
        _model.TotalWrongCount = string.Format(wrongFormat, wrongCount);
        _viewer.ChangeGetRank(_model.TotalHintUseCount, _model.TotalWrongCount);
    }

    public void ChangeCollectionRatio(float currentCollectRatio, float totalCollectRatio)
    {
        _model.CurrentCollectRatio = currentCollectRatio;
        _model.TotalCollectRatio = totalCollectRatio;
        _model.CurrentCollectRatioString = $"{Mathf.RoundToInt(currentCollectRatio * 100)}%";
        _model.TotalCollectRatioString = $"{Mathf.RoundToInt(totalCollectRatio * 100)}%";

        _viewer.ChangeCollectionRatio(
            _model.CurrentCollectRatio, 
            _model.TotalCollectRatio,
            _model.CurrentCollectRatioString,
            _model.TotalCollectRatioString);
    }

    public void ActivatePausePanel(bool active)
    {
        if (active)
        {
            ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.BtnClick);
            ServiceLocater.ReturnTimeController().Stop();

            // 데이터 불러와서 반영
            SaveData data = ServiceLocater.ReturnSaveManager().GetSaveData();

            ChangeBGMModel(data.BgmVolume);
            _viewer.ChangeBGMSliderValue(data.BgmVolume, _model.BgmleftTextInfo, _model.ColorOnBgmHandle);

            ChangeSFXModel(data.SfxVolume);
            _viewer.ChangeSFXSliderValue(data.SfxVolume, _model.SfxleftTextInfo, _model.ColorOnSfxHandle);
        }
        else
        {
            ServiceLocater.ReturnTimeController().Start();
            ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.BtnClick);
        }

        _model.ActivePausePanel = active;
        _viewer.ActivatePausePanel(_model.ActivePausePanel);
    }

    public void SaveBGMValue()
    {
        ServiceLocater.ReturnSaveManager().ChangeBGMVolume(_model.BgmRatio);
    }

    public void SaveSFXValue()
    {
        ServiceLocater.ReturnSaveManager().ChangeSFXVolume(_model.SfxRatio);
    }

    readonly UnityEngine.Color _colorOnZeroValue = new UnityEngine.Color(118f / 255f, 113f / 255f, 111f / 255f);
    readonly UnityEngine.Color _colorOnBgmHandle = new UnityEngine.Color(113f / 255f, 191f / 255f, 255f / 255f);
    readonly UnityEngine.Color _colorOnSfxHandle = new UnityEngine.Color(255f / 255f, 154f / 255f, 145f / 255f);

    void ChangeBGMModel(float volumn)
    {
        if (volumn == 0)
        {
            _model.BgmleftTextInfo = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.Mute);
            _model.ColorOnBgmHandle = _colorOnZeroValue;
        }
        else
        {
            _model.BgmleftTextInfo = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.DecreaseSound);
            _model.ColorOnBgmHandle = _colorOnBgmHandle;
        }
    }

    void ChangeSFXModel(float volumn)
    {
        if (volumn == 0)
        {
            _model.SfxleftTextInfo = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.Mute);
            _model.ColorOnSfxHandle = _colorOnZeroValue;
        }
        else
        {
            _model.SfxleftTextInfo = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.DecreaseSound);
            _model.ColorOnSfxHandle = _colorOnSfxHandle;
        }
    }

    public void OnBGMSliderValeChanged(float ratio)
    {
        _model.BgmRatio = ratio;

        ChangeBGMModel(_model.BgmRatio);
        _viewer.ChangeBGMSliderHandleColor(_model.BgmleftTextInfo, _model.ColorOnBgmHandle);
        ServiceLocater.ReturnSoundPlayer().SetBGMVolume(_model.BgmRatio);
    }

    public void OnSFXSliderValeChanged(float ratio)
    {
        _model.SfxRatio = ratio;

        ChangeSFXModel(_model.SfxRatio);
        _viewer.ChangeSFXSliderHandleColor(_model.SfxleftTextInfo, _model.ColorOnSfxHandle);
        ServiceLocater.ReturnSoundPlayer().SetSFXVolume(_model.SfxRatio);
    }

    public void ActivatePlayPanel(bool active)
    {
        _model.ActivePlayPanel = active;
        _viewer.ActivatePlayPanel(_model.ActivePlayPanel);
    }

    public void ChangeHintInfoText(string infoText)
    {
        _model.HintInfo = infoText;
        _viewer.ChangeHintInfoText(_model.HintInfo);
    }

    public void ChangeProgressText(int progress)
    {
        _model.Progress = $"{progress}%";
        _viewer.ChangeProgressText(_model.Progress);
    }

    const int maxTitleSize = 13;
    const int dotSize = 3;

    public void ChangeTitle(string title, int currentSection, int totalSectionSize)
    {
        if (title.Length > maxTitleSize) title = title.Substring(0, maxTitleSize - dotSize) + "...";

        _model.Title = title + " (" + currentSection + "/" + totalSectionSize + ")";
        _viewer.ChangeTitle(_model.Title);
    }

    public void ChangeTotalTime(float totalTime)
    {
        int intPart = (int)totalTime;      // 정수 부분
        float decimalPart = totalTime % 1; // 소수점 이하

        // 정수 부분이 1자리면 D2로 맞추고, 그렇지 않으면 그대로 출력
        string formattedIntPart = intPart < 10 ? $"{intPart:D2}" : $"{intPart}";

        // 소수점 이하 두 자리 유지
        _model.TotalTime = $"{formattedIntPart}.{(decimalPart * 100):00}";
        _viewer.ChangeTotalTime(_model.TotalTime);
    }

    public void ChangeLeftTime(float leftTime, float ratio)
    {
        int intPart = (int)leftTime;
        float decimalPart = leftTime % 1;

        string formattedIntPart = intPart < 10 ? $"{intPart:D2}" : $"{intPart}";

        _model.LeftTime = $"{formattedIntPart}.{(decimalPart * 100):00}";
        _model.TimeRatio = ratio;
        _viewer.ChangeLeftTime(_model.LeftTime, _model.TimeRatio);
    }

    public void ActivateTimerContent(bool active)
    {
        _model.ActiveTimerContent = active;
        _viewer.ActivateTimerContent(_model.ActiveTimerContent);
    }

    public void ActivateRememberPanel(bool active)
    {
        _model.ActiveRememberPanel = active;
        _viewer.ActivateRememberPanel(_model.ActiveRememberPanel);
    }

   

    public void ActivateGameResultPanel(bool active)
    {
        _model.ActiveGameResultPanel = active;
        _viewer.ActivateGameResultPanel(_model.ActiveGameResultPanel);
    }
}
