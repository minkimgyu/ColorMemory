using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtworkUI : MonoBehaviour
{
    public enum Type
    {
        Bronze,
        Silver,
        Gold
    }

    [SerializeField] Image _artImg;

    Image _artFrameImage;
    [SerializeField] RectTransform _artFrame;
    // 크기는 500 ~ 700 사이로 맞춰야함

    const int minSize = 500;
    const int maxSize = 700;

    public Vector2Int ResizeSprite(Sprite sprite)
    {
        // 원본 텍스처 크기 가져오기
        int originalWidth = sprite.texture.width;
        int originalHeight = sprite.texture.height;

        // 비율 계산
        float aspectRatio = (float)originalWidth / originalHeight;

        // 목표 크기 계산
        int targetWidth = originalWidth;
        int targetHeight = originalHeight;

        // 가로 또는 세로를 700으로 맞추고, 비율 유지
        if (targetWidth > targetHeight)
        {
            targetWidth = maxSize;
            targetHeight = Mathf.RoundToInt(targetWidth / aspectRatio);
        }
        else
        {
            targetHeight = maxSize;
            targetWidth = Mathf.RoundToInt(targetHeight * aspectRatio);
        }

        // 500~700 범위 내에서 크기 조정
        if (targetWidth < minSize)
        {
            targetWidth = minSize;
            targetHeight = Mathf.RoundToInt(targetWidth / aspectRatio);
        }
        if (targetHeight < minSize)
        {
            targetHeight = minSize;
            targetWidth = Mathf.RoundToInt(targetHeight * aspectRatio);
        }

        return new Vector2Int(targetWidth, targetHeight);
    }

    public void Initialize(Sprite artSprite, Sprite artFrameSprite)
    {
        Vector2Int size = ResizeSprite(artSprite);
        _artFrame.sizeDelta = size;
        _artImg.sprite = artSprite;

        _artFrameImage = _artFrame.gameObject.GetComponent<Image>();
        _artFrameImage.sprite = artFrameSprite;
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
