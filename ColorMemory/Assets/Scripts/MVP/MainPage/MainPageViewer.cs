using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPageViewer
{
    GameObject _content;
    Image _titleImg;

    Button _playBtn;
    TMPro.TMP_Text _playBtnTxt;

    Image _playBtnImg;
    ToggleBtn _toggleBtn;
    Dot[,] _dots;

    MainPagePresenter _mainPagePresenter;

    public MainPageViewer(
        GameObject content,
        Image titleImg,
        Button playBtn,
        TMPro.TMP_Text playBtnTxt,

        Image playBtnImg,
        ToggleBtn toggleBtn,
        Dot[,] dots,
        MainPagePresenter homePagePresenter)
    {
        _content = content;
        _titleImg = titleImg;
        _playBtn = playBtn;
        _playBtn.onClick.AddListener(() => { homePagePresenter.OnClickPlayBtn(); });

        _playBtnTxt = playBtnTxt;

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

        ToWhite();
        DOVirtual.DelayedCall(0.5f, () =>
        {
            ToColor(colors);
        });
    }

    void ToColor(Color[,] colors)
    {
        int gridSize = 6;
        float delay = 0;

        for (int d = 0; d < gridSize * 2 - 1; d++)
        {
            delay += 0.1f;
            int startX = Mathf.Max(0, d - (gridSize - 1));
            int startY = Mathf.Min(d, gridSize - 1);

            while (startX < gridSize && startY >= 0)
            {
                DelayCall(delay, startX, startY, colors[startX, startY]);
                startX++;
                startY--;
            }
        }
    }

    void ToWhite()
    {
        int gridSize = 6;
        float delay = 0;

        for (int d = 0; d < gridSize * 2 - 1; d++)
        {
            delay += 0.1f;
            int startX = Mathf.Max(0, d - (gridSize - 1));
            int startY = Mathf.Min(d, gridSize - 1);

            while (startX < gridSize && startY >= 0)
            {
                DelayCall(delay, startX, startY, Color.white);
                startX++;
                startY--;
            }
        }
    }

    void DelayCall(float delay, int i, int j, Color color)
    {
        DOVirtual.DelayedCall(delay, () =>
        {
            _dots[i, j].Expand(color, 0.5f);
        });
    }

    public void ChangePlayBtnTxt(string btnTxt)
    {
        _playBtnTxt.text = btnTxt;
    }
}
