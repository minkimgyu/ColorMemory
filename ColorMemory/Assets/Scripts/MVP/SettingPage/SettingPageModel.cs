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

    public bool ActiveContent { get => _activeContent; set => _activeContent = value; }

    public int CurrentToggleIndex { get => _toggleIndex; set => _toggleIndex = value; }
    public int ProfileIndex { get => _profileIndex; set => _profileIndex = value; }
    public int SelectedProfileIndex { get => _selectedProfileIndex; set => _selectedProfileIndex = value; }

    public float BgmRatio { get => _bgmRatio; set => _bgmRatio = value; }
    public float SfxRatio { get => _sfxRatio; set => _sfxRatio = value; }
    public Dictionary<int, Sprite> ProfileSprites { get => _profileSprites; set => _profileSprites = value; }
    public string Name { get => _name; set => _name = value; }

    Color _colorOnBgmHandle;
    Color _colorOnSfxHandle;

    public Color ColorOnBgmHandle { get => _colorOnBgmHandle; set => _colorOnBgmHandle = value; }
    public Color ColorOnSfxHandle { get => _colorOnSfxHandle; set => _colorOnSfxHandle = value; }

    string _bgmleftTextInfo;
    public string BgmleftTextInfo { get => _bgmleftTextInfo; set => _bgmleftTextInfo = value; }

    string _sfxleftTextInfo;
    public string SfxleftTextInfo { get => _sfxleftTextInfo; set => _sfxleftTextInfo = value; }

    public SettingPageModel(Dictionary<int, Sprite> profileSprites)
    {
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
