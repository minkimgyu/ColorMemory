using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingPageViewer
{
    SideSheetUI _sideSheetUI;
    Toggle[] _toggles;
    GameObject[] _panels;

    Image _miniprofileImg;
    TMP_Text _nameText;

    Image _profileImg;
    Toggle[] _profileSelectBtns;
    Button _doneBtn;

    CustomSlider _bgmSlider;
    Image _bgmSliderHandle;

    CustomSlider _sfxSlider;
    Image _sfxSliderHandle;

    public SettingPageViewer(
        SideSheetUI sideSheetUI,
        Toggle[] toggles,
        GameObject[] panels,

        Image miniprofileImg,
        TMP_Text nameText,

        Image profileImg,
        Toggle[] profileSelectBtns,
        Button doneBtn,
        CustomSlider bgmSlider,
        CustomSlider sfxSlider,
        SettingPagePresenter presenter)
    {
        _sideSheetUI = sideSheetUI;
        _toggles = toggles;
        _panels = panels;

        _miniprofileImg = miniprofileImg;
        _nameText = nameText;

        _profileImg = profileImg;

        _profileSelectBtns = profileSelectBtns;
        _doneBtn = doneBtn;
        _bgmSlider = bgmSlider;
        _bgmSliderHandle = bgmSlider.handleRect.GetComponent<Image>();

        _sfxSlider = sfxSlider;
        _sfxSliderHandle = sfxSlider.handleRect.GetComponent<Image>();

        _doneBtn.onClick.AddListener(() => { presenter.OnProfileDone(); });

        for (int i = 0; i < _profileSelectBtns.Length; i++)
        {
            int index = i + 1; // 로컬 변수로 복사하여 클로저 문제 해결
            _profileSelectBtns[i].onValueChanged.AddListener((on) => { presenter.OnProfileSelected(on, index); });
        }

        _sideSheetUI.OnPanelActivated += presenter.OnPanelActivated;

        for (int i = 0; i < _toggles.Length; i++)
        {
            int index = i; // 로컬 변수로 복사하여 클로저 문제 해결
            _toggles[i].onValueChanged.AddListener((on) => { presenter.ActivateTogglePanel(index); });
        }

        _bgmSlider.onHandlePointerUp += ((ratio) => { presenter.SaveBGMValue(); });
        _sfxSlider.onHandlePointerUp += ((ratio) => { presenter.SaveSFXValue(); });

        _bgmSlider.onValueChanged.AddListener((ratio) => { presenter.OnBGMSliderValeChanged(ratio); });
        _sfxSlider.onValueChanged.AddListener((ratio) => { presenter.OnSFXSliderValeChanged(ratio); });
    }

    public void ChangeBGMSliderValue(float ratio, Color nomalColor, Color colorOnZeroValue)
    {
        _bgmSlider.value = ratio;
        ChangeBGMSliderHandleColor(ratio, nomalColor, colorOnZeroValue);
    }

    public void ChangeSFXSliderValue(float ratio, Color nomalColor, Color colorOnZeroValue)
    {
        _sfxSlider.value = ratio;
        ChangeSFXSliderHandleColor(ratio, nomalColor, colorOnZeroValue);
    }

    // 핸들 이벤트에 들어가는 코드 -> _sfxSlider.value를 직접 변경하면 안 됨
    public void ChangeBGMSliderHandleColor(float ratio, Color nomalColor, Color colorOnZeroValue)
    {
        if (ratio == 0) _bgmSliderHandle.color = colorOnZeroValue;
        else _bgmSliderHandle.color = nomalColor;
    }

    public void ChangeSFXSliderHandleColor(float ratio, Color nomalColor, Color colorOnZeroValue)
    {
        if (ratio == 0) _sfxSliderHandle.color = colorOnZeroValue;
        else _sfxSliderHandle.color = nomalColor;
    }


    public void ChangeProfileToggle(int index)
    {
        _profileSelectBtns[index].isOn = true;
    }

    public void ChangeName(string name)
    {
        _nameText.text = name;
    }

    public void ChangeProfileImg(Sprite profile)
    {
        _miniprofileImg.sprite = profile;
        _profileImg.sprite = profile;
    }

    public void ActivateTogglePanel(int index)
    {
        for (int i = 0; i < _panels.Length; i++)
        {
            if (i == index) _panels[i].SetActive(true);
            else _panels[i].SetActive(false);
        }
    }
}
