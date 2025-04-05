using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

abstract public class ScrollUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] Scrollbar _scrollbar;
    [SerializeField] protected Transform _content;
    [SerializeField] RectTransform _scrollRect;

    protected int _menuSize;
    protected float[] _points;
    protected float _currentPos, _targetPos;
    protected bool _isDrag;

    [SerializeField] protected int _targetIndex;

    public System.Action<int> OnDragEnd { get; set; }

    public virtual void AddItem(Transform item)
    {
        item.SetParent(_content);
        item.transform.localScale = Vector3.one;
    }

    public virtual void DestroyItems()
    {
        for (int i = _content.childCount - 1; i >= 0; i--)
        {
            _content.GetChild(i).GetComponent<SpawnableUI>().DestroyObject();
        }
    }

    void FitHorizontalLayoutGroup(int leftMenuSize, int rightMenuSize)
    {
        Canvas parentCanvas = GetComponentInParent<Canvas>();
        RectTransform rectTransform = parentCanvas.GetComponent<RectTransform>();

        int leftRectHalfSize = (int)(leftMenuSize / 2);
        int leftOffset = ((int)rectTransform.rect.width / 2) - leftRectHalfSize;

        int rightRectHalfSize = (int)(rightMenuSize / 2);
        int rightOffset = ((int)rectTransform.rect.width / 2) - rightRectHalfSize;

        HorizontalLayoutGroup horizontalLayoutGroup = _content.GetComponent<HorizontalLayoutGroup>();
        horizontalLayoutGroup.padding.left = leftRectHalfSize + leftOffset + (int)(horizontalLayoutGroup.spacing / 2);
        horizontalLayoutGroup.padding.right = rightRectHalfSize + rightOffset + (int)(horizontalLayoutGroup.spacing / 2);
    }

    public virtual void SetUp(int menuCount, int startIndex = 0)
    {
        _menuSize = menuCount;
        _points = new float[_menuSize];

        float totalSize = 0f;
        float[] itemSizes = new float[_menuSize];
        float[] itemCenters = new float[_menuSize];

        for (int i = 0; i < _menuSize; i++)
        {
            RectTransform item = _content.GetChild(i) as RectTransform;
            itemSizes[i] = item.rect.width;
            totalSize += itemSizes[i];
        }

        FitHorizontalLayoutGroup((int)itemSizes[0], (int)itemSizes[_content.childCount - 1]);

        float cumulativeSize = 0;
        for (int i = 0; i < _menuSize; i++)
        {
            cumulativeSize += itemSizes[i] / 2;
            itemCenters[i] = cumulativeSize;
            cumulativeSize += itemSizes[i] / 2;
        }

        for (int i = 0; i < _menuSize; i++)
        {
            _points[i] = itemCenters[i] / totalSize;
        }

        _targetPos = _points[startIndex];
    }

    protected virtual void Update()
    {
        if (!_isDrag)
        {
            _scrollbar.value = Mathf.Lerp(_scrollbar.value, _targetPos, 0.1f);
        }
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        _currentPos = GetPos();
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        _isDrag = true;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        _isDrag = false;
        _targetPos = GetPos();

        if (_currentPos == _targetPos)
        {
            if (eventData.delta.x > 18 && _targetIndex > 0)
            {
                --_targetIndex;
            }
            else if (eventData.delta.x < -18 && _targetIndex < _menuSize - 1)
            {
                ++_targetIndex;
            }
            _targetPos = _points[_targetIndex];
        }

        OnDragEnd?.Invoke(_targetIndex);
    }

    protected float GetPos()
    {
        float minDiff = float.MaxValue;
        int closestIndex = 0;

        for (int i = 0; i < _menuSize; i++)
        {
            float diff = Mathf.Abs(_scrollbar.value - _points[i]);
            if (diff < minDiff)
            {
                minDiff = diff;
                closestIndex = i;
            }
        }

        _targetIndex = closestIndex;
        return _points[closestIndex];
    }
}