using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

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

    //public void Fade(Color endColor, Image.FillMethod method, float duration)
    //{
    //    Effect fadeEffect = _effectFactory.Create(Effect.Name.CircleEffect);

    //    fadeEffect.transform.SetParent(transform);
    //    fadeEffect.transform.SetAsLastSibling();

    //    fadeEffect.transform.position = transform.position;

    //    fadeEffect.ChangeSize(_dotTransform.sizeDelta);
    //    fadeEffect.ChangeColor(_dotImage.color);

    //    ChangeColor(endColor); // 본인 색 바꾸기

    //    fadeEffect.Fade(transform.localScale, duration, method,
    //        () =>
    //        {
    //            Destroy(fadeEffect.gameObject);
    //        }
    //    );
    //}

    public void Expand(float endScale, Color endColor, float duration)
    {
        Color originColor = _dotImage.color;
        ChangeColor(new Color(0, 0, 0, 0));

        Effect expendEffect = _effectFactory.Create(Effect.Name.CircleEffect);
        expendEffect.name = "expendEffect";

        expendEffect.transform.SetParent(transform);
        expendEffect.transform.SetAsLastSibling();

        expendEffect.transform.position = transform.position;
        expendEffect.transform.localScale = Vector3.zero;

        expendEffect.ChangeSize(_dotTransform.sizeDelta);
        expendEffect.ChangeColor(endColor);

        expendEffect.Scale(endScale, duration,
            () =>
            {
                Destroy(expendEffect.gameObject);
            }
        );


        Effect fadeEffect = _effectFactory.Create(Effect.Name.CircleEffect);
        fadeEffect.name = "fadeEffect";

        fadeEffect.transform.SetParent(transform);
        fadeEffect.transform.SetAsLastSibling();

        fadeEffect.transform.position = transform.position;
        fadeEffect.transform.localScale = Vector3.one;

        fadeEffect.ChangeSize(_dotTransform.sizeDelta);
        fadeEffect.ChangeColor(originColor);

        fadeEffect.Color(new Color(originColor.r, originColor.g, originColor.b, 0), duration,
            () =>
            {
                ChangeColor(endColor);
                Destroy(fadeEffect.gameObject);
            }
        );
    }

    public void Pop(Color endColor, float duration)
    {
        ChangeColor(endColor);

        Effect diffuseEffect = _effectFactory.Create(Effect.Name.CircleEffect);
        diffuseEffect.name = "diffuseEffect";

        diffuseEffect.transform.SetParent(_dotTransform);
        diffuseEffect.transform.SetAsFirstSibling();

        diffuseEffect.transform.position = _dotTransform.position;

        diffuseEffect.ChangeSize(_dotTransform.sizeDelta);
        diffuseEffect.ChangeScale(Vector3.one);

        diffuseEffect.Scale(1.75f, duration);
        diffuseEffect.Color(new Color(endColor.r * 0.7f, endColor.g * 0.7f, endColor.b * 0.7f, 0), duration,
            () =>
            {
                Destroy(diffuseEffect.gameObject);
            }
        );

        Effect fadeEffect = _effectFactory.Create(Effect.Name.CircleEffect);
        fadeEffect.name = "fadeEffect";

        fadeEffect.transform.SetParent(transform);
        fadeEffect.transform.SetAsLastSibling();

        fadeEffect.transform.position = transform.position;

        fadeEffect.ChangeSize(_dotTransform.sizeDelta);
        fadeEffect.ChangeScale(Vector3.one);

        fadeEffect.Fade(transform.localScale, duration, Image.FillMethod.Horizontal,
            () =>
            {
                Destroy(fadeEffect.gameObject);
            }
        );
    }

    public void XSlide(Color endColor, float duration)
    {
        //ChangeColor(endColor);

        Effect diffuseEffect = _effectFactory.Create(Effect.Name.CircleEffect);
        diffuseEffect.name = "diffuseEffect";

        diffuseEffect.transform.SetParent(_dotTransform);
        diffuseEffect.transform.SetAsFirstSibling();

        diffuseEffect.transform.position = _dotTransform.position;

        diffuseEffect.ChangeSize(_dotTransform.sizeDelta);
        diffuseEffect.ChangeScale(Vector3.one);

        diffuseEffect.Scale(1.75f, duration);
        diffuseEffect.Color(new Color(endColor.r * 0.7f, endColor.g * 0.7f, endColor.b * 0.7f, 0), duration,
            () =>
            {
                Destroy(diffuseEffect.gameObject);
            }
        );

        Effect fadeEffect = _effectFactory.Create(Effect.Name.XEffect);
        fadeEffect.name = "fadeEffect";

        fadeEffect.transform.SetParent(transform);
        fadeEffect.transform.SetAsLastSibling();

        fadeEffect.transform.position = transform.position;

        fadeEffect.ChangeSize(_dotTransform.sizeDelta);
        fadeEffect.ChangeScale(Vector3.zero);
        fadeEffect.Scale(1, duration,
            () =>
            {
                fadeEffect.Scale(0, duration,
                    () =>
                    {
                        Destroy(fadeEffect.gameObject);
                    }
                );
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
