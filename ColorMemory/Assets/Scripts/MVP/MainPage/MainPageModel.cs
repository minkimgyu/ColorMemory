using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPageModel
{
    bool _activeContent;
    GameMode.Type _modeType;

    Dictionary<GameMode.Type, Sprite> _titleImages;
    Dictionary<GameMode.Type, Color[,]> _dotColors;
    Dictionary<GameMode.Type, Color> _playBtnColors;

    public MainPageModel(
        GameMode.Type modeType,

        Dictionary<GameMode.Type, Sprite> titleImages,
        Dictionary<GameMode.Type, Color[,]> dotColors,
        Dictionary<GameMode.Type, Color> playBtnColors)
    {
        _activeContent = false;
        _modeType = modeType;

        _titleImages = titleImages;
        _dotColors = dotColors;
        _playBtnColors = playBtnColors;
    }

    public GameMode.Type ModeType { get => _modeType; set => _modeType = value; }

    public Dictionary<GameMode.Type, Sprite> TitleImages { get => _titleImages; }
    public Dictionary<GameMode.Type, Color[,]> DotColors { get => _dotColors; }
    public Dictionary<GameMode.Type, Color> PlayBtnColors { get => _playBtnColors; }
    public bool ActiveContent { get => _activeContent; set => _activeContent = value; }
}
