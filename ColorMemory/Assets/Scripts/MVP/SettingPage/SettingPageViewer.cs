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

    TMP_Text _bgmTitleText;
    CustomSlider _bgmSlider;
    Image _bgmSliderHandle;
    TMP_Text _bgmLeftText;
    TMP_Text _bgmRightText;

    TMP_Text _sfxTitleText;
    CustomSlider _sfxSlider;
    Image _sfxSliderHandle;
    TMP_Text _sfxLeftText;
    TMP_Text _sfxRightText;

    TMP_Text _languageTitle;

    Button _homeBtn;
    TMP_Dropdown _languageDropdown;

    SettingPagePresenter _presenter;

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
        TMP_Text bgmTitleText,
        TMP_Text bgmLeftText,
        TMP_Text bgmRightText,

        CustomSlider sfxSlider,
        TMP_Text sfxTitleText,
        TMP_Text sfxLeftText,
        TMP_Text sfxRightText,

        TMP_Text languageTitle,

        Button homeBtn,
        TMP_Dropdown languageDropdown,

        SettingPagePresenter presenter)
    {
        _presenter = presenter;

        _sideSheetUI = sideSheetUI;
        _toggles = toggles;
        _panels = panels;

        _miniprofileImg = miniprofileImg;
        _nameText = nameText;

        _profileImg = profileImg;

        _profileSelectBtns = profileSelectBtns;
        _doneBtn = doneBtn;

        _bgmTitleText = bgmTitleText;
        _bgmSlider = bgmSlider;
        _bgmSliderHandle = bgmSlider.handleRect.GetComponent<Image>();
        _bgmLeftText = bgmLeftText;
        _bgmRightText = bgmRightText;

        _sfxTitleText = sfxTitleText;
        _sfxSlider = sfxSlider;
        _sfxSliderHandle = sfxSlider.handleRect.GetComponent<Image>();
        _sfxLeftText = sfxLeftText;
        _sfxRightText = sfxRightText;

        _homeBtn = homeBtn;
        _homeBtn.onClick.AddListener(() => { presenter.OnHomeBtnClicked?.Invoke(); });

        _languageDropdown = languageDropdown;
        _languageDropdown.onValueChanged.AddListener((index) => { presenter.OnChangeLanguage?.Invoke(index); });

        _languageTitle = languageTitle;
        _doneBtn.onClick.AddListener(() => { presenter.OnProfileDone(); });

        for (int i = 0; i < _profileSelectBtns.Length; i++)
        {
            int index = i; // 로컬 변수로 복사하여 클로저 문제 해결
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

    public void ChangeLanguageDropdown(int index)
    {
        _languageDropdown.value = index;
    }

    public void ChangeLanguageTitle(string title)
    {
        _languageTitle.text = title;
    }

    public void ChangeSoundText(string bgmTitle, string sfxTitle, string leftText, string rightText)
    {
        _bgmTitleText.text = bgmTitle;
        _sfxTitleText.text = sfxTitle;

        _bgmLeftText.text = leftText;
        _sfxLeftText.text = leftText;

        _bgmRightText.text = rightText;
        _sfxRightText.text = rightText;
    }

    public void ChangeBGMSliderValue(float ratio, string leftSmallTxt, Color handleColor)
    {
        _bgmSlider.value = ratio;
        ChangeBGMSliderHandleColor(leftSmallTxt, handleColor);
    }

    public void ChangeSFXSliderValue(float ratio, string leftSmallTxt, Color handleColor)
    {
        _sfxSlider.value = ratio;
        ChangeSFXSliderHandleColor(leftSmallTxt, handleColor);
    }

    public void ChangeBGMSliderHandleColor(string leftSmallTxt, Color handleColor)
    {
        _bgmSliderHandle.color = handleColor;
        _bgmLeftText.text = leftSmallTxt;
    }

    public void ChangeSFXSliderHandleColor(string leftSmallTxt, Color handleColor)
    {
        _sfxSliderHandle.color = handleColor;
        _sfxLeftText.text = leftSmallTxt;
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
