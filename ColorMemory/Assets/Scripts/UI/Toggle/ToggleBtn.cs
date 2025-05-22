using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Button))]
public class ToggleBtn : MonoBehaviour
{
    public System.Action<bool, bool> OnClick { get; set; }

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

    public void Initialize(float delayTime)
    {
        _iconImage = _toggleIcon.gameObject.GetComponent<Image>();
        _iconImage.color = _offColor;

        _timer = new Timer();
        _delayTime = delayTime;

        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(ChangeState);
    }

    float _delayTime;
    Timer _timer;

    void ChangeState()
    {
        if (_timer.CurrentState == Timer.State.Running) return;
        _timer.Reset();
        _timer.Start(_delayTime);

        _isOn = !_isOn;
        UpdateIcon();
        OnClick?.Invoke(true, _isOn); // UI 입력 이벤트
    }

    public void ChangeState(bool isOn)
    {
        if (_timer.CurrentState == Timer.State.Running) return;
        _timer.Reset();
        _timer.Start(_delayTime);

        _isOn = isOn;
        UpdateIcon();
        OnClick?.Invoke(false, _isOn);
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
