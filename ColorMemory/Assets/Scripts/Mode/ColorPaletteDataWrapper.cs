using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Colors
{
    [Newtonsoft.Json.JsonProperty("R")] int _r;
    [Newtonsoft.Json.JsonProperty("G")] int _g;
    [Newtonsoft.Json.JsonProperty("B")] int _b;

    [Newtonsoft.Json.JsonIgnore] public float Red { get => (float)_r / 255f; }
    [Newtonsoft.Json.JsonIgnore] public float Green { get => (float)_g / 255f; }
    [Newtonsoft.Json.JsonIgnore] public float Blue { get => (float)_b / 255f; }
}

[Serializable]
public struct ColorPaletteData
{
    [Newtonsoft.Json.JsonProperty("colors")] Colors[] _colors;

    public ColorPaletteData(Colors[] colors)
    {
        _colors = colors;
    }

    [Newtonsoft.Json.JsonIgnore]
    public Color[] Colors
    { 
        get 
        {
            Color[] colorPalette = new Color[_colors.Length];
            for (int i = 0; i < _colors.Length; i++)
            {
                colorPalette[i] = new Color(_colors[i].Red, _colors[i].Green, _colors[i].Blue);
            }

            return colorPalette; 
        } 
    }
}

[Serializable]
public struct ColorPaletteDataWrapper
{
    [Newtonsoft.Json.JsonProperty("ColorPaletteDatas")] List<ColorPaletteData> _colorPaletteDatas;

    public ColorPaletteDataWrapper(List<ColorPaletteData> colorPaletteDatas)
    {
        _colorPaletteDatas = colorPaletteDatas;
    }

    [Newtonsoft.Json.JsonIgnore] public ColorPaletteData RandomColorPaletteData 
    {
        get
        {
            return _colorPaletteDatas[UnityEngine.Random.Range(0, _colorPaletteDatas.Count)];
        }
    }
}
