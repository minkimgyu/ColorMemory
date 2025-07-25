using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefitableImage : MonoBehaviour
{
    [SerializeField] float fixedHeight = 600f;
    [SerializeField] float fixedWidth = 600f;
    [SerializeField] Image image;

    public Vector2 ResizeImageWithHeight(Sprite sprite)
    {
        if (sprite == null) return Vector2.zero;
        image.sprite = sprite;

        float spriteWidth = sprite.texture.width;
        float spriteHeight = sprite.texture.height;
        float aspectRatio = spriteWidth / spriteHeight;

        return new Vector2(fixedHeight * aspectRatio, fixedHeight);
    }

    public Vector2 ResizeImageWithWidth(Sprite sprite)
    {
        if (sprite == null) return Vector2.zero;
        image.sprite = sprite;

        float spriteWidth = sprite.texture.width;
        float spriteHeight = sprite.texture.height;
        float aspectRatio = spriteHeight / spriteWidth;

        return new Vector2(fixedWidth, fixedWidth * aspectRatio);
    }
}
