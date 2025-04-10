using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPageModel
{
    const int _contentCount = 4;
    public int ContentCount => _contentCount;

    Dictionary<int, Sprite> _profileSprites;

    int _profileIndex;
    int _selectedProfileIndex;

    bool _activeContent;
    int _toggleIndex;

    float _bgmRatio;
    float _sfxRatio;

    string _name;

    Color _colorOnZeroValue;

    Color _colorOnBgmHandle;
    Color _colorOnSfxHandle;

    public bool ActiveContent { get => _activeContent; set => _activeContent = value; }

    public int CurrentToggleIndex { get => _toggleIndex; set => _toggleIndex = value; }
    public int ProfileIndex { get => _profileIndex; set => _profileIndex = value; }
    public int SelectedProfileIndex { get => _selectedProfileIndex; set => _selectedProfileIndex = value; }

    public float BgmRatio { get => _bgmRatio; set => _bgmRatio = value; }
    public float SfxRatio { get => _sfxRatio; set => _sfxRatio = value; }
    public Dictionary<int, Sprite> ProfileSprites { get => _profileSprites; set => _profileSprites = value; }
    public string Name { get => _name; set => _name = value; }
    public Color ColorOnZeroValue { get => _colorOnZeroValue; }
    public Color ColorOnBgmHandle { get => _colorOnBgmHandle; }
    public Color ColorOnSfxHandle { get => _colorOnSfxHandle; }

    public SettingPageModel(Dictionary<int, Sprite> profileSprites)
    {
        _colorOnZeroValue = new Color(118f / 255f, 113f / 255f, 111f / 255f);
        _colorOnBgmHandle = new Color(113f / 255f, 191f / 255f, 255f / 255f);
        _colorOnSfxHandle = new Color(255f / 255f, 154f / 255f, 145f / 255f);

        _name = "";
        _activeContent = false;
        _toggleIndex = 0;
        _profileIndex = 0;
        _selectedProfileIndex = 0;
        _bgmRatio = 0;
        _sfxRatio = 0;
        _profileSprites = profileSprites;
    }
}
