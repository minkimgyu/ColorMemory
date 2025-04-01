using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

abstract public class Dot : MonoBehaviour, IPointerDownHandler
{
    public enum Name
    {
        Basic,
        ColorPen,
    }

    protected bool _clickable = true;
    public bool Clickable { get => _clickable; set => _clickable = value; }

    protected DotEffectComponent _dotEffectComponent;

    public virtual void Initialize()
    {
        _dotEffectComponent = GetComponent<DotEffectComponent>();
        _dotEffectComponent.Initialize();
    }

    public virtual void Inject(EffectFactory effectFactory, Vector2Int index, Action<Vector2Int> OnClick) { }
    public virtual void Inject(EffectFactory effectFactory, ToggleGroup toggleGroup, int index, Action<int> OnClick) { }


    public virtual void ChangeColorCount(int colorCount) { }

    public virtual void ChangeColor(Color color)
    {
        _dotEffectComponent.ChangeColor(color);
    }

    public void Minimize()
    {
        _dotEffectComponent.Scale(0);
    }

    public void Minimize(float duration)
    {
        _dotEffectComponent.Scale(0f, duration);
    }

    public void Maximize(float duration)
    {
        _dotEffectComponent.Scale(1f, duration);
    }

    public void Expand(Color endColor, float duration, Action OnComplete = null)
    {
        _dotEffectComponent.Expand(1f, endColor, duration, OnComplete);
    }

    //public void Fade(Color color, Image.FillMethod method, float duration)
    //{
    //    _dotEffectComponent.Fade(color, method, duration);
    //}

    public void Pop(Color changeColor)
    {
        _dotEffectComponent.Pop(changeColor, 0.5f);
    }

    public abstract void OnPointerDown(PointerEventData eventData);
}
