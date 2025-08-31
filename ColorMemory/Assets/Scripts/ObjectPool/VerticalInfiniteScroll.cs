using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VerticalInfiniteScroll : MonoBehaviour
{
    [SerializeField] ScrollRect _scrollRect;
    RectTransform _scroll;
    RectTransform _viewport;
    RectTransform _content;

    [SerializeField] RectTransform _upPoint;
    [SerializeField] RectTransform _downPoint;
    //[SerializeField] GameObject _itemPrefab;

    int _firstItemIndex = 0; // 첫 아이템 인덱스
    int _lastItemIndex = 0; // 첫 아이템 인덱스

    LinkedList<IScrollItem> _itemList;

    public event System.Func<int, IScrollItem> GetItem;

    List<int> _currentItemIndexes = new List<int>();

    float _upOffset;
    float _downOffset;

    [SerializeField] Vector2 _itemSize;
    int _itemTotalCount = 100;
    [SerializeField] float _contentSpace = 80; // 컨텐츠 사이의 거리

    [SerializeField] bool _autoFit = false; // 자동 핏
    [SerializeField] float _minLeftRightPadding = 20; // 최소 패딩 크기
    // 컨텐츠 사이의 거리를 기반으로 _columnSpace을 자동으로 계산한다.
    // 최소 패딩 연산을 추가한다.

    float CalculateWidth(int itemCount)
    {
        return (_itemSize.x * itemCount) + (_contentSpace * Mathf.Clamp((itemCount - 1), 0, (itemCount - 1)));
    }

    void CalculateAutoFit()
    {
        int itemCount = 0;

        while (_content.rect.width > CalculateWidth(itemCount))
        {
            itemCount++;
        }

        itemCount--;
        float realSize = CalculateWidth(itemCount);
        _leftRightPadding = (_content.rect.width - realSize) / 2;

        while (_leftRightPadding < _minLeftRightPadding && itemCount > 0) // 최소 패딩보다 패딩 크기가 작다면 아이템 하나 더 빼주기
        {
            itemCount--;
            realSize = (_itemSize.x * itemCount) + (_contentSpace * (itemCount - 1));
            _leftRightPadding = (_content.rect.width - realSize) / 2;
        }

        //_upDownPadding = _leftRightPadding;
        _rowColumnCount = itemCount;
    }

    [SerializeField] int _rowColumnCount = 3; // 한 줄에 3개씩 배치
    [SerializeField] float _leftRightPadding = 30; // 행 사이 거리
    [SerializeField] float _upDownPadding = 20f; // 열 사이 거리

    //int[] itemRanks;

    float GetItemSpawnPosY(int rowIndex)
    {
        return _upDownPadding + (_itemSize.y * rowIndex) + (_contentSpace * rowIndex) + _itemSize.y / 2;
    }

    float GetItemSpawnPosX(int columnIndex)
    {
        return _leftRightPadding + (_itemSize.x / 2) + (_itemSize.x + _contentSpace) * columnIndex;
    }

    const float _heightOffset = 450f; // 콘텐츠 높이 오프셋

    float GetContentHeight()
    {
        int totalRows = Mathf.CeilToInt((float)_itemTotalCount / _rowColumnCount);
        return _upDownPadding * 2 + (_contentSpace * Mathf.Clamp(totalRows - 1, 0, totalRows - 1)) + totalRows * _itemSize.y + _heightOffset;
    }

    //// Start is called before the first frame update
    //void Start()
    //{
    //    Initialize();
    //}

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

    IEnumerator UpdateItemCo()
    {
        _nowUpdateItemCount = true;

        foreach (IScrollItem item in _itemList)
        {
            item.ReturnToPool();
        }

        // 사이에 ui 업데이트를 위한 1프레임 대기 과정 필요
        yield return new WaitForEndOfFrame();

        InitializeSettings(); // 콘텐츠 너비 및 아이템 센터 재계산
        InitializeScrollItems(); // 다시 child를 풀에서 불러와 추가

        _nowUpdateItemCount = false;
    }

    public void UpdateContent(List<int> currentItemIndexes)
    {
        _currentItemIndexes = currentItemIndexes;
        _itemTotalCount = currentItemIndexes.Count;

        StartCoroutine(UpdateItemCo());
    }

    private void InitializeSettings()
    {
        if (_autoFit == true) CalculateAutoFit();
        ChangeContentHeight();
    }

    private void ChangeContentHeight()
    {
        _content.sizeDelta = new Vector2(_content.rect.width, GetContentHeight());
    }

    void AddLastItem(Vector2 localPos)
    {
        IScrollItem item;

        // 여기 Factory를 통해 아이템을 생성해야함
        // 인덱스를 넘어간다면 가장 마지막 아이템을 생성해서 준다.

        int spawnIdx = Mathf.Clamp(_lastItemIndex, 0, _itemTotalCount - 1);
        item = GetItem(_currentItemIndexes[spawnIdx]);

        _itemList.AddLast(item);
        item.ChangeLocalPosition(localPos);

        if (_lastItemIndex > _itemTotalCount - 1) item.Active(false);
        _lastItemIndex += 1;
    }

    void AddFirstItem(Vector2 localPos)
    {
        _firstItemIndex -= 1;

        IScrollItem item = GetItem(_currentItemIndexes[_firstItemIndex]);
        _itemList.AddFirst(item);
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

    private void InitializeScrollItems()
    {
        _firstItemIndex = 0;
        _lastItemIndex = 0;

        int itemCount = 0;
        int row = 0;
        int column = 0;

        while (_scroll.rect.height > GetItemSpawnPosY(row) && itemCount < _itemTotalCount)
        {
            float x = GetItemSpawnPosX(column);
            float y = -GetItemSpawnPosY(row);
            AddLastItem(new Vector2(x, y));

            column++;
            if (column >= _rowColumnCount)
            {
                column = 0;
                row++;
            }

            itemCount++;
        }
    }

    // 가로로 들어갈 수 있는 아이템 개수 측정
    public void Initialize()
    {
        _scroll = _scrollRect.GetComponent<RectTransform>();
        _viewport = _scrollRect.viewport;
        _content = _scrollRect.content;

        _itemList = new LinkedList<IScrollItem>();

        _content.sizeDelta = new Vector2(_viewport.rect.width, _viewport.rect.height); // 첫 사이즈 맞춰주기
    }

    const float _upPointOffset = 50f; // 체크 오프셋
    const float _downPointOffset = 20f; // 체크 오프셋

    // Update is called once per frame
    void Update()
    {
        if (_nowUpdateItemCount == true) return;
        if (_itemList.Count == 0) return;

        LinkedListNode<IScrollItem> first = _itemList.First;
        LinkedListNode<IScrollItem> last = _itemList.Last;

        // 피벗이 뷰포트 상단임을 기억하자

        // 상단으로 빠져나가는지 체크 (first가 뷰포트 상단 기준보다 위로 올라가면 더 아래에 아이템을 추가)
        float topThreshold = _upPointOffset;     // 예: 화면 상단보다 조금 위
        float bottomThreshold = -_viewport.rect.height - _downPointOffset; // 예: 화면 하단보다 조금 아래

        // 아이템의 Y를 '뷰포트 로컬 좌표'로 계산:
        // content.anchoredPosition.y : content가 뷰포트에 대해 얼마나 움직였는지(로컬)
        // item.GetLocalPosition().y : item의 content 내부 로컬 Y
        float firstItemViewportY = _content.anchoredPosition.y + first.Value.GetLocalPosition().y;
        float lastItemViewportY = _content.anchoredPosition.y + last.Value.GetLocalPosition().y;

        if (topThreshold < firstItemViewportY && _lastItemIndex < _itemTotalCount)
        {
            float lastYPos = last.Value.GetLocalPosition().y;

            for (int i = 0; i < _rowColumnCount && first != null; i++)
            {
                float xPos = first.Value.GetLocalPosition().x;

                first = first.Next; // 다음 아이템으로 이동

                RemoveFirstItem(); // 맨 앞의 아이템 제거
                AddLastItem(new Vector2(xPos, lastYPos - _itemSize.y - _contentSpace));
            }
        }
        else if (bottomThreshold > lastItemViewportY && _firstItemIndex > 0)
        {
            float firstYPos = first.Value.GetLocalPosition().y;

            for (int i = 1; i <= _rowColumnCount && last != null; i++)
            {
                float xPos = last.Value.GetLocalPosition().x;

                last = last.Previous; // 이전 아이템으로 이동

                RemoveLastItem(); // 맨 뒤의 아이템 제거
                AddFirstItem(new Vector2(xPos, firstYPos + _itemSize.y + _contentSpace));
            }
        }
    }
}
