using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtworkPreviewUI : MonoBehaviour
{
    [SerializeField] Image _rankIcon;

    [SerializeField] Image _artFrameImage;
    [SerializeField] RefitableImage _refitableImage;
    [SerializeField] RectTransform _artFrame;

    public void Initialize(Sprite artSprite, Sprite artFrameSprite, Sprite rankIconSprite)
    {
        Vector2 changedSize = _refitableImage.ResizeImageWithWidth(artSprite);

        float halfSize = changedSize.y / 2;
        float yPos = halfSize - 212.76f;

        _artFrame.anchoredPosition = new Vector2(0, _artFrame.anchoredPosition.y + yPos);

        _artFrame.sizeDelta = changedSize;
        _artFrameImage.sprite = artFrameSprite;
        _rankIcon.sprite = rankIconSprite;
    }
}
