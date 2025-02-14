using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BasicDot : Dot
{
    Vector2Int _index;
    protected Action<Vector2Int> OnClick;

    public override void Inject(EffectFactory effectFactory, Vector2Int index, Action<Vector2Int> OnClick)
    {
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
