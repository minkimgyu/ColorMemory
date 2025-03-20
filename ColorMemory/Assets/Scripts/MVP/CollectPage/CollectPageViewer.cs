using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectPageViewer
{
    GameObject _content;
    TMP_Text _titleTxt;
    TMP_Text _descriptionTxt;
    Button _selectBtn;

    ScrollUI _artworkScroll;
    CollectPagePresenter _collectPagePresenter;

    public CollectPageViewer(
        GameObject content,
        TMP_Text titleTxt,
        TMP_Text descriptionTxt,
        Button selectBtn,

        ScrollUI artworkScroll,
        CollectPagePresenter collectPagePresenter)
    {
        _content = content;
        _titleTxt = titleTxt;
        _descriptionTxt = descriptionTxt;
        _selectBtn = selectBtn;
        _artworkScroll = artworkScroll;
        _collectPagePresenter = collectPagePresenter;

        artworkScroll.OnDragEnd += collectPagePresenter.ChangeArtworkDescription;
        ActiveContent(false);
    }

    public void ChangeArtDescription(string title, string description)
    {
        _titleTxt.text = title;
        _descriptionTxt.text = description;
    }

    public void AddArtwork(ArtworkUI artwork)
    {
        _artworkScroll.AddItem(artwork.transform);
    }

    public void ActiveContent(bool active)
    {
        _content.SetActive(active);
    }
}
