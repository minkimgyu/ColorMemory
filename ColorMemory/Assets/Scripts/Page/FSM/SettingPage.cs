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

    [SerializeField] Image _profileImg;
    [SerializeField] Toggle[] _profileSelectBtns;
    [SerializeField] Button _doneBtn;
    [SerializeField] Slider _bgmSlider;
    [SerializeField] Slider _sfxSlider;

    public void TogglePanel()
    {
        _sideSheetUI.TogglePanel();
    }

    public void Initialize(Dictionary<int, Sprite> profileSprites)
    {
        _sideSheetUI.Initialize();
        SettingPageModel model = new SettingPageModel(profileSprites);
        SettingPagePresenter presenter = new SettingPagePresenter(model);

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
            _sfxSlider,
            presenter);
        presenter.InjectViewer(viewer);

        //presenter.ChangeProfileImgFromServer();
        presenter.ChangeName("meal");
    }
}
