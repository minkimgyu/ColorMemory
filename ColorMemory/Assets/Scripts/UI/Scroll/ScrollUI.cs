using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private Scrollbar _scrollbar;
    [SerializeField] private RectTransform _content;
    [SerializeField] private HorizontalLayoutGroup _layoutGroup;

    private float[] _itemCenters;
    private int _itemCount;
    private bool _isDragging;
    private float _targetPos;
    private int _targetIndex;

    public System.Action<int> OnDragEnd;

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

    public virtual void Setup()
    {
        _itemCount = _content.childCount;
        if (_itemCount == 0) return;

        CalculateContentWidth();
        ApplyEdgePadding();
        CalculateItemCenters();
        //ScrollTo(0);
    }

    private void CalculateContentWidth()
    {
        float spacing = _layoutGroup.spacing;
        float totalWidth = 0f;

        for (int i = 0; i < _itemCount; i++)
        {
            RectTransform item = _content.GetChild(i) as RectTransform;
            totalWidth += item.rect.width;
        }

        totalWidth += spacing * (_itemCount - 1);

        RectTransform first = _content.GetChild(0) as RectTransform;
        RectTransform last = _content.GetChild(_itemCount - 1) as RectTransform;

        float viewportWidth = _scrollRect.viewport.rect.width;
        float leftPadding = viewportWidth / 2f - first.rect.width / 2f;
        float rightPadding = viewportWidth / 2f - last.rect.width / 2f;

        totalWidth += leftPadding + rightPadding;

        _content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, totalWidth);
    }

    private void ApplyEdgePadding()
    {
        float viewportWidth = _scrollRect.viewport.rect.width;

        RectTransform first = _content.GetChild(0) as RectTransform;
        RectTransform last = _content.GetChild(_itemCount - 1) as RectTransform;

        float leftPadding = viewportWidth / 2f - first.rect.width / 2f;
        float rightPadding = viewportWidth / 2f - last.rect.width / 2f;

        _layoutGroup.padding.left = Mathf.RoundToInt(leftPadding);
        _layoutGroup.padding.right = Mathf.RoundToInt(rightPadding);
    }

    private void CalculateItemCenters()
    {
        float viewportWidth = _scrollRect.viewport.rect.width;
        float contentWidth = _content.rect.width;
        float spacing = _layoutGroup.spacing;

        _itemCenters = new float[_itemCount];
        float currentX = _layoutGroup.padding.left;

        for (int i = 0; i < _itemCount; i++)
        {
            RectTransform item = _content.GetChild(i) as RectTransform;
            float itemWidth = item.rect.width;

            currentX += itemWidth / 2f;

            float center = currentX;

            _itemCenters[i] = Mathf.Clamp01((center - viewportWidth / 2f) / (contentWidth - viewportWidth));

            currentX += itemWidth / 2f + spacing;
        }
    }

    public void ScrollTo(int index, bool immediate = false)
    {
        if (index < 0 || index >= _itemCount) return;
        _targetIndex = index;
        _targetPos = _itemCenters[index];

        if (immediate)
        {
            _scrollbar.value = _targetPos;
        }
    }

    private void Update()
    {
        if (!_isDragging)
        {
            _scrollbar.value = Mathf.Lerp(_scrollbar.value, _targetPos, 0.15f);
        }
    }

    public void OnBeginDrag(PointerEventData eventData) => _isDragging = true;
    public void OnDrag(PointerEventData eventData) => _isDragging = true;

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;

        float currentPos = _scrollbar.value;
        float closestDiff = float.MaxValue;
        int closestIndex = 0;

        for (int i = 0; i < _itemCenters.Length; i++)
        {
            float diff = Mathf.Abs(currentPos - _itemCenters[i]);
            if (diff < closestDiff)
            {
                closestDiff = diff;
                closestIndex = i;
            }
        }

        ScrollTo(closestIndex);
        OnDragEnd?.Invoke(closestIndex);
    }
}