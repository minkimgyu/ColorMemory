using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FilteredArtworkUI : SpawnableUI, IScrollItem
{
    [SerializeField] Button _selectButton;
    [SerializeField] Image _artImg;
    [SerializeField] TMP_Text _artTitle;
    [SerializeField] GameObject _lockPanel;

    [SerializeField] ResizeableImage _resizeableImage;

    const int _maxStringLength = 20;

    public override void Initialize(Sprite artSprite, string title, bool hasIt = true)
    {
        _resizeableImage.Initialize(artSprite);

        _lockPanel.SetActive(!hasIt);
        _artImg.sprite = artSprite;
        if (title.Length > _maxStringLength) _artTitle.text = $"{title.Substring(0, _maxStringLength)}...";
        else _artTitle.text = title;
    }

    System.Action OnClickRequested;

    public override void InjectClickEvent(System.Action OnClick)
    {
        OnClickRequested = OnClick;
        _selectButton.onClick.AddListener(() => 
        { 
            OnClickRequested?.Invoke();
            ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.BtnClick);
        });
    }

    public void ChangeLocalScale(Vector2 scale)
    {
        transform.localScale = scale;
    }

    public void ChangeLocalPosition(Vector2 pos)
    {
        transform.localPosition = pos;
    }

    public Vector2 GetLocalPosition()
    {
        return transform.localPosition;
    }

    public void ChangeSibiling(bool toTtop)
    {
        if (toTtop) transform.SetAsFirstSibling();
        else transform.SetAsLastSibling();
    }

    public void Active(bool nowActive)
    {
        gameObject.SetActive(nowActive);
    }

    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }

    System.Action<IPoolObject> ReturnToPoolEvent;

    void RemoveEvent()
    {
        // 이벤트 지우기
        _selectButton.onClick.RemoveAllListeners();
        OnClickRequested = null;
    }

    public void ReturnToPool()
    {
        RemoveEvent();
        ReturnToPoolEvent?.Invoke(this);
    }

    public void InjectReturnToPoolEvent(System.Action<IPoolObject> ReturnToPoolEvent)
    {
        this.ReturnToPoolEvent = ReturnToPoolEvent;
    }
}
