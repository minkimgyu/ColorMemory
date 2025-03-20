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
    // ũ��� 500 ~ 700 ���̷� �������

    const int minSize = 500;
    const int maxSize = 700;

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
