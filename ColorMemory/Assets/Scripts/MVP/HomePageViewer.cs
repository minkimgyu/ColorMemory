using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomePageViewer
{
    Image _titleImg;
    Image _playBtnImg;
    ToggleBtn _toggleBtn;
    Dot[,] _dots;

    HomePagePresenter _homePagePresenter;

    public HomePageViewer(
        Image titleImg,
        Image playBtnImg,
        ToggleBtn toggleBtn,
        Dot[,] dots,
        HomePagePresenter homePagePresenter)
    {
        _titleImg = titleImg;
        _playBtnImg = playBtnImg;
        _dots = dots;
        _homePagePresenter = homePagePresenter;

        _toggleBtn = toggleBtn;
        _toggleBtn.OnClick += _homePagePresenter.OnModeTypeChanged;
    }

    public void ChangePlayBtnColor(Color playBtnColor)
    {
        _playBtnImg.color = playBtnColor;
    }

    public void ChangeTitleImage(Sprite titleSprite)
    {
        _titleImg.sprite = titleSprite;
    }

    public void ChangeDotColors(Color[,] colors)
    {
        int size = _dots.GetLength(0);

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                _dots[i, j].Fade(colors[i, j], UnityEngine.UI.Image.FillMethod.Horizontal, 0.3f);
            }
        }
    }
}
