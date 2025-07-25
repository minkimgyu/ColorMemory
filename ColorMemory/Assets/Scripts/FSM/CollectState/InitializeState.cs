using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using System.Collections.Generic;

namespace Collect
{
    public class InitializeState : BaseState<CollectMode.State>
    {
        // 스테이지 생성 후 다른 State로 레벨 데이터 뿌리기

        CollectArtData _artData;
        CollectMode.Data _modeData;

        EffectFactory _effectFactory;
        DotFactory _dotFactory;

        GridLayoutGroup _dotGridContent;
        RectTransform _penContent;
        ToggleGroup _penToggleGroup;

        Action<Dot[,], Dot[], MapData> SetLevelData;
        CollectStageUIPresenter _collectStageUIPresenter;

        public InitializeState(
            FSM<CollectMode.State> fsm,

            CollectMode.Data modeData,

            EffectFactory effectFactory,
            DotFactory dotFactory,

            GridLayoutGroup grid,
            RectTransform penContent,
            ToggleGroup penToggleGroup,

            CollectArtData artData,
            CollectStageUIPresenter collectStageUIPresenter,

            Action<Dot[,], Dot[], MapData> SetLevelData
        ) : base(fsm)
        {
            _modeData = modeData;

            _effectFactory = effectFactory;
            _dotFactory = dotFactory;

            _dotGridContent = grid;
            _penContent = penContent;
            _penToggleGroup = penToggleGroup;

            _artData = artData;
            _collectStageUIPresenter = collectStageUIPresenter;
            this.SetLevelData = SetLevelData;
        }

        public override void OnStateEnter()
        {
            _collectStageUIPresenter.ActivatePlayPanel(true);

            SaveData saveData = ServiceLocater.ReturnSaveManager().GetSaveData();

            float progress = saveData.SelectedArtworkProgress;
            _collectStageUIPresenter.ChangeProgressText((int)(progress * 100));
            _collectStageUIPresenter.ChangeHintInfoText("힌트를 사용할수록 높은 랭크를 받을 확률이 떨어져요!");

            CreateLevel(saveData.SelectedArtworkSectionIntIndex, saveData.TotalArtworkSectionSize, saveData.SelectedArtworkSectionIndex);
            _fsm.SetState(CollectMode.State.Memorize);
        }

        void CreateLevel(int sectionIntIndex, int totalSectionSize, Vector2Int sectionIndex)
        {
            int colorCount = _artData.Sections[sectionIndex.x][sectionIndex.y].UsedColors[0].Count;
            _modeData.PickColors = new Color[colorCount];

            // 진입 전에 초기화
            _modeData.GoBackCount = 0;
            _modeData.WrongCount = 0;

            for (int i = 0; i < colorCount; i++)
            {
                _modeData.PickColors[i] = _artData.Sections[sectionIndex.x][sectionIndex.y].UsedColors[0][i].GetColor();
            }

            CustomLevelGenerator customLevelGenerator = new CustomLevelGenerator(_artData.Sections[sectionIndex.x][sectionIndex.y]);
            if (customLevelGenerator.CanGenerateLevelData() == false) return;

            MapData mapData = customLevelGenerator.GenerateLevelData();

            _collectStageUIPresenter.ChangeTitle(_modeData.Title, sectionIntIndex + 1, totalSectionSize);

            int row = mapData.DotColor.GetLength(0);
            int col = mapData.DotColor.GetLength(1);

            Dot[,] dots = new Dot[row, col];

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    Dot dot = _dotFactory.Create(Dot.Name.Basic);

                    dot.Inject(_effectFactory, new Vector2Int(i, j), (index) => { _fsm.OnClickDot(index); });
                    dot.transform.SetParent(_dotGridContent.transform);
                    dot.transform.localScale = Vector3.one;

                    dots[i, j] = dot;
                }
            }

            Dot[] colorPenDots = new Dot[mapData.PickColors.Count];

            for (int i = 0; i < mapData.PickColors.Count; i++)
            {
                Dot colorPenDot = _dotFactory.Create(Dot.Name.ColorPen);
                colorPenDot.ChangeColor(_modeData.PickColors[mapData.PickColors[i]]);

                colorPenDot.Inject(_effectFactory, _penToggleGroup, mapData.PickColors[i], (index) => { _fsm.OnClickDot(index); });
                colorPenDot.transform.SetParent(_penContent);
                colorPenDot.transform.localScale = Vector3.one;

                colorPenDot.Minimize();
                colorPenDots[i] = colorPenDot;
            }

            SetLevelData?.Invoke(dots, colorPenDots, mapData);
        }
    }
}