using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;

public class StageUI : SpawnableUI
{
    const int maxSize = 5;
    [SerializeField] GameObject _outline;
    [SerializeField] Image _outlineImg;

    [SerializeField] Image _selectBtnImg;
    [SerializeField] Button _selectBtn;

    public Action<Vector2Int> OnClickRequested;

    readonly Color _lockColor = new Color(236/255f, 232/255f, 232/255f);
    readonly Color _openColor = new Color(113/255f, 196/255f, 255/255f);

    public enum State
    {
        Lock,
        Open,
        Clear
    }

    State _state;

    public override void InjectClickEvent(System.Action<Vector2Int> OnClick)
    {
        this.OnClickRequested = OnClick;
    }

    Vector2Int _index;
    public Vector2Int Index { get => _index; }

    public override void ChangeSelect(bool select) 
    {
        _outline.SetActive(select);
    }

    public override void SetState(State state)
    {
        _state = state;
        switch (_state)
        {
            case State.Lock:
                _outlineImg.color = _lockColor;
                _selectBtnImg.color = _lockColor;

                break;
            case State.Open:
                _outlineImg.color = _openColor;
                _selectBtnImg.color = _openColor;

                break;
            case State.Clear:
                break;
            default:
                break;
        }
    }

    public override void Initialize(Vector2Int index)
    {
        _index = index;
        ChangeSelect(false);

        _selectBtn.onClick.AddListener(() => 
        {
            switch (_state)
            {
                case State.Lock:
                    break;
                case State.Open:
                    OnClickRequested?.Invoke(_index);
                    break;
                case State.Clear:
                    OnClickRequested?.Invoke(_index);
                    break;
                default:
                    break;
            }
        });
    } 
}
