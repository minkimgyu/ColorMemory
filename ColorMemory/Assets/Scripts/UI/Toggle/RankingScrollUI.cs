using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RankingScrollUI : ScrollUI
{
    const float _selectedSize = 1.3f;
    Timer _scaleChangeTimer;
    RectTransform _contentRectTransform;

    const int menuSize = 250;

    public override void SetUp(int menuCount, int startIndex = 0)
    {
        FitVerticalLayoutGroup(menuSize);
        _isHorizontal = false;
        base.SetUp(menuCount, startIndex);
        _scaleChangeTimer = new Timer();
        _contentRectTransform = _content.GetComponent<RectTransform>();

        ScrollToLevel(startIndex);
    }

    void FitVerticalLayoutGroup(int menuSize)
    {
        Canvas parentCanvas = GetComponentInParent<Canvas>();
        RectTransform rectTransform = parentCanvas.GetComponent<RectTransform>();

        int rectHalfSize = (int)(menuSize * 1 / 2);
        int offset = ((int)rectTransform.rect.width / 2) - rectHalfSize;

        VerticalLayoutGroup verticalLayoutGroup = _content.GetComponent<VerticalLayoutGroup>();
        verticalLayoutGroup.padding.top = offset;
        verticalLayoutGroup.padding.bottom = offset;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        _scaleChangeTimer.Reset();
        _scaleChangeTimer.Start(1f);
    }

    public void ScrollToLevel(int index)
    {
        _currentPos = GetPos();

        _targetIndex = index;
        _targetPos = _points[_targetIndex];

        _scaleChangeTimer.Reset();
        _scaleChangeTimer.Start(1f);
    }

    void ScaleTarget(float ratio = 1)
    {
        for (int i = 0; i < _content.childCount; i++)
        {
            if (i == _targetIndex)
            {
                Transform targetTr = _content.GetChild(i);
                targetTr.localScale = Vector3.Lerp(targetTr.localScale, Vector3.one, ratio);
            }
            else
            {
                Transform targetTr = _content.GetChild(i);
                targetTr.localScale = Vector3.Lerp(targetTr.localScale, Vector3.one * 0.8f, ratio);
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        if (_scaleChangeTimer.CurrentState == Timer.State.Running)
        {
            ScaleTarget(_scaleChangeTimer.Ratio);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_contentRectTransform);
        }
    }
}
