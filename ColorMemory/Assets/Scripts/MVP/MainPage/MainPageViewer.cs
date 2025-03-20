using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPageViewer
{
    GameObject _content;
    Image _titleImg;

    Button _playBtn;
    Image _playBtnImg;
    ToggleBtn _toggleBtn;
    Dot[,] _dots;

    MainPagePresenter _mainPagePresenter;

    public MainPageViewer(
        GameObject content,
        Image titleImg,
        Button playBtn,
        Image playBtnImg,
        ToggleBtn toggleBtn,
        Dot[,] dots,
        MainPagePresenter homePagePresenter)
    {
        _content = content;
        _titleImg = titleImg;
        _playBtn = playBtn;
        _playBtn.onClick.AddListener(() => { homePagePresenter.OnClickPlayBtn(); });


        _playBtnImg = playBtnImg;
        _dots = dots;
        _mainPagePresenter = homePagePresenter;

        _toggleBtn = toggleBtn;
        _toggleBtn.OnClick += _mainPagePresenter.OnModeTypeChanged;

        ActiveContent(false);
    }

    public void ChangeToggleState(bool isOn)
    {
        _toggleBtn.ChangeState(isOn);
    }

    public void ActiveContent(bool active)
    {
        _content.SetActive(active);
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
