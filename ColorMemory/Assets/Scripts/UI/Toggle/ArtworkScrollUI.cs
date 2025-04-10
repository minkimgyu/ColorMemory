using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtworkScrollUI : ScrollUI
{
    const int menuSize = 800;

    public override void SetUp(int menuCount, int startIndex = 0)
    {
        FitHorizontalLayoutGroup(menuSize);
        _isHorizontal = true;
        base.SetUp(menuCount, startIndex);
    }

    void FitHorizontalLayoutGroup(int menuSize)
    {
        Canvas parentCanvas = GetComponentInParent<Canvas>();
        RectTransform rectTransform = parentCanvas.GetComponent<RectTransform>();

        int rectHalfSize = (int)(menuSize * 1 / 2);
        int offset = ((int)rectTransform.rect.width / 2) - rectHalfSize;

        HorizontalLayoutGroup horizontalLayoutGroup = _content.GetComponent<HorizontalLayoutGroup>();
        horizontalLayoutGroup.padding.left = offset;
        horizontalLayoutGroup.padding.right = offset;
    }
}
