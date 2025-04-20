using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResizeableImage : MonoBehaviour
{
    [SerializeField] Image _artImg;
    RectTransform _artRect;

    [SerializeField] int minSize = 500;
    [SerializeField] int maxSize = 700;

    Vector2Int _changedSize;
    public Vector2Int ChangedSize { get => _changedSize; }

    public void Initialize(Sprite artSprite)
    {
        _artRect = _artImg.gameObject.GetComponent<RectTransform>();

        Vector2Int size = ResizeSprite(artSprite);
        _artRect.sizeDelta = size;

        _artImg.sprite = artSprite;
        _changedSize = size;
    }

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
}
