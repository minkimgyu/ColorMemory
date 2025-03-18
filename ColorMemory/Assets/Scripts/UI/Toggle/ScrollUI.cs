using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] Scrollbar _scrollbar;
    [SerializeField] protected Transform _content;

    protected int _menuSize;

    protected float[] _points;
    protected float _distance, _currentPos, _targetPos;
    protected bool _isDrag;

    protected int _targetIndex;

    public System.Action<int> OnDragEnd { get; set; }

    protected void SetUp(int menuCount, int startIndex = 0)
    {
        _menuSize = menuCount;
        _points = new float[_menuSize];

        // 거리에 따라 0~1인 pos대입
        _distance = 1f / (_menuSize - 1);
        for (int i = 0; i < _menuSize; i++)
        {
            _points[i] = _distance * i;
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

        // 절반거리를 넘지 않아도 마우스를 빠르게 이동하면
        if (_currentPos == _targetPos)
        {
            // ← 으로 가려면 목표가 하나 감소
            if (eventData.delta.x > 18 && _currentPos - _distance >= 0)
            {
                --_targetIndex;
                _targetPos = _currentPos - _distance;
            }

            // → 으로 가려면 목표가 하나 증가
            else if (eventData.delta.x < -18 && _currentPos + _distance <= 1.01f)
            {
                ++_targetIndex;
                _targetPos = _currentPos + _distance;
            }
        }

        OnDragEnd?.Invoke(_targetIndex);
    }

    protected float GetPos()
    {
        // 절반거리를 기준으로 가까운 위치를 반환
        for (int i = 0; i < _menuSize; i++)
        {
            if (_scrollbar.value < _points[i] + _distance * 0.5f && _scrollbar.value > _points[i] - _distance * 0.5f)
            {
                _targetIndex = i;
                return _points[i];
            }
        }

        return 0;
    }
}
