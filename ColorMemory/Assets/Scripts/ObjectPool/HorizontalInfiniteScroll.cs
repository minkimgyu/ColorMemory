using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using UnityEngine.UI;

public class HorizontalInfiniteScroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] RectTransform _leftPoint;
    [SerializeField] RectTransform _rightPoint;

    float _leftOffset;
    float _rightOffset;

    int _firstItemIndex = 0; // 첫 아이템 인덱스
    int _lastItemIndex = 0; // 첫 아이템 인덱스

    LinkedList<IScrollItem> _itemList;

    public event System.Func<int, IScrollItem> GetItem;

    //float _itemSize = 150;
    int _itemTotalCount = 100;

    List<int> _currentItemIndexes = new List<int>();
    List<Vector2> _totalItemSizes;
    //List<float> itemSpawnPos;

    public event System.Action<int> OnDragEnd;

    //int[] itemRanks;
    private float[] _itemCenters;
    private float[] _itemScrollRatios;
    [SerializeField] private ScrollRect _scrollRect;
    RectTransform _scroll;
    RectTransform _viewport;
    RectTransform _content;


    [SerializeField] float _spacing = 800;
    [SerializeField] float _itemHeight = 600;
    float _leftPadding = 0;
    float _rightPadding = 0;


    private bool _isDragging;
    private float _targetPos;

    void ScrollTo(int index)
    {
        if (index < 0 || index >= _itemTotalCount) return;
        _targetPos = _itemScrollRatios[index];
    }

    public void OnBeginDrag(PointerEventData eventData) => _isDragging = true;

    private float _dragDeltaX = 0;

    public void OnDrag(PointerEventData eventData)
    {
        _isDragging = true;
        _dragDeltaX = eventData.delta.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;
        _dragDeltaX = 0;

        float currentPos = _scrollRect.horizontalScrollbar.value;
        float closestDiff = float.MaxValue;
        int closestIndex = 0;

        for (int i = 0; i < _itemScrollRatios.Length; i++)
        {
            float diff = Mathf.Abs(currentPos - _itemScrollRatios[i]);
            if (diff < closestDiff)
            {
                closestDiff = diff;
                closestIndex = i;
            }
        }

        ScrollTo(closestIndex);
        OnDragEnd?.Invoke(closestIndex);
    }


    private void CalculateContentWidth()
    {
        if(_itemTotalCount == 0)
        {
            _content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
            return;
        }

        float totalWidth = 0f;
        for (int i = 0; i < _itemTotalCount; i++)
        {
            totalWidth += _totalItemSizes[_currentItemIndexes[i]].x * _itemHeight;
        }

        totalWidth += _spacing * (_itemTotalCount - 1);

        float first = _totalItemSizes[_currentItemIndexes[0]].x * _itemHeight;
        float last = _totalItemSizes[_currentItemIndexes[_itemTotalCount - 1]].x * _itemHeight;

        float viewportWidth = _viewport.rect.width;
        _leftPadding = viewportWidth / 2f - first / 2f;
        _rightPadding = viewportWidth / 2f - last / 2f;

        totalWidth += _leftPadding + _rightPadding;

        _content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, totalWidth);
    }

    private void CalculateItemCenters()
    {
        _itemCenters = new float[_itemTotalCount];
        _itemScrollRatios = new float[_itemTotalCount];

        if (_itemTotalCount == 0) return;

        float viewportWidth = _viewport.rect.width;
        float contentWidth = _content.rect.width;

        float currentX = _leftPadding;

        for (int i = 0; i < _itemTotalCount; i++)
        {
            float itemWidth = _totalItemSizes[_currentItemIndexes[i]].x * _itemHeight;
            currentX += itemWidth / 2f;

            float center = currentX;
            _itemCenters[i] = center;
            _itemScrollRatios[i] = Mathf.Clamp01((center - viewportWidth / 2f) / (contentWidth - viewportWidth));

            currentX += itemWidth / 2f + _spacing;
        }
    }

    void AddLastItem()
    {
        IScrollItem item = GetItem(_currentItemIndexes[_lastItemIndex]);
        _itemList.AddLast(item);

        Vector2 localPos = new Vector2(_itemCenters[_lastItemIndex], 0);
        item.ChangeLocalPosition(localPos);

        _lastItemIndex += 1;
    }

    void AddFirstItem()
    {
        _firstItemIndex -= 1;

        IScrollItem item = GetItem(_currentItemIndexes[_firstItemIndex]);
        _itemList.AddFirst(item);

        Vector2 localPos = new Vector2(_itemCenters[_firstItemIndex], 0);
        item.ChangeLocalPosition(localPos);
    }

    void RemoveFirstItem()
    {
        IScrollItem item = _itemList.First.Value;
        item.ChangeLocalPosition(Vector2.zero);
        item.ReturnToPool();

        _itemList.RemoveFirst();
        _firstItemIndex += 1;
    }

    void RemoveLastItem()
    {
        IScrollItem item = _itemList.Last.Value;
        item.ChangeLocalPosition(Vector2.zero);
        item.ReturnToPool();

        _itemList.RemoveLast();
        _lastItemIndex -= 1;
    }

    public void ClearAllItems()
    {
        foreach (IScrollItem item in _itemList)
        {
            item.ReturnToPool();
        }
        _itemList.Clear();
        _firstItemIndex = 0;
        _lastItemIndex = 0;
    }

    bool _nowUpdateItemCount = false;

    IEnumerator UpdateItemCo(int centerIdx)
    {
        _nowUpdateItemCount = true;
        ClearAllItems();

        // 사이에 ui 업데이트를 위한 1프레임 대기 과정 필요
        yield return new WaitForEndOfFrame();

        InitializeSettings(); // 콘텐츠 너비 및 아이템 센터 재계산
        InitializeScrollItems(centerIdx);  // 다시 child를 풀에서 불러와 추가
        ScrollTo(centerIdx);

        _nowUpdateItemCount = false;
    }

    public void UpdateContent(List<int> currentItemIndexes, int centerIdx)
    {
        _currentItemIndexes = currentItemIndexes;
        _itemTotalCount = _currentItemIndexes.Count;

        StartCoroutine(UpdateItemCo(centerIdx));
    }

    public void UpdateContent(int centerIdx)
    {
        StartCoroutine(UpdateItemCo(centerIdx));
    }


    // 모든 아이템 비율을 넘겨받는다.
    // 이후 스크롤에 띄울 아이템 인덱스를 리스트로 넘겨받는다.
    // 해당 인덱스에 맞는 아이템의 너비를 바탕으로 콘텐츠들의 너비를 계산한다.
    public void Initialize(List<Vector2> totalItemSizes)
    {
        _scroll = _scrollRect.GetComponent<RectTransform>();
        _viewport = _scrollRect.viewport;
        _content = _scrollRect.content;

        _totalItemSizes = totalItemSizes;

        float halfSize = _viewport.rect.width / 2;

        _itemList = new LinkedList<IScrollItem>();
        _leftOffset = _leftPoint.anchoredPosition.x + halfSize;
        _rightOffset = _rightPoint.anchoredPosition.x - halfSize;
    }

    private void InitializeSettings()
    {
        CalculateContentWidth();
        CalculateItemCenters();
    }

    const int _scrollItemCount = 5;

    private void InitializeScrollItems(int centerIdx)
    {
        // 가운데에서 왼쪽으로 3칸 이동한 인덱스부터 시작
        int changedIdx = Mathf.Clamp(centerIdx - 3, 0, _itemTotalCount - 1); 
        // 따라서 범위는 (centerIdx - 3) ~  (_itemTotalCount - 1) 까지임

        // 실질적으로 생성될 수 있는 개수를 구해야함
        int maxSpawnableItemCount = (_itemTotalCount - 1) - changedIdx + 1; // 생성 가능한 아이템 개수
        int spawnableItemCount = Mathf.Min(maxSpawnableItemCount, _scrollItemCount); // 더 작은 개수로 결정

        _firstItemIndex = changedIdx;
        _lastItemIndex = changedIdx;

        // 아이템 생성
        for (int i = 0; i < spawnableItemCount; i++) AddLastItem();
    }

    // Update is called once per frame
    void Update()
    {
        if (_nowUpdateItemCount == true) return;
        if (_itemList.Count == 0) return;

        if (!_isDragging)
        {
            _scrollRect.horizontalScrollbar.value = Mathf.Lerp(_scrollRect.horizontalScrollbar.value, _targetPos, 0.15f);
        }

        LinkedListNode<IScrollItem> first = _itemList.First;
        LinkedListNode<IScrollItem> last = _itemList.Last;

        // 아이템의 X를 '뷰포트 로컬 좌표'로 계산:
        // content.anchoredPosition.y : content가 뷰포트에 대해 얼마나 움직였는지(로컬)
        // item.GetLocalPosition().y : item의 content 내부 로컬 Y
        float firstItemViewportX = _content.anchoredPosition.x + first.Value.GetLocalPosition().x;
        float lastItemViewportX = _content.anchoredPosition.x + last.Value.GetLocalPosition().x;

        // _dragDeltaX를 추가해서 이동 방향에 맞는 경우만 아이템 추가/제거
        if (_dragDeltaX < 0 && _leftOffset > firstItemViewportX && _lastItemIndex < _itemTotalCount)
        {
            RemoveFirstItem(); // 맨 앞의 아이템 제거
            AddLastItem();
        }
        else if (_dragDeltaX > 0 && _rightOffset + _viewport.rect.width < lastItemViewportX && _firstItemIndex > 0)
        {
            RemoveLastItem(); // 맨 뒤의 아이템 제거
            AddFirstItem();
        }
    }
}
