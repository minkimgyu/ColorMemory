using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShareArtworkUI : MonoBehaviour
{
    [SerializeField] Image _artworkImg;
    [SerializeField] TMP_Text _artworkTitle;
    [SerializeField] TMP_Text _artworkDescription;

    const int maxLength = 25;

    public void Initialize(Sprite artSprite, string artworkTitle, string artworkDescription)
    {
        _artworkImg.sprite = artSprite;

        if(artworkTitle.Length > maxLength) _artworkTitle.text = $"{artworkTitle.Substring(0, maxLength)}...";
        else _artworkTitle.text = $"{artworkTitle.Substring(0, artworkTitle.Length)}";

        if (artworkDescription.Length > maxLength) _artworkDescription.text = $"{artworkDescription.Substring(0, maxLength)}...";
        else _artworkDescription.text = $"{artworkDescription.Substring(0, artworkDescription.Length)}";
    }
}
