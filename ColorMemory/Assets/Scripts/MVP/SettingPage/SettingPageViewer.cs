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

    Slider _bgmSlider;
    Slider _sfxSlider;

    public SettingPageViewer(
        SideSheetUI sideSheetUI,
        Toggle[] toggles,
        GameObject[] panels,

        Image miniprofileImg,
        TMP_Text nameText,

        Image profileImg,
        Toggle[] profileSelectBtns,
        Button doneBtn,
        Slider bgmSlider,
        Slider sfxSlider,
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
        _sfxSlider = sfxSlider;

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

        _bgmSlider.onValueChanged.AddListener((ratio) => { presenter.OnBGMSliderValeChanged(ratio); });
        _sfxSlider.onValueChanged.AddListener((ratio) => { presenter.OnSFXSliderValeChanged(ratio); });
    }

    public void ChangeBGMSliderValue(float ratio)
    {
        _bgmSlider.value = ratio;
    }

    public void ChangeSFXSliderValue(float ratio)
    {
        _sfxSlider.value = ratio;
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
