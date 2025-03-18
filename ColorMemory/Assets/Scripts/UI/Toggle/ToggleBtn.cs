using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Button))]
public class ToggleBtn : MonoBehaviour
{
    public System.Action<bool> OnClick { get; set; }

    bool _isOn = false;
    public bool IsOn { get => _isOn; }

    [SerializeField] Color _offColor;
    [SerializeField] Color _onColor;

    Button _btn;
    Image _iconImage;

    [SerializeField] RectTransform _toggleIcon;

    readonly Vector2 _offPoint = new Vector2(-40, 0);
    readonly Vector2 _onPoint = new Vector2(40, 0);
    const float _iconMoveDuration = 0.3f;

    public void Initialize()
    {
        _iconImage = _toggleIcon.gameObject.GetComponent<Image>();
        _iconImage.color = _offColor;

        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(ChangeState);
    }

    void ChangeState()
    {
        _isOn = !_isOn;
        UpdateIcon();
        OnClick?.Invoke(_isOn);
    }

    public void ChangeState(bool isOn)
    {
        _isOn = isOn;
        UpdateIcon();
        OnClick?.Invoke(_isOn);
    }

    void UpdateIcon()
    {
        if (_isOn)
        {
            _iconImage.DOColor(_onColor, _iconMoveDuration);
            _toggleIcon.DOAnchorPos(_onPoint, _iconMoveDuration);
        }
        else
        {
            _iconImage.DOColor(_offColor, _iconMoveDuration);
            _toggleIcon.DOAnchorPos(_offPoint, _iconMoveDuration);
        }
    }
}
