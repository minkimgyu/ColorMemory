using Challenge;
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

        public override void OnClickSkipBtn()
        {
            GoToPaintState();
        }

        public override void OnStateEnter()
        {

            // �ʱ�ȭ ����
            Tuple<Dot[,], Dot[], MapData> levelData = GetLevelData();
            _dots = levelData.Item1;
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
                    // ���� ���� ������ �������ֱ�
                    _dots[i, j].ChangeColor(GetDotColor(i, j));

                    // �����ϰ� Ű���
                    _dots[i, j].Maximize(1f);
                }
            }

            _collectStageUIPresenter.ActivateTimerContent(true);
            _collectStageUIPresenter.ActivateBottomContent(false);
            _collectStageUIPresenter.ActivateSkipBtn(true);

            _collectStageUIPresenter.ActivateRememberPanel(true);
            _collectStageUIPresenter.ChangeTotalTime(_data.MemorizeDuration);
            _timer.Start(_data.MemorizeDuration);
        }

        void GoToPaintState()
        {
            _collectStageUIPresenter.ActivateTimerContent(false);
            _collectStageUIPresenter.ActivateBottomContent(true);
            _collectStageUIPresenter.ActivateSkipBtn(false);

            _collectStageUIPresenter.ActivateRememberPanel(false);

            // dot ������ �ڵ� �߰�
            for (int i = 0; i < _levelSize.x; i++)
            {
                for (int j = 0; j < _levelSize.y; j++)
                {
                    _dots[i, j].Expand(_fadeColor, 1.5f);
                }
            }

            _timer.Reset(); // Ÿ�̸� ����

            // ���� �ð� ������ ���� State�� �̵�
            DOVirtual.DelayedCall(1.5f, () =>
            {
                _fsm.SetState(CollectMode.State.Paint);
            });
        }

        public override void OnStateUpdate()
        {
            _collectStageUIPresenter.ChangeLeftTime(_timer.LeftTime, 1 - _timer.Ratio);

            if (_timer.CurrentState == Timer.State.Finish)
            {
                GoToPaintState();
                return;
            }
        }
    }
}