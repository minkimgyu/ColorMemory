using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ChallengeStageUIModel
{
    GameObject _hintPanel;
    GameObject _endPanel;

    Image _timerSlider;
    TMP_Text _timeText;
    TMP_Text _titleText;

    public ChallengeStageUIModel(
        Image timerSlider,
        TMP_Text timeText,
        TMP_Text titleText,
        GameObject hintPanel,
        GameObject endPanel)
    {
        _timerSlider = timerSlider;
        _timeText = timeText;
        _titleText = titleText;

        _hintPanel = hintPanel;
        _endPanel = endPanel;
    }

    string _title;
    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            _titleText.text = _title;
        }
    }

    float _leftTime;
    public float LeftTime
    {
        get => _leftTime;
        set
        {
            _leftTime = value;
            _timeText.text = string.Format("{0:F2}", _leftTime);
        }
    }

    float _timeRatio;
    public float TimeRatio 
    { 
        get => _timeRatio; 
        set
        {
            _timeRatio = value;
            _timerSlider.fillAmount = value;
        }
    }

    bool _activeHintPanel;
    public bool ActiveHintPanel
    {
        get => _activeHintPanel;
        set
        {
            _activeHintPanel = value;
            _hintPanel.SetActive(value);
        }
    }

    bool _activeEndPanel;
    public bool ActiveEndPanel
    {
        get => _activeEndPanel;
        set
        {
            _activeHintPanel = value;
            _endPanel.SetActive(value);
        }
    }
}

public class ChallengeStageUIController : MonoBehaviour
{
    ChallengeStageUIModel _model;

    public void Initialize(
        Image timerSlider,
        TMP_Text timeText,
        TMP_Text titleText,
        GameObject hintPanel,
        GameObject endPanel)
    {
        _model = new ChallengeStageUIModel(timerSlider, timeText, titleText, hintPanel, endPanel);
    }

    public void ChangeTitle(string title)
    {
        _model.Title = title;
    }

    public void FillTimeSlider(float duration)
    {
        DOVirtual.Float(_model.TimeRatio, 1, duration, ((x) => { _model.TimeRatio = x; }));
    }

    public void ChangeTime(float time, float ratio)
    {
        _model.LeftTime = time;
        _model.TimeRatio = ratio;
    }

    public void ActivateHintPanel(bool active)
    {
        _model.ActiveHintPanel = active;
    }

    public void ActivateEndPanel(bool active)
    {
        _model.ActiveEndPanel = active;
    }
}
