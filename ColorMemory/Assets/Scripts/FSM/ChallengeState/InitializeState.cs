using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using static Challenge.ChallengeMode;

namespace Challenge
{
    public class InitializeState : BaseState<ChallengeMode.State>
    {
        // 스테이지 생성 후 다른 State로 레벨 데이터 뿌리기
        //Vector2Int _levelSize;

        Color[] _pickColors;
        //int _pickCount;

        EffectFactory _effectFactory;
        DotFactory _dotFactory;

        GridLayoutGroup _dotGridContent;
        RectTransform _penContent;
        ToggleGroup _penToggleGroup;

        ModeData _modeData;

        Action<Dot[,], Dot[], MapData> SetStage;

        RandomLevelGenerator _randomLevelGenerator;
        ChallengeStageUIPresenter _challengeStageUIPresenter;

        public InitializeState(
            FSM<ChallengeMode.State> fsm,

            Color[] pickColors,

            EffectFactory effectFactory,
            DotFactory dotFactory,

            GridLayoutGroup grid,
            RectTransform penContent,
            ToggleGroup penToggleGroup,

            RandomLevelGenerator randomLevelGenerator,
            ChallengeStageUIPresenter challengeStageUIPresenter,

            ModeData modeData,
            Action<Dot[,], Dot[], MapData> SetStage
        ) : base(fsm)
        {
            _pickColors = pickColors;
            //_pickCount = pickCount;
            //_levelSize = levelSize;

            _effectFactory = effectFactory;
            _dotFactory = dotFactory;

            _dotGridContent = grid;
            _penContent = penContent;
            _penToggleGroup = penToggleGroup;

            _randomLevelGenerator = randomLevelGenerator;
            _challengeStageUIPresenter = challengeStageUIPresenter;

            _modeData = modeData;
            this.SetStage = SetStage;
        }

        public override void OnStateEnter()
        {
            _challengeStageUIPresenter.ActivatePlayPanel(true);
            CreateLevel();
            _fsm.SetState(ChallengeMode.State.Memorize);
        }

        void CreateLevel()
        {
            if (_randomLevelGenerator.CanGenerateLevelData() == false) return;

            MapData mapData = _randomLevelGenerator.GenerateLevelData(_modeData.StageCount);

            int row = mapData.DotColor.GetLength(0);
            int col = mapData.DotColor.GetLength(1);

            _dotGridContent.constraintCount = row;

            Dot[,] dots = new Dot[row, col];

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    Dot dot = _dotFactory.Create(Dot.Name.Basic);

                    dot.Inject(_effectFactory, new Vector2Int(i, j), (index) => { _fsm.OnClickDot(index); });
                    dot.transform.SetParent(_dotGridContent.transform);
                    dots[i, j] = dot;
                }
            }

            Dot[] colorPenDots = new Dot[mapData.PickColors.Count];

            for (int i = 0; i < mapData.PickColors.Count; i++)
            {
                Dot colorPenDot = _dotFactory.Create(Dot.Name.ColorPen);
                colorPenDot.ChangeColor(_pickColors[mapData.PickColors[i]]);

                colorPenDot.Inject(_effectFactory, _penToggleGroup, mapData.PickColors[i], (index) => { _fsm.OnClickDot(index); });
                colorPenDot.transform.SetParent(_penContent);

                colorPenDot.Minimize();
                colorPenDots[i] = colorPenDot;
            }

            _modeData.StageData.Add(mapData);
            _modeData.StageCount += 1;

            SetStage?.Invoke(dots, colorPenDots, mapData);
            _challengeStageUIPresenter.ChangeStageCount(_modeData.StageCount);
        }
    }
}