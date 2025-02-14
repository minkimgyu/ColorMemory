using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

abstract public class BaseEffect : MonoBehaviour
{
    public enum Name
    {
        CircleEffect,
        RectEffect,
    }

    public abstract void Initialize();
    public abstract void ChangeSize(Vector2 size);
    public abstract void ChangeColor(Color color);
    public abstract void Color(Color endColor, float duration, Action OnComplete = null);
    public abstract void Fade(float duration, Image.FillMethod fillMethod, Action OnComplete = null);
    public abstract void Scale(float endScale, float duration, Action OnComplete = null);
}
