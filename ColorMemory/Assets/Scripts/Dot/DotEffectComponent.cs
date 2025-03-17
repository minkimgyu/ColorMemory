using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class DotEffectComponent : MonoBehaviour
{
    EffectFactory _effectFactory;
    [SerializeField] RectTransform _dotTransform;
    Image _dotImage;

    public void Initialize()
    {
        _dotImage = GetComponentInChildren<Image>();
    }

    public void Inject(EffectFactory effectFactory)
    {
        _effectFactory = effectFactory;
    }

    public void Fade(Color endColor, Image.FillMethod method, float duration)
    {
        Effect fadeEffect = _effectFactory.Create(Effect.Name.CircleEffect);

        fadeEffect.transform.SetParent(transform);
        fadeEffect.transform.SetAsLastSibling();

        fadeEffect.transform.position = transform.position;

        fadeEffect.ChangeSize(_dotTransform.sizeDelta);
        fadeEffect.ChangeColor(_dotImage.color);

        ChangeColor(endColor); // 본인 색 바꾸기

        fadeEffect.Fade(transform.localScale, duration, method,
            () =>
            {
                Destroy(fadeEffect.gameObject);
            }
        );
    }

    public void Pop(Color endColor, float duration)
    {
        ChangeColor(endColor);

        Effect diffuseEffect = _effectFactory.Create(Effect.Name.CircleEffect);

        diffuseEffect.transform.SetParent(_dotTransform);
        diffuseEffect.transform.SetAsFirstSibling();

        diffuseEffect.transform.position = _dotTransform.position;

        diffuseEffect.ChangeSize(_dotTransform.sizeDelta);

        diffuseEffect.Scale(1.75f, duration);
        diffuseEffect.Color(new Color(endColor.r * 0.7f, endColor.g * 0.7f, endColor.b * 0.7f, 0), duration,
            () =>
            {
                Destroy(diffuseEffect.gameObject);
            }
        );

        Effect fadeEffect = _effectFactory.Create(Effect.Name.CircleEffect);

        fadeEffect.transform.SetParent(transform);
        fadeEffect.transform.SetAsLastSibling();

        fadeEffect.transform.position = transform.position;

        fadeEffect.ChangeSize(_dotTransform.sizeDelta);

        fadeEffect.Fade(transform.localScale, duration, Image.FillMethod.Horizontal,
            () =>
            {
                Destroy(fadeEffect.gameObject);
            }
        );
    }

    public void Fill(Color endColor, float duration)
    {
        ChangeColor(endColor);

        Effect rectEffect = _effectFactory.Create(Effect.Name.RectEffect);

        rectEffect.transform.SetParent(transform);
        rectEffect.transform.SetAsLastSibling();

        rectEffect.transform.position = transform.position;

        rectEffect.ChangeSize(_dotTransform.sizeDelta);
        rectEffect.ChangeColor(endColor);

        rectEffect.Scale(1 + 0.07f, duration,
            () =>
            {
                Destroy(rectEffect.gameObject);
            }
        );
    }

    public void ChangeColor(Color color)
    {
        _dotImage.color = color;
    }

    public void Scale(float endScale, float duration)
    {
        _dotTransform.DOScale(endScale, duration).SetLink(gameObject);
    }

    public void Scale(float endScale)
    {
        _dotTransform.localScale = Vector3.one * endScale;
    }
}
