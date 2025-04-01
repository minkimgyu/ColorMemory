using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

abstract public class ScrollUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] Scrollbar _scrollbar;
    [SerializeField] protected Transform _content;

    public int ItemCount { get { return _content.childCount; } }

    protected int _menuSize;
    protected float[] _points;
    protected float _distance, _currentPos, _targetPos;
    bool _isDrag;

    const int _layoutGroupSpacing = 200;
    [SerializeField] protected int _targetIndex;
    [SerializeField] bool _canLoop = false;

    Timer _scaleChangeTimer;

    public System.Action<int> OnDragEnd { get; set; }

    protected bool _isHorizontal = true;

    public virtual void SetUp(int menuCount, int startIndex = 0)
    {
        _scaleChangeTimer = new Timer();

        _menuSize = menuCount;
        _points = new float[_menuSize];

        // �Ÿ��� ���� 0~1�� pos����
        _distance = 1f / (_menuSize - 1);
        for (int i = 0; i < _menuSize; i++)
        {
            _points[i] = _distance * i;
        }

        _targetPos = _points[startIndex];
    }

    public virtual void AddItem(Transform item, bool setToMiddle)
    {
        item.SetParent(_content);
        if (setToMiddle) item.SetSiblingIndex(ItemCount / 2);
    }

    public virtual void AddItem(Transform item)
    {
        item.SetParent(_content);
    }

    public virtual void DestroyItems()
    {
        for (int i = _content.childCount - 1; i >= 0; i--)
        {
            _content.GetChild(i).GetComponent<SpawnableUI>().DestroyObject();
        }
    }

    protected virtual void Update()
    {
        if (!_isDrag)
        {
            _scrollbar.value = Mathf.Lerp(_scrollbar.value, _targetPos, 0.1f);
        }

        if (_canLoop && !_isDrag && ItemCount > 1)
        {
            if (_isHorizontal)
            {
                if (_scrollbar.value <= 0f)
                {
                    MoveLastItemToFront();
                    _scrollbar.value = _points[_menuSize - 1] - _distance;
                    _targetPos = _scrollbar.value;
                    _targetIndex = _menuSize - 1;
                }
                else if (_scrollbar.value >= 1f)
                {
                    MoveFirstItemToEnd();
                    _scrollbar.value = _points[0] + _distance;
                    _targetPos = _scrollbar.value;
                    _targetIndex = 0;
                }
            }
            else
            {
                if (_scrollbar.value <= 0f)
                {
                    MoveLastItemToFront();
                    _scrollbar.value = _points[_menuSize - 1] - _distance;
                    _targetPos = _scrollbar.value;
                    _targetIndex = _menuSize - 1;
                }
                else if (_scrollbar.value >= 1f)
                {
                    MoveFirstItemToEnd();
                    _scrollbar.value = _points[0] + _distance;
                    _targetPos = _scrollbar.value;
                    _targetIndex = 0;
                }
            }
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

        Debug.Log(eventData.delta);

        // ���ݰŸ��� ���� �ʾƵ� ���콺�� ������ �̵��ϸ�
        if (_currentPos == _targetPos)
        {
            if(_isHorizontal)
            {
                if(_canLoop == false)
                {
                    // �� ���� ������ ��ǥ�� �ϳ� ����
                    if (eventData.delta.x > 18 && _currentPos - _distance >= 0)
                    {
                        --_targetIndex;
                        _targetPos = _currentPos - _distance;
                    }
                    // �� ���� ������ ��ǥ�� �ϳ� ����
                    else if (eventData.delta.x < -18 && _currentPos + _distance <= 1.01f)
                    {
                        ++_targetIndex;
                        _targetPos = _currentPos + _distance;
                    }
                }
            }
            else
            {
                // �� ���� ������ ��ǥ�� �ϳ� ����
                if (eventData.delta.y > 18 && _currentPos - _distance >= 0)
                {
                    ++_targetIndex;
                    _targetPos = _currentPos + _distance;
                }
                // �� ���� ������ ��ǥ�� �ϳ� ����
                else if (eventData.delta.y < -18 && _currentPos + _distance <= 1.01f)
                {
                    --_targetIndex;
                    _targetPos = _currentPos - _distance;
                }
            }
        }



        OnDragEnd?.Invoke(_targetIndex);

        _scaleChangeTimer.Reset();
        _scaleChangeTimer.Start(1f);



        if (_canLoop)
        {
            if (_isHorizontal)
            {
                if (eventData.delta.x > 18) // Dragged Left (towards end)
                {
                    if (_scrollbar.value <= 0f)
                    {
                        MoveLastItemToFront();
                        //_scrollbar.value = _points[_menuSize - 1] - _distance; // Adjust scrollbar value
                        _targetPos = _scrollbar.value;
                        _targetIndex = _menuSize - 1;
                    }
                }
                else if (eventData.delta.x < -18) // Dragged Right (towards start)
                {
                    if (_scrollbar.value >= 1f)
                    {
                        MoveFirstItemToEnd();
                        //_scrollbar.value = _points[0] + _distance; // Adjust scrollbar value
                        _targetPos = _scrollbar.value;
                        _targetIndex = 0;
                    }
                }
            }
            else
            {
                if (eventData.delta.y > 18) // Dragged Up (towards end - in Scrollbar value)
                {
                    if (_scrollbar.value >= 1f)
                    {
                        // Vertical scrollbar value increases upwards, so logic is reversed
                        MoveFirstItemToEnd();
                        //_scrollbar.value = _points[0] + _distance;
                        _targetPos = _scrollbar.value;
                        _targetIndex = 0;
                    }
                }
                else if (eventData.delta.y < -18) // Dragged Down (towards start - in Scrollbar value)
                {
                    if (_scrollbar.value <= 0f)
                    {
                        // Vertical scrollbar value increases upwards, so logic is reversed
                        MoveLastItemToFront();
                        //_scrollbar.value = _points[_menuSize - 1] - _distance;
                        _targetPos = _scrollbar.value;
                        _targetIndex = _menuSize - 1;
                    }
                }
            }
        }
    }

    private void MoveFirstItemToEnd()
    {
        if (_content.childCount == 0) return;

        Transform firstItem = _content.GetChild(0);
        firstItem.SetSiblingIndex(_content.childCount - 1);

        // ��ũ�� �� ���� (�ڿ������� �̾�������)
        //_scrollbar.value += _distance;
    }

    private void MoveLastItemToFront()
    {
        if (_content.childCount == 0) return;

        Transform lastItem = _content.GetChild(_content.childCount - 1);
        lastItem.SetSiblingIndex(0);

        // ��ũ�� �� ���� (�ڿ������� �̾�������)
        //_scrollbar.value -= _distance;
    }

    protected float GetPos()
    {
        // ���ݰŸ��� �������� ����� ��ġ�� ��ȯ
        for (int i = 0; i < _menuSize; i++)
        {
            if (_isHorizontal)
            {
                if (_scrollbar.value < _points[i] + _distance * 0.5f && _scrollbar.value > _points[i] - _distance * 0.5f)
                {
                    _targetIndex = i;
                    return _points[i];
                }
            }
            else
            {
                if (_scrollbar.value < _points[i] + _distance * 0.5f && _scrollbar.value > _points[i] - _distance * 0.5f)
                {
                    _targetIndex = (_menuSize - 1 - i);
                    return _points[i];
                }
            }
        }

        return 0;
    }
}
