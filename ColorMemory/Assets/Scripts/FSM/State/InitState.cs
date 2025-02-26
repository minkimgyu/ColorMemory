using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class InitState : BaseState<ChallengeStageController.State>
{
    // 스테이지 생성 후 다른 State로 레벨 데이터 뿌리기
    Vector2Int _levelSize;

    Color[] _pickColors;
    int _pickCount;

    EffectFactory _effectFactory;
    DotFactory _dotFactory;

    GridLayoutGroup _dotGridContent;
    RectTransform _penContent;
    ToggleGroup _penToggleGroup;

    Action<Dot[,], Dot[], MapData> SetLevelData;

    ChallengeStageUIController _challengeStageUIController;
    int _level = 0;

    public InitState(
        FSM<ChallengeStageController.State> fsm, 

        Color[] pickColors, 
        int pickCount,
        Vector2Int levelSize,

        EffectFactory effectFactory, 
        DotFactory dotFactory,

        GridLayoutGroup grid,
        RectTransform penContent,
        ToggleGroup penToggleGroup,
        ChallengeStageUIController challengeStageUIController,

        Action<Dot[,], Dot[], MapData> SetLevelData
    ) : base(fsm)
    {
        _pickColors = pickColors;
        _pickCount = pickCount;
        _levelSize = levelSize;

        _effectFactory = effectFactory;
        _dotFactory = dotFactory;

        _dotGridContent = grid;
        _penContent = penContent;
        _penToggleGroup = penToggleGroup;

        _challengeStageUIController = challengeStageUIController;
        this.SetLevelData = SetLevelData;
    }

    public override void OnStateEnter()
    {
        _challengeStageUIController.ChangeTitle($"LEVEL {++_level}");
        CreateLevel();
        _fsm.SetState(ChallengeStageController.State.Memorize);
    }

    void CreateLevel()
    {
        RandomLevelGenerator randomLevelGenerator = new RandomLevelGenerator(_pickColors.Length, _pickCount, _levelSize);
        if (randomLevelGenerator.CanGenerateLevelData() == false) return;

        MapData mapData = randomLevelGenerator.GenerateLevelData();
        Dot[,] dots = new Dot[_levelSize.x, _levelSize.y];

        for (int i = 0; i < _levelSize.x; i++)
        {
            for (int j = 0; j < _levelSize.y; j++)
            {
                Dot dot = _dotFactory.Create(Dot.Name.Basic);

                dot.Inject(_effectFactory, new Vector2Int(i, j), (index) => { _fsm.OnClickDot(index); });
                dot.transform.SetParent(_dotGridContent.transform);

                dots[i, j] = dot;
            }
        }

        Dot[] colorPenDots = new Dot[_pickCount];

        for (int i = 0; i < _pickCount; i++)
        {
            Dot colorPenDot = _dotFactory.Create(Dot.Name.ColorPen);
            colorPenDot.ChangeColor(_pickColors[mapData.PickColors[i]]);

            colorPenDot.Inject(_effectFactory, _penToggleGroup, mapData.PickColors[i], (index) => { _fsm.OnClickDot(index); });
            colorPenDot.transform.SetParent(_penContent);

            colorPenDot.Minimize();
            colorPenDots[i] = colorPenDot;
        }

        SetLevelData?.Invoke(dots, colorPenDots, mapData);
    }
}
