using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Collect
{
    public class MemorizeState : BaseState<CollectMode.State>
    {
        Dot[,] _dots;
        Dot[] _penDots;
        Vector2Int _levelSize;
        MapData _mapData;

        readonly Color _fadeColor = new Color(236f / 255f, 232f / 255f, 232f / 255f);

        Timer _timer;
        Func<Tuple<Dot[,], Dot[], MapData>> GetLevelData;

        CollectMode.Data _data;

        CollectStageUIPresenter _collectStageUIPresenter;

        public MemorizeState(
            FSM<CollectMode.State> fsm,
            CollectMode.Data data,

            CollectStageUIPresenter collectStageUIPresenter,
            Func<Tuple<Dot[,], Dot[], MapData>> GetLevelData
        ) : base(fsm)
        {
            _data = data;
            _timer = new Timer();

            _collectStageUIPresenter = collectStageUIPresenter;
            this.GetLevelData = GetLevelData;
        }

        Color GetDotColor(int row, int col)
        {
            return _data.PickColors[_mapData.DotColor[row, col]];
        }

        public override void OnStateEnter()
        {
            _collectStageUIPresenter.ActivateTimerContent(true);

            // 초기화 진행
            Tuple<Dot[,], Dot[], MapData> levelData = GetLevelData();
            _dots = levelData.Item1;
            _penDots = levelData.Item2;
            _mapData = levelData.Item3;
            _levelSize = new Vector2Int(_dots.GetLength(0), _dots.GetLength(1));

            for (int i = 0; i < _levelSize.x; i++)
            {
                for (int j = 0; j < _levelSize.y; j++)
                {
                    _dots[i, j].Minimize();
                }
            }

            for (int i = 0; i < _levelSize.x; i++)
            {
                for (int j = 0; j < _levelSize.y; j++)
                {
                    // 원래 레벨 색으로 변경해주기
                    _dots[i, j].ChangeColor(GetDotColor(i, j));

                    // 랜덤하게 키우기
                    _dots[i, j].Maximize(1f);
                }
            }

            _collectStageUIPresenter.ActivateRememberPanel(true);
            _collectStageUIPresenter.ChangeTotalTime(_data.MemorizeDuration);
            _timer.Start(_data.MemorizeDuration);
        }

        public override void OnStateUpdate()
        {
            _collectStageUIPresenter.ChangeLeftTime(_timer.LeftTime, 1 - _timer.Ratio);

            if (_timer.CurrentState == Timer.State.Finish)
            {
                _collectStageUIPresenter.ActivateRememberPanel(false);
                _collectStageUIPresenter.ActivateTimerContent(false);

                // dot 뒤집는 코드 추가
                for (int i = 0; i < _levelSize.x; i++)
                {
                    for (int j = 0; j < _levelSize.y; j++)
                    {
                        //float duration = Random.Range(0.5f, 1.5f);
                        Color originColor = GetDotColor(i, j);
                        _dots[i, j].ChangeColor(new Color(0, 0, 0, 0));
                        _dots[i, j].Expand(_fadeColor, originColor, 1.6f);
                    }
                }


                _timer.Reset(); // 타이머 리셋

                // 일정 시간 지나면 다음 State로 이동
                DOVirtual.DelayedCall(1.5f, () =>
                {
                    // dot 뒤집는 코드 추가
                    for (int i = 0; i < _levelSize.x; i++)
                    {
                        for (int j = 0; j < _levelSize.y; j++)
                        {
                            _dots[i, j].ChangeColor(_fadeColor);
                        }
                    }

                    _fsm.SetState(CollectMode.State.Paint);
                });
                return;
            }
        }
    }
}