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
        BaseEffect fadeEffect = _effectFactory.Create(BaseEffect.Name.CircleEffect);

        fadeEffect.transform.SetParent(transform);
        fadeEffect.transform.SetAsLastSibling();

        fadeEffect.transform.position = transform.position;

        fadeEffect.ChangeSize(_dotTransform.sizeDelta);
        fadeEffect.ChangeColor(_dotImage.color);

        ChangeColor(endColor); // 본인 색 바꾸기

        fadeEffect.Fade(duration, method,
            () =>
            {
                Destroy(fadeEffect.gameObject);
            }
        );
    }

    public void Pop(Color endColor, float duration)
    {
        ChangeColor(endColor);

        BaseEffect diffuseEffect = _effectFactory.Create(BaseEffect.Name.CircleEffect);

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

        BaseEffect fadeEffect = _effectFactory.Create(BaseEffect.Name.CircleEffect);

        fadeEffect.transform.SetParent(transform);
        fadeEffect.transform.SetAsLastSibling();

        fadeEffect.transform.position = transform.position;

        fadeEffect.ChangeSize(_dotTransform.sizeDelta);

        fadeEffect.Fade(duration, Image.FillMethod.Horizontal,
            () =>
            {
                Destroy(fadeEffect.gameObject);
            }
        );
    }

    public void Fill(Color endColor, float duration)
    {
        ChangeColor(endColor);

        BaseEffect rectEffect = _effectFactory.Create(BaseEffect.Name.RectEffect);

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
