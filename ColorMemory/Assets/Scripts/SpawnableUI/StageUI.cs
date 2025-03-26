using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StageUI : SpawnableUI
{
    const int maxSize = 5;
    [SerializeField] Transform _content;
    [SerializeField] GameObject _outline;
    [SerializeField] Image _coverImg;
    [SerializeField] Button _selectBtn;

    public Action<Vector2Int> OnClickRequested;

    readonly Color _lockColor = new Color(43/255f, 43/255f, 43/255f);
    readonly Color _openColor = new Color(255/255f, 255/255f, 255/255f);

    GameObject[,] _dots;

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

    public override void ChangeIndex(Vector2Int index)
    {
        _index = index;
    }

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
                _coverImg.gameObject.SetActive(true);
                _coverImg.color = _lockColor;

                break;
            case State.Open:
                _coverImg.gameObject.SetActive(true);
                _coverImg.color = _openColor;

                break;
            case State.Clear:
                _coverImg.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    public override void Initialize(
        List<List<CollectiveArtData.Block>> blocks, 
        List<List<CollectiveArtData.Color>> usedColors)
    {
        ChangeSelect(false);

        _dots = new GameObject[maxSize, maxSize];
        for (int i = 0; i < maxSize * maxSize; i++)
        {
            Transform child = _content.GetChild(i);
            int maxCol = i % maxSize;
            int maxRow = i / maxSize;
            _dots[maxRow, maxCol] = child.gameObject;
            child.gameObject.SetActive(false);
        }

        int row = blocks.Count;
        int height = blocks[0].Count;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < height; j++)
            {
                _dots[i, j].gameObject.SetActive(true);
                _dots[i, j].GetComponent<Image>().color = blocks[i][j].Color.GetColor();
            }
        }

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
