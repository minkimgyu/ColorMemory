using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class RefitableImage : MonoBehaviour
{
    [SerializeField] float fixedHeight = 600f;

    public Vector2 ResizeImage(Sprite sprite)
    {
        if (sprite == null) return Vector2.zero;

        RectTransform rt = GetComponent<RectTransform>();
        Image image = GetComponent<Image>();
        image.sprite = sprite;

        float spriteWidth = sprite.texture.width;
        float spriteHeight = sprite.texture.height;
        float aspectRatio = spriteWidth / spriteHeight;

        return new Vector2(fixedHeight * aspectRatio, fixedHeight);
    }
}
