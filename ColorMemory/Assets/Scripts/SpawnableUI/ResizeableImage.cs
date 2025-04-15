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
        // ���� �ؽ�ó ũ�� ��������
        int originalWidth = sprite.texture.width;
        int originalHeight = sprite.texture.height;

        // ���� ���
        float aspectRatio = (float)originalWidth / originalHeight;

        // ��ǥ ũ�� ���
        int targetWidth = originalWidth;
        int targetHeight = originalHeight;

        // ���� �Ǵ� ���θ� 700���� ���߰�, ���� ����
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

        // 500~700 ���� ������ ũ�� ����
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
