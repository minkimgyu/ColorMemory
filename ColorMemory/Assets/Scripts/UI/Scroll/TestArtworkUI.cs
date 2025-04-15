using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestArtworkUI : MonoBehaviour
{
    [SerializeField] Image _artImg;
    [SerializeField] RectTransform _artFrame;
    // ũ��� 500 ~ 700 ���̷� �������

    [SerializeField] int minSize = 500;
    [SerializeField] int maxSize = 700;

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

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        Vector2Int size = ResizeSprite(_artImg.sprite);
        _artFrame.sizeDelta = size;
    }
}
