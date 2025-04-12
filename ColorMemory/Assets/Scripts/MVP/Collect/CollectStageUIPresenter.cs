using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using Unity.VisualScripting;

public class CollectStageUIPresenter
{
    CollectStageUIModel _model;
    CollectStageUIViewer _viewer;

    public Action GoToResultState { get; private set; }

    public CollectStageUIPresenter(CollectStageUIModel model, Action GoToResultState)
    {
        _model = model;
        this.GoToResultState = GoToResultState;
    }

    public void InjectViewer(CollectStageUIViewer viewer)
    {
        _viewer = viewer;
    }

    public void ChangeGameResultTitle(bool hasIt)
    {
        if (hasIt)
        {
            _model.GameResultTitle = "축하해요! 새로운 명화를 획득했어요!";
        }
        else
        {
            _model.GameResultTitle = "아직 명화를 획득하지 못했어요";
        }

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

    public void ChangeCurrentHintUsage(int usage)
    {
        _model.CurrentHintUsage = usage;
        _viewer.ChangeCurrentHintUsage(_model.CurrentHintUsage);
    }

    public void ChangeCurrentWrongCount(int wrongCount)
    {
        _model.CurrentWrongCount = wrongCount;
        _viewer.ChangeCurrentWrongCount(_model.CurrentWrongCount);
    }





    public void ChangeArtwork(Sprite artSprite, Sprite artFrameSprite, Sprite rankDecorationIcon, bool hasIt)
    {
        _model.ArtSprite = artSprite;
        _model.ArtFrameSprite = artFrameSprite;
        _model.RankDecorationIconSprite = rankDecorationIcon;
        _model.HasIt = hasIt;
        _viewer.ChangeArtwork(_model.ArtSprite, _model.ArtFrameSprite, _model.RankDecorationIconSprite, _model.HasIt);
    }

    public void ChangeRank(Sprite rankIcon, bool activeIcon, string rankName, Color rankBackgroundColor)
    {
        _model.RankIcon = rankIcon;
        _model.ActiveIcon = activeIcon;
        _model.RankName = rankName;
        _model.RankBackgroundColor = rankBackgroundColor;
        _viewer.ChangeRank(_model.RankIcon, _model.ActiveIcon, _model.RankName, _model.RankBackgroundColor);
    }

    public void ChangeGetRank(int hintUseCount, int wrongCount)
    {
        _model.HintUseCount = hintUseCount;
        _model.WrongCount = wrongCount;

        _viewer.ChangeGetRank(_model.HintUseCount, _model.WrongCount);
    }

    public void ChangeCollectionRatio(float currentCollectRatio, float totalCollectRatio)
    {
        _model.CurrentCollectRatio = currentCollectRatio;
        _model.TotalCollectRatio = totalCollectRatio;
        _viewer.ChangeCollectionRatio(_model.CurrentCollectRatio, _model.TotalCollectRatio);
    }

    public void OnClickGameExitBtn()
    {
        // 데이터 저장
        ServiceLocater.ReturnSaveManager().ChangeBGMVolume(_model.BgmRatio);
        ServiceLocater.ReturnSaveManager().ChangeSFXVolume(_model.SfxRatio);
        ActivatePausePanel(false);
        //ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.HomeScene);
    }

    readonly Color _colorOnZeroValue = new Color(118f / 255f, 113f / 255f, 111f / 255f);
    readonly Color _colorOnBgmHandle = new Color(113f / 255f, 191f / 255f, 255f / 255f);
    readonly Color _colorOnSfxHandle = new Color(255f / 255f, 154f / 255f, 145f / 255f);

    void ChangeBGMModel(float volumn)
    {
        if (volumn == 0)
        {
            _model.BgmleftTextInfo = "음소거";
            _model.ColorOnBgmHandle = _colorOnZeroValue;
        }
        else
        {
            _model.BgmleftTextInfo = "작게";
            _model.ColorOnBgmHandle = _colorOnBgmHandle;
        }
    }

    void ChangeSFXModel(float volumn)
    {
        if (volumn == 0)
        {
            _model.SfxleftTextInfo = "음소거";
            _model.ColorOnSfxHandle = _colorOnZeroValue;
        }
        else
        {
            _model.SfxleftTextInfo = "작게";
            _model.ColorOnSfxHandle = _colorOnSfxHandle;
        }
    }

    public void ActivatePausePanel(bool active)
    {
        if (active)
        {
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
        _model.Progress = progress;
        _viewer.ChangeProgressText(_model.Progress);
    }

    public void ChangeTitle(string title)
    {
        _model.Title = title;
        _viewer.ChangeTitle(_model.Title);
    }

    public void FillTimeSlider(float duration)
    {
        DOVirtual.Float(_model.TimeRatio, 1, duration, 
            ((ratio) => 
            { 
                _model.TimeRatio = ratio; 
                _viewer.FillTimeSlider(_model.TimeRatio);
            }
        ));
    }

    public void ChangeTotalTime(float totalTime)
    {
        _model.TotalTime = totalTime;
        _viewer.ChangeTotalTime(_model.TotalTime);
    }

    public void ChangeLeftTime(float leftTime, float ratio)
    {
        _model.LeftTime = leftTime;
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
