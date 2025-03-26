using UnityEngine;
using UnityEngine.UI;

public class ArtworkUI : SpawnableUI
{
    [SerializeField] Image _artImg;

    Image _artFrameImage;
    Button _artFrameBtn;
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

    public override void Initialize(Sprite artSprite, Sprite artFrameSprite)
    {
        Vector2Int size = ResizeSprite(artSprite);
        _artFrame.sizeDelta = size;
        _artImg.sprite = artSprite;

        _artFrameImage = _artFrame.gameObject.GetComponent<Image>();
        _artFrameBtn = _artFrame.gameObject.GetComponent<Button>();
        _artFrameImage.sprite = artFrameSprite;
    }

    System.Action OnClickRequested;

    public override void InjectClickEvent(System.Action OnClick)
    {
        OnClickRequested = OnClick;
        _artFrameBtn.onClick.AddListener(() => { OnClickRequested?.Invoke(); });
    }
}
