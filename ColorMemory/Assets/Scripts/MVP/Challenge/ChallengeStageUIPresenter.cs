using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using Unity.VisualScripting;
using Challenge;

public class ChallengeStageUIPresenter
{
    ChallengeStageUIModel _model;
    IChallengeStageUIViewer _viewer;

    public Action OnClickPauseGameExitBtn { get; set; }
    public Action OnClickGoToGameOverBtn { get; set; }

    public Action OnClickGameOverExitBtn { get; set; }
    public Action OnClickNextBtn { get; set; }
    public Action OnClickSkipBtn { get; set; }


    public Action OnClickOneZoneHint { get; set; }
    public Action OnClickOneColorHint { get; set; }

    public Action OnClickRetryBtn { get; set; }
    public Action OnClickExitBtn { get; set; }


    public ChallengeStageUIPresenter(ChallengeStageUIModel model)
    {
        _model = model;

        OnClickPauseGameExitBtn += () => { ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.BtnClick); };
        OnClickGoToGameOverBtn += () => { ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.BtnClick); };

        OnClickGameOverExitBtn += () => { ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.BtnClick); };
        OnClickNextBtn += () => { ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.BtnClick); };

        OnClickOneZoneHint += () => { ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.HintClick); };
        OnClickOneColorHint += () => { ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.HintClick); };
        OnClickSkipBtn += () => { ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.HintClick); };

        OnClickRetryBtn += () => { ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.BtnClick); };
        OnClickExitBtn += () => { ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.BtnClick); };
    }

    public void InjectViewer(IChallengeStageUIViewer viewer)
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


    public void ChangeHintCost(int oneColorHintCost, int oneZoneHintCost)
    {
        _model.OneColorHintCost = $"-{oneColorHintCost.ToString("N0")}";
        _model.OneZoneHintCost = $"-{oneZoneHintCost.ToString("N0")}";
        _viewer.ChangeHintCost(_model.OneColorHintCost, _model.OneZoneHintCost);
    }

    public void ActivateHint(bool oneColorHintActive, bool oneZoneHintActive)
    {
        _model.OneColorHintActive = oneColorHintActive;
        _model.OneZoneHintActive = oneZoneHintActive;
        _viewer.ActivateHint(_model.OneColorHintActive, _model.OneZoneHintActive);
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


    readonly Color _colorOnZeroValue = new Color(118f / 255f, 113f / 255f, 111f / 255f);
    readonly Color _colorOnBgmHandle = new Color(113f / 255f, 191f / 255f, 255f / 255f);
    readonly Color _colorOnSfxHandle = new Color(255f / 255f, 154f / 255f, 145f / 255f);

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


    public void ActivateStageOverPreviewPanel(bool active)
    {
        _model.ActiveStageOverPreviewPanel = active;
        _viewer.ActivateStageOverPreviewPanel(_model.ActiveStageOverPreviewPanel);
    }

    public void ChangeLastStagePattern(MapData data, Color[] pickColors)
    {
        _model.MapData = data;
        _model.PickColors = pickColors;
        _viewer.ChangeLastStagePattern(_model.StageCount, _model.MapData, _model.PickColors);
    }

    public void ChangeStageOverInfo(string stageOverTitleText, string stageOverInfo1TextFormat, string stageOverInfo2Text)
    {
        _model.StageOverTitleText = stageOverTitleText;
        _model.StageOverInfo1Text = string.Format(stageOverInfo1TextFormat, _model.StageCount);
        _model.StageOverInfo2Text = stageOverInfo2Text;
        _viewer.ChangeStageOverInfo(_model.StageOverTitleText, _model.StageOverInfo1Text, _model.StageOverInfo2Text);
    }

    public void ActivatePlayPanel(bool active)
    {
        _model.ActivePlayPanel = active;
        _viewer.ActivatePlayPanel(_model.ActivePlayPanel);
    }

    public void ChangeBestScore(int bestScore)
    {
        _model.BestScore = bestScore;
        _viewer.ChangeBestScore(_model.BestScore);
    }

    public void ChangeNowScore(int nowScore)
    {
        _model.NowScore = nowScore;
        _viewer.ChangeNowScore(_model.NowScore);
    }

    public void ChangeTotalTime(float totalTime)
    {
        int intPart = (int)totalTime;      // 정수 부분
        float decimalPart = totalTime % 1; // 소수점 이하

        // 정수 부분이 1자리면 D2로 맞추고, 그렇지 않으면 그대로 출력
        string formattedIntPart = intPart < 10 ? $"{intPart:D2}" : $"{intPart}";
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

    public void ChangeStageCount(int stageCount)
    {
        _model.StageCount = stageCount;
        _viewer.ChangeStageCount(_model.StageCount);
    }

    public void ActivateRememberPanel(bool active, string rememberTxt)
    {
        _model.ActiveRememberPanel = active;
        _model.RememberTxt = rememberTxt;

        _viewer.ActivateRememberPanel(_model.ActiveRememberPanel, _model.RememberTxt);
    }

    public void ActivateHintPanel(bool active)
    {
        _model.ActiveHintPanel = active;
        _viewer.ActivateHintPanel(_model.ActiveHintPanel);
    }


    public void ActiveGoldPanel(bool active)
    {
        _model.ActiveCoinPanel = active;
        _viewer.ActivateCoinPanel(_model.ActiveCoinPanel);
    }

    public void ChangeCoinCount(int coinCount)
    {
        _model.CoinCount = coinCount.ToString("N0"); // "9,000"
        _viewer.ChangeCoinCount(_model.CoinCount);
    }


    public void ActivateGameOverPanel(bool active)
    {
        _model.ActiveGameOverPanel = active;
        _viewer.ActivateGameOverPanel(_model.ActiveGameOverPanel);
    }

    public void ChangeClearStageCount(int clearStageCount, int resultScore)
    {
        _model.ClearStageCount = clearStageCount.ToString("N0");
        _model.ResultScore = resultScore.ToString("N0");
        _viewer.ChangeClearStageCount(_model.ClearStageCount, _model.ResultScore);
    }

    public void AddClearPattern(SpawnableUI clearPattern)
    {
        _viewer.AddClearPattern(clearPattern);
    }

    public void RemoveClearPattern()
    {
        _viewer.RemoveClearPattern();
    }

    public void ActivateGameResultPanel(bool active)
    {
        _model.ActiveGameResultPanel = active;
        _viewer.ActivateGameResultPanel(_model.ActiveGameResultPanel);
    }

    public void ChangeResultGoldCount(int goldCount, string format)
    {
        _model.GoldCount = string.Format(format, goldCount);
        _viewer.ChangeResultGoldCount(_model.GoldCount);
    }

    public void AddRanking(SpawnableUI ranking, Vector3 size)
    {
        _viewer.AddRanking(ranking, size);
    }

    public void RemoveAllRanking()
    {
        _viewer.RemoveAllRanking();
    }

    public void SetUpRankingScroll(int menuCount, int index)
    {
        _model.MenuCount = menuCount;
        _model.ScrollIndex = index;
        //_viewer.ChangeRankingScrollValue(_model.MenuCount);
    }
}
