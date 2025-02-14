using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ColorPenDot : Dot
{
    int _index;
    protected Action<int> OnClick;

    Toggle _toggle;

    public override void Initialize()
    {
        base.Initialize();
        _toggle = GetComponent<Toggle>();
    }

    public override void Inject(EffectFactory effectFactory, ToggleGroup toggleGroup, int index, Action<int> OnClick)
    {
        _toggle.group = toggleGroup;
        _dotEffectComponent.Inject(effectFactory);
        _index = index;
        this.OnClick = OnClick;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (_clickable == false) return;

        Debug.Log("Click");
        OnClick?.Invoke(_index);
    }
}
