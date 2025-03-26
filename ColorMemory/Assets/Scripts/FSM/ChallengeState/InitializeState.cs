using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using static Challenge.ChallengeMode;

namespace Challenge
{
    public class InitializeState : BaseState<ChallengeMode.State>
    {
        // �������� ���� �� �ٸ� State�� ���� ������ �Ѹ���
        Vector2Int _levelSize;

        Color[] _pickColors;
        int _pickCount;

        EffectFactory _effectFactory;
        DotFactory _dotFactory;

        GridLayoutGroup _dotGridContent;
        RectTransform _penContent;
        ToggleGroup _penToggleGroup;

        ModeData _modeData;

        Action<Dot[,], Dot[], MapData> SetStage;

        ChallengeStageUIPresenter _challengeStageUIPresenter;

        public InitializeState(
            FSM<ChallengeMode.State> fsm,

            Color[] pickColors,
            int pickCount,
            Vector2Int levelSize,

            EffectFactory effectFactory,
            DotFactory dotFactory,

            GridLayoutGroup grid,
            RectTransform penContent,
            ToggleGroup penToggleGroup,
            ChallengeStageUIPresenter challengeStageUIPresenter,

            ModeData modeData,
            Action<Dot[,], Dot[], MapData> SetStage
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

            _challengeStageUIPresenter = challengeStageUIPresenter;

            _modeData = modeData;
            this.SetStage = SetStage;
        }

        public override void OnStateEnter()
        {
            //_challengeStageUIPresenter.ChangeTitle($"LEVEL {++_level}");
            CreateLevel();
            _fsm.SetState(ChallengeMode.State.Memorize);
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

            _modeData.StageCount += 1;

            SetStage?.Invoke(dots, colorPenDots, mapData);
            _challengeStageUIPresenter.ChangeStageCount(_modeData.StageCount);
        }
    }
}