using UnityEngine;
using UnityEngine.UI;

public class ArtworkUI : SpawnableUI
{
    [SerializeField] Image _artFrameImage;
    [SerializeField] RectTransform _artFrame;
    [SerializeField] Button _artFrameBtn;

    [SerializeField] RefitableImage _refitableImage;

    [SerializeField] GameObject _lockObj;
    [SerializeField] Image _rankIcon;
    // 크기는 500 ~ 700 사이로 맞춰야함

    public override void Initialize(Sprite artSprite, Sprite artFrameSprite, Sprite rankIconSprite, bool hasIt = true)
    {
        _lockObj.SetActive(!hasIt);

        Vector2 changedSize = _refitableImage.ResizeImage(artSprite);

        _artFrame.sizeDelta = changedSize;
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
