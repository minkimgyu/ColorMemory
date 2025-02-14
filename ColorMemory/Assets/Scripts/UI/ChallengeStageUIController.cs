using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChallengeStageUIModel
{
    Image _timerSlider;
    TMP_Text _timeText;
    TMP_Text _titleText;

    public ChallengeStageUIModel(Image timerSlider, TMP_Text timeText, TMP_Text titleText)
    {
        _timerSlider = timerSlider;
        _timeText = timeText;
        _titleText = titleText;
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
}

public class ChallengeStageUIController : MonoBehaviour
{
    ChallengeStageUIModel _model;
    [SerializeField] Image _timerSlider;
    [SerializeField] TMP_Text _timeText;
    [SerializeField] TMP_Text _titleText;

    public void Initialize()
    {
        _model = new ChallengeStageUIModel(_timerSlider, _timeText, _titleText);
    }

    public void ChangeTitle(string title)
    {
        _model.Title = title;
    }

    public void ChangeTime(float time, float ratio)
    {
        _model.LeftTime = time;
        _model.TimeRatio = ratio;
    }
}
