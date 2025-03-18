using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeStageUIViewer
{
    Image _timerSlider;

    TMP_Text _leftTimeText;
    TMP_Text _totalTimeText;


    //TMP_Text titleText;
    GameObject _hintPanel;
    GameObject _rememberPanel;
    GameObject _endPanel;

    public ChallengeStageUIViewer(
        Image timerSlider,
        TMP_Text leftTimeText,
        TMP_Text totalTimeText,


        GameObject hintPanel,
        GameObject rememberPanel,
        GameObject endPanel)
    {
        _timerSlider = timerSlider;

        _leftTimeText = leftTimeText;
        _totalTimeText = totalTimeText;

        _hintPanel = hintPanel;
        _rememberPanel = rememberPanel;
        _endPanel = endPanel;
    }

    //public void ChangeTitle(string title)
    //{
    //    _model.Title = title;
    //}

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

    public void ActivateRememberPanel(bool active)
    {
        _rememberPanel.SetActive(active);
    }

    public void ActivateHintPanel(bool active)
    {
        _hintPanel.SetActive(active);
    }

    public void ActivateEndPanel(bool active)
    {
        _endPanel.SetActive(active);
    }
}
