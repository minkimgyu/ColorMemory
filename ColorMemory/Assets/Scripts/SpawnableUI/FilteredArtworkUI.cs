using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FilteredArtworkUI : SpawnableUI
{
    [SerializeField] Button _selectButton;
    [SerializeField] Image _artImg;
    [SerializeField] TMP_Text _artTitle;
    [SerializeField] GameObject _lockPanel;

    [SerializeField] ResizeableImage _resizeableImage;

    const int _maxStringLength = 20;

    public override void Initialize(Sprite artSprite, string title, bool hasIt = true)
    {
        _resizeableImage.Initialize(artSprite);

        _lockPanel.SetActive(!hasIt);
        _artImg.sprite = artSprite;
        if (title.Length > _maxStringLength) _artTitle.text = $"{title.Substring(0, _maxStringLength)}...";
        else _artTitle.text = title;
    }

    System.Action OnClickRequested;

    public override void InjectClickEvent(System.Action OnClick)
    {
        OnClickRequested = OnClick;
        _selectButton.onClick.AddListener(() => { OnClickRequested?.Invoke(); });
    }
}
