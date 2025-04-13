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
    ChallengeStageUIViewer _viewer;

    public Action GoToEndState { get; private set; }

    public ChallengeStageUIPresenter(ChallengeStageUIModel model, Action GoToEndState)
    {
        _model = model;
        this.GoToEndState = GoToEndState;
    }

    public void InjectViewer(ChallengeStageUIViewer viewer)
    {
        _viewer = viewer;
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
        DOVirtual.Float(_model.TimeRatio, 1, duration, 
            ((ratio) => 
            { 
                _model.TimeRatio = ratio; 
                _viewer.FillTimeSlider(_model.TimeRatio);
            }
        ));
    }

    public void ActivateHint(bool oneColorHintActive, bool oneZoneHintActive)
    {
        _model.OneColorHintActive = oneColorHintActive;
        _model.OneZoneHintActive = oneZoneHintActive;
        _viewer.ActivateHint(_model.OneColorHintActive, _model.OneZoneHintActive);
    }

    public void OnClickGameExitBtn()
    {
        ServiceLocater.ReturnTimeController().Start();

        // �Ͻ� ���� ���ֱ�
        ActivatePausePanel(false); 
    }

    public void ActivatePausePanel(bool active)
    {
        if (active)
        {
            ServiceLocater.ReturnTimeController().Stop();

            // ������ �ҷ��ͼ� �ݿ�
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


    readonly Color _colorOnZeroValue = new Color(118f / 255f, 113f / 255f, 111f / 255f);
    readonly Color _colorOnBgmHandle = new Color(113f / 255f, 191f / 255f, 255f / 255f);
    readonly Color _colorOnSfxHandle = new Color(255f / 255f, 154f / 255f, 145f / 255f);

    void ChangeBGMModel(float volumn)
    {
        if (volumn == 0)
        {
            _model.BgmleftTextInfo = "���Ұ�";
            _model.ColorOnBgmHandle = _colorOnZeroValue;
        }
        else
        {
            _model.BgmleftTextInfo = "�۰�";
            _model.ColorOnBgmHandle = _colorOnBgmHandle;
        }
    }

    void ChangeSFXModel(float volumn)
    {
        if (volumn == 0)
        {
            _model.SfxleftTextInfo = "���Ұ�";
            _model.ColorOnSfxHandle = _colorOnZeroValue;
        }
        else
        {
            _model.SfxleftTextInfo = "�۰�";
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

    public void ChangeStageOverInfo()
    {
        _viewer.ChangeStageOverInfo(_model.StageCount);
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
        _model.TotalTime = totalTime;
        _viewer.ChangeTotalTime(_model.TotalTime);
    }

    public void ChangeLeftTime(float leftTime, float ratio)
    {
        _model.LeftTime = leftTime;
        _model.TimeRatio = ratio;

        _viewer.ChangeLeftTime(_model.LeftTime, _model.TimeRatio);
    }

    public void ChangeStageCount(int stageCount)
    {
        _model.StageCount = stageCount;
        _viewer.ChangeStageCount(_model.StageCount);
    }

    public void ActivateRememberPanel(bool active)
    {
        _model.ActiveRememberPanel = active;
        _viewer.ActivateRememberPanel(_model.ActiveRememberPanel);
    }

    public void ActivateHintPanel(bool active)
    {
        _model.ActiveHintPanel = active;
        _viewer.ActivateHintPanel(_model.ActiveHintPanel);
    }

    public void ActivateGameOverPanel(bool active)
    {
        _model.ActiveGameOverPanel = active;
        _viewer.ActivateGameOverPanel(_model.ActiveGameOverPanel);
    }

    public void ChangeClearStageCount(int clearStageCount, int resultScore)
    {
        _model.ClearStageCount = clearStageCount;
        _model.ResultScore = resultScore;
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

    public void ChangeGoldCount(int goldCount)
    {
        _model.GoldCount = goldCount;
        _viewer.ChangeGoldCount(_model.GoldCount);
    }

    public void AddRanking(SpawnableUI ranking, bool setToMiddle = false)
    {
        _viewer.AddRanking(ranking, setToMiddle);
    }

    public void RemoveAllRanking()
    {
        _viewer.RemoveAllRanking();
    }

    public void SetUpRankingScroll(int menuCount, int index)
    {
        _model.MenuCount = menuCount;
        _model.ScrollIndex = index;
        _viewer.ChangeRankingScrollValue(_model.MenuCount, _model.ScrollIndex);
    }
}
