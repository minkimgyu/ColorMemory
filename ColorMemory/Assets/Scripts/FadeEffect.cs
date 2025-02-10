using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeEffect : MonoBehaviour
{
    Sprite _circleSprite;
    Sprite _squareSprite;

    Image _myImage;
    Color _myColor;
    RectTransform _myRectTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _myImage = GetComponent<Image>();
        _myColor = _myImage.color;
        _myRectTransform = GetComponent<RectTransform>();

        _circleSprite = Resources.Load<Sprite>("Sprites/Circle");
        _squareSprite = Resources.Load<Sprite>("Sprites/Square");
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Scale(1, 0.8f);
        }

        if (Input.GetMouseButtonDown(2))
        {
            Fade(Color.green, 0.8f, Image.FillMethod.Horizontal);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Scale(0, 0.8f);
        }
    }

    public void Fade(Color endColor, float duration, Image.FillMethod fillMethod)
    {
        _myImage.color = endColor;

        GameObject circle = new GameObject("Fade");
        RectTransform rect = circle.AddComponent<RectTransform>();
        Image image = circle.AddComponent<Image>();

        circle.transform.SetParent(transform);

        rect.sizeDelta = new Vector2(_myRectTransform.rect.width, _myRectTransform.rect.height);
        rect.localPosition = Vector2.zero;

        image.sprite = _circleSprite;

        image.type = Image.Type.Filled;
        image.fillMethod = fillMethod;
        image.fillAmount = 1;
        image.color = _myColor;

        image.DOFillAmount(0, duration).onComplete = () => { Destroy(image.gameObject); };
    }

    public void Scale(float endScale, float duration)
    {
        _myRectTransform.DOScale(endScale, duration);
    }
}
