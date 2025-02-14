using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class DotEffect : BaseEffect
{
    Image _image;
    RectTransform _rectTransform;

    public override void Initialize()
    {
        _image = GetComponentInChildren<Image>();
        _rectTransform = GetComponent<RectTransform>();
    }

    public override void ChangeSize(Vector2 size)
    {
        _rectTransform.sizeDelta = size;
    }

    public override void ChangeColor(Color color)
    {
        _image.color = color;
    }

    public override void Fade(float duration, Image.FillMethod fillMethod, Action OnComplete = null)
    {
        _image.type = Image.Type.Filled;
        _image.fillMethod = fillMethod;
        _image.fillAmount = 1;

        // .SetLink(gameObject)를 활용하면 파괴될 때 자동으로 트위닝이 중지됨

        _image.DOFillAmount(0, duration).SetLink(gameObject).onComplete = () => { OnComplete?.Invoke(); };
    }

    public override void Scale(float endScale, float duration, Action OnComplete = null)
    {
        _rectTransform.DOScale(endScale, duration).SetLink(gameObject);
    }

    public override void Color(Color endColor, float duration, Action OnComplete = null)
    {
        _image.DOColor(endColor, duration).SetLink(gameObject).onComplete = () => { OnComplete?.Invoke(); };
    }
}