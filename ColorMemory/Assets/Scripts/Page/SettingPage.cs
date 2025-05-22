using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingPage : MonoBehaviour
{
    const int _contentCount = 4;
    [SerializeField] Toggle[] _toggles = new Toggle[_contentCount];
    [SerializeField] GameObject[] _panels = new GameObject[_contentCount];
    [SerializeField] SideSheetUI _sideSheetUI;

    [SerializeField] Image _miniProfileImg;
    [SerializeField] TMP_Text _nameText;

    [SerializeField] Button _homeBtn;

    [SerializeField] Image _profileImg;
    [SerializeField] Toggle[] _profileSelectBtns;
    [SerializeField] Button _doneBtn;

    [SerializeField] TMP_Text _bgmTitleText;
    [SerializeField] CustomSlider _bgmSlider;
    [SerializeField] TMP_Text _bgmLeftText;
    [SerializeField] TMP_Text _bgmRightText;

    [SerializeField] TMP_Text _sfxTitleText;
    [SerializeField] CustomSlider _sfxSlider;
    [SerializeField] TMP_Text _sfxLeftText;
    [SerializeField] TMP_Text _sfxRightText;

    [SerializeField] TMP_Text _languageTitle;

    [SerializeField] TMP_Dropdown _languageDropdown;

    public void TogglePanel()
    {
        _sideSheetUI.TogglePanel();
    }

    public void Initialize(string myName, Dictionary<int, Sprite> profileSprites, System.Action OnClickHomeBtn, System.Action OnChangeLanguage)
    {
        _sideSheetUI.Initialize();
        SettingPageModel model = new SettingPageModel(profileSprites);
        SettingPagePresenter presenter = new SettingPagePresenter(model, new ProfileService());
        SettingPageViewer viewer = new SettingPageViewer(
            _sideSheetUI,
            _toggles,
            _panels,
            _miniProfileImg,
            _nameText,
            _profileImg,
            _profileSelectBtns,
            _doneBtn,

            _bgmSlider,
            _bgmTitleText,
            _bgmLeftText,
            _bgmRightText,

            _sfxSlider,
            _sfxTitleText,
            _sfxLeftText,
            _sfxRightText,
            _languageTitle,
            _homeBtn,
            _languageDropdown,
            presenter);
        presenter.InjectViewer(viewer);

        presenter.OnHomeBtnClicked += () => 
        {
            ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.BtnClick);
            TogglePanel(); 
            OnClickHomeBtn?.Invoke(); 
        };

        presenter.ChangeLanguageDropdown((int)ServiceLocater.ReturnSaveManager().GetSaveData().Language);
        presenter.OnChangeLanguage += (index) =>
        {
            ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.BtnClick);

            ServiceLocater.ReturnSaveManager().ChangeLanguage((ILocalization.Language)index);
            string languageTitle = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.LanguageTitle);
            presenter.ChangeLanguage();

            OnChangeLanguage?.Invoke();
        };

        presenter.ChangeProfileImgFromServer();
        presenter.ChangeName(myName);

        // ÃÊ±âÈ­
        //_languageDropdown.value = ;
        //_languageDropdown.onValueChanged.AddListener((index) =>
        //{
        //    ServiceLocater.ReturnSaveManager().ChangeLanguage((ILocalization.Language)index);
        //    string languageTitle = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.LanguageTitle);
        //    presenter.ChangeLanguage();

        //    OnChangeLanguage?.Invoke();
        //});
    }
}
