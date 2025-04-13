using UnityEngine;
using UnityEngine.UI;

public class ArtworkUI : SpawnableUI
{
    [SerializeField] Image _artFrameImage;
    [SerializeField] RectTransform _artFrame;
    [SerializeField] Button _artFrameBtn;

    [SerializeField] ResizeableImage _resizeableImage;

    [SerializeField] GameObject _lockObj;
    [SerializeField] Image _rankIcon;
    // ũ��� 500 ~ 700 ���̷� �������

    public override void Initialize(Sprite artSprite, Sprite artFrameSprite, Sprite rankIconSprite, bool hasIt = true)
    {
        _lockObj.SetActive(!hasIt);

        _resizeableImage.Initialize(artSprite);

        _artFrame.sizeDelta = _resizeableImage.ChangedSize;
        _artFrameImage.sprite = artFrameSprite;
        _rankIcon.sprite = rankIconSprite;
    }

    System.Action OnClickRequested;

    public override void InjectClickEvent(System.Action OnClick)
    {
        OnClickRequested = OnClick;
        _artFrameBtn.onClick.AddListener(() => { OnClickRequested?.Invoke(); });
    }
}
