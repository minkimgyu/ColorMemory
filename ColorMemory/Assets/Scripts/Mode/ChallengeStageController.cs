using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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


    [SerializeField] Button _randomHintBtn;
    [SerializeField] Button _revealSameColorHintBtn;

    [SerializeField] Image _timerSlider;
    [SerializeField] TMP_Text _timeText;
    [SerializeField] TMP_Text _titleText;
    [SerializeField] GameObject _hintPanel;

    [SerializeField] GameObject _endPanel;
    [SerializeField] Button _tryAgainBtn;
    [SerializeField] Button _exitBtn;

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
        _challengeStageUIController.Initialize(_timerSlider, _timeText, _titleText, _hintPanel, _endPanel);

        _randomHintBtn.onClick.AddListener(() => { _fsm.OnClickRandomFillHint(); });
        _revealSameColorHintBtn.onClick.AddListener(() => { _fsm.OnClickRevealSameColorHint(); });

        _tryAgainBtn.onClick.AddListener(() => { SceneManager.LoadScene("ChallengeScene"); });
        _exitBtn.onClick.AddListener(() => { SceneManager.LoadScene("StartScene"); });

        _fsm = new FSM<State>();
        Dictionary<State, BaseState<State>> states = new Dictionary<State, BaseState<State>>()
        {
            { 
                State.Init, 
                new InitState
                (
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
                    SetLevelData
                ) 
            },

            { State.Memorize, new MemorizeState(_fsm, _pickColors, 7f, _challengeStageUIController, GetLevelData) },
            { State.Paint, new PaintState(_fsm, _pickColors, 10f, _challengeStageUIController, GetLevelData) },
            { State.Clear, new ClearState(_fsm, _challengeStageUIController, GetLevelData, DestroyDots) },
            { State.End, new EndState(_fsm, _challengeStageUIController) }
        };

        _fsm.Initialize(states, State.Init);
    }
}
