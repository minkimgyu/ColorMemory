using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FilterItemUI : SpawnableUI
{
    [SerializeField] Button _xButton;
    [SerializeField] Image _iconImg;
    [SerializeField] TMP_Text _descriptionText;

    public override void Initialize(Sprite artSprite, string description)
    {
        _iconImg.sprite = artSprite;
        _descriptionText.text = description;
    }

    public override void Initialize(string description) 
    {
        _iconImg.gameObject.SetActive(false);
        _descriptionText.text = description;
    }


    System.Action OnClickRequested;

    public override void InjectClickEvent(System.Action OnClick)
    {
        OnClickRequested = OnClick;
        _xButton.onClick.AddListener(() => { 
            OnClickRequested?.Invoke();
        });
    }
}
