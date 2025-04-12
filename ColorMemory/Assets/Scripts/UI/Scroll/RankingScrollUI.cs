//using System;
//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//public class RankingScrollUI : ScrollUI
//{
//    const float _selectedSize = 1.3f;
//    Timer _scaleChangeTimer;
//    RectTransform _contentRectTransform;

//    const int menuSize = 250;

//    List<RankingUI> _rankingUIs = new List<RankingUI>();

//    public override void SetUp(int menuCount, int startIndex = 0)
//    {
//        FitVerticalLayoutGroup(menuSize);
//        _isHorizontal = false;
//        base.SetUp(menuCount, startIndex);
//        _scaleChangeTimer = new Timer();
//        _contentRectTransform = _content.GetComponent<RectTransform>();
//        ScrollToLevel(startIndex);
//    }

//    void FitVerticalLayoutGroup(int menuSize)
//    {
//        Canvas parentCanvas = GetComponentInParent<Canvas>();
//        RectTransform rectTransform = parentCanvas.GetComponent<RectTransform>();

//        int rectHalfSize = (int)(menuSize * 1 / 2);
//        int offset = ((int)rectTransform.rect.width / 2) - rectHalfSize;

//        VerticalLayoutGroup verticalLayoutGroup = _content.GetComponent<VerticalLayoutGroup>();
//        verticalLayoutGroup.padding.top = offset;
//        verticalLayoutGroup.padding.bottom = offset;
//    }

//    public override void OnEndDrag(PointerEventData eventData)
//    {
//        base.OnEndDrag(eventData);

//        _scaleChangeTimer.Reset();
//        _scaleChangeTimer.Start(1f);
//    }

//    public void ScrollToLevel(int index)
//    {
//        _currentPos = GetPos();

//        _targetIndex = index;
//        _targetPos = _points[_targetIndex];

//        _scaleChangeTimer.Reset();
//        _scaleChangeTimer.Start(1f);
//    }

//    public override void AddItem(Transform item)
//    {
//        base.AddItem(item);
//        _rankingUIs.Add(item.GetComponent<RankingUI>());
//    }


//    public override void AddItem(Transform item, bool setToMiddle)
//    {
//        base.AddItem(item, setToMiddle);

//        _rankingUIs.Clear();
//        for (int i = 0; i < _content.childCount; i++)
//        {
//            _rankingUIs.Add(_content.GetChild(i).GetComponent<RankingUI>());
//        }
//    }

//    public override void DestroyItems()
//    {
//        base.DestroyItems();
//        _rankingUIs.Clear();
//    }

//    //void ScaleTarget(float ratio)
//    //{
//    //    for (int i = 0; i < _rankingUIs.Count; i++)
//    //    {
//    //        RankingUI rankingUI = _rankingUIs[i];

//    //        if (i == _targetIndex)
//    //        {
//    //            rankingUI.ChangeSelect(true);
//    //            rankingUI.ChangeScale(Vector3.one, ratio);
//    //        }
//    //        else
//    //        {
//    //            rankingUI.ChangeSelect(false);
//    //            rankingUI.ChangeScale(Vector3.one * 0.8f, ratio);
//    //        }
//    //    }
//    //}

//    //protected override void Update()
//    //{
//    //    base.Update();
//    //    if (_scaleChangeTimer.CurrentState == Timer.State.Running)
//    //    {
//    //        ScaleTarget(_scaleChangeTimer.Ratio);
//    //        LayoutRebuilder.ForceRebuildLayoutImmediate(_contentRectTransform);
//    //    }
//    //}
//}
