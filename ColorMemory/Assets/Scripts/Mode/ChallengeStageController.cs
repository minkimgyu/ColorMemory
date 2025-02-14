using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeStageController : StageController
{
    [SerializeField] EffectPrefabDictionary _effectPrefab;
    [SerializeField] DotPrefabDictionary _dotPrefab;

    [SerializeField] ToggleGroup _penToggleGroup;
    [SerializeField] RectTransform _penContent;
    [SerializeField] GridLayoutGroup _dotGridContent;

    [SerializeField] Color[] _pickColors;
    [SerializeField] Vector2 _spacing;
    [SerializeField] int _pickCount;
    [SerializeField] Vector2Int _levelSize = new Vector2Int(5, 5); // row, col

    MapData _mapData;
    Dot[,] _dots;
    Dot[] _colorPenDots;

    ChallengeStageUIController _challengeStageUIController;

    void DestroyDots()
    {
        for (int i = 0; i < _levelSize.x; i++)
        {
            for (int j = 0; j < _levelSize.y; j++)
            {
                Destroy(_dots[i, j].gameObject);
            }
        }

        for (int i = 0; i < _colorPenDots.Length; i++)
        {
            Destroy(_colorPenDots[i].gameObject);
        }

        _dots = null;
    }

    void SetLevelData(Dot[,] dots, Dot[] colorPenDots, MapData mapData)
    {
        _dots = dots;
        _colorPenDots = colorPenDots;
        _mapData = mapData;
    }

    Tuple<Dot[,], Dot[], MapData> GetLevelData() 
    {
        return new Tuple<Dot[,], Dot[], MapData>(_dots, _colorPenDots, _mapData); 
    }

    // state는 여기에 추가
    public enum State
    {
        Init,
        Memorize,
        Paint,
        Clear,
        End,
    }

    FSM<State> _fsm;

    private void Update()
    {
        _fsm.OnUpdate();
    }

    public override void Initialize()
    {
        _challengeStageUIController  = GetComponent<ChallengeStageUIController>();
        _challengeStageUIController.Initialize();

        _fsm = new FSM<State>();
        Dictionary<State, BaseState<State>> states = new Dictionary<State, BaseState<State>>()
        {
            { 
                State.Init, new InitState(
                _fsm, 
                _pickColors, 
                _pickCount,
                _levelSize, 
                new EffectFactory(_effectPrefab), 
                new DotFactory(_dotPrefab), 
                _dotGridContent, 
                _penContent,
                _penToggleGroup,
                _challengeStageUIController,
                SetLevelData) 
            },

            { State.Memorize, new MemorizeState(_fsm, _pickColors, 5f, _challengeStageUIController, GetLevelData) },
            { State.Paint, new PaintState(_fsm, _pickColors, 5f, _challengeStageUIController, GetLevelData) },
            { State.Clear, new ClearState(_fsm, DestroyDots) }
        };

        _fsm.Initialize(states, State.Init);
    }
}
