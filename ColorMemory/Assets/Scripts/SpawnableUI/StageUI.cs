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
    [SerializeField] Image _rankIconImg;
    [SerializeField] Image _selectBtnImg;

    [SerializeField] Button _selectBtn;

    public Action OnClickRequested;

    readonly Color _lockColor = new Color(236/255f, 232/255f, 232/255f);
    readonly Color _openColor = new Color(113/255f, 196/255f, 255/255f);

    //readonly Color _copperColor = new Color(208 / 255f, 148 / 255f, 107 / 255f);
    //readonly Color _silverColor = new Color(205 / 255f, 205 / 255f, 205 / 255f);
    //readonly Color _goleColor = new Color(249 / 255f, 210 / 255f, 56 / 255f);

    public enum State
    {
        Lock,
        Open,
        Clear
    }

    State _state;
    NetworkService.DTO.Rank _rank;

    public override void InjectClickEvent(System.Action OnClick)
    {
        this.OnClickRequested = OnClick;
    }

    public override void ChangeSelect(bool select) 
    {
        _outline.SetActive(select);
    }

    void ChangeColor(Color color)
    {
        _selectBtnImg.color = color;
    }

    void ChangeSprite(NetworkService.DTO.Rank rank)
    {
        _rankIconImg.gameObject.SetActive(true);
        _rankIconImg.sprite = _stageRankIconAssets[rank];
    }

    public override void SetRank(NetworkService.DTO.Rank rank)
    {
        _rank = rank;
        switch (_rank)
        {
            case NetworkService.DTO.Rank.NONE:
                break;
            default:
                ChangeSprite(_rank);
                break;
        }
    }

    public override void SetState(State state)
    {
        _state = state;
        switch (_state)
        {
            case State.Lock:
                ChangeColor(_lockColor);
                break;
            case State.Open:
                ChangeColor(_openColor);
                break;
            case State.Clear:
                break;
            default:
                break;
        }
    }

    Dictionary<NetworkService.DTO.Rank, Sprite> _stageRankIconAssets;

    public override void Initialize(Dictionary<NetworkService.DTO.Rank, Sprite> StageRankIconAssets)
    {
        _stageRankIconAssets = StageRankIconAssets;
        ChangeSelect(false);

        _selectBtn.onClick.AddListener(() => 
        {
            switch (_state)
            {
                case State.Lock:
                    break;
                case State.Open:
                    OnClickRequested?.Invoke();
                    break;
                case State.Clear:
                    OnClickRequested?.Invoke();
                    break;
                default:
                    break;
            }
        });
    } 
}
