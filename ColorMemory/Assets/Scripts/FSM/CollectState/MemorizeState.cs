using Challenge;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Collect
{
    public class MemorizeState : BaseState<CollectMode.State>
    {
        Dot[,] _dots;
        Dot[] _penDots;
        Vector2Int _levelSize;
        MapData _mapData;

        readonly Color _fadeColor = new Color(236f / 255f, 232f / 255f, 232f / 255f);

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
            _stateTimer = new Timer();

            _collectStageUIPresenter = collectStageUIPresenter;
            collectStageUIPresenter.OnClickSkipBtn += OnClickSkipBtn;

            this.GetLevelData = GetLevelData;
        }

        Color GetDotColor(int row, int col)
        {
            return _data.PickColors[_mapData.DotColor[row, col]];
        }

        void OnClickSkipBtn()
        {
            _stateTimer.Reset();
            _currentState = State.Fade;
        }

        void MaximizePreviewDots()
        {
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
                    _dots[i, j].Maximize(1f);
                }
            }

            _collectStageUIPresenter.ActivateTimerContent(true);
            _collectStageUIPresenter.ActivateBottomContent(false);
            _collectStageUIPresenter.ActivateSkipBtn(true);

            _collectStageUIPresenter.ActivateRememberPanel(true);
            _collectStageUIPresenter.ChangeTotalTime(_data.MemorizeDuration);
        }

        void FadePreviewDots()
        {
            _collectStageUIPresenter.ActivateTimerContent(false);
            _collectStageUIPresenter.ActivateBottomContent(true);
            _collectStageUIPresenter.ActivateSkipBtn(false);

            _collectStageUIPresenter.ActivateRememberPanel(false);

            // dot 뒤집는 코드 추가
            for (int i = 0; i < _levelSize.x; i++)
            {
                for (int j = 0; j < _levelSize.y; j++)
                {
                    _dots[i, j].Expand(_fadeColor, 1.5f);
                }
            }
        }

        public override void OnStateEnter()
        {
            _currentState = State.Maximize;
            _stateTimer.Reset();
        }


        public enum State
        {
            Maximize,
            Fade,
            ChangeState
        }

        State _currentState;
        Timer _stateTimer;
        const float _stateChangeDelay = 1.5f;

        public override void OnStateUpdate()
        {
            switch (_currentState)
            {
                case State.Maximize:
                    _collectStageUIPresenter.ChangeLeftTime(_stateTimer.LeftTime, 1 - _stateTimer.Ratio);
                    break;
            }

            if (_stateTimer.CurrentState == Timer.State.Running) return;

            // 타이머가 완료되었다면 현재 상태에서 다음 상태로 넘어가기
            if (_stateTimer.CurrentState == Timer.State.Finish)
            {
                switch (_currentState)
                {
                    case State.Maximize:
                        _currentState = State.Fade;
                        break;
                    case State.Fade:
                        _currentState = State.ChangeState;
                        break;
                }
            }

            // 다음 상태로 넘어가기
            switch (_currentState)
            {
                case State.Maximize:
                    MaximizePreviewDots();

                    _stateTimer.Reset();
                    _stateTimer.Start(_data.MemorizeDuration);
                    break;
                case State.Fade:
                    FadePreviewDots();

                    _stateTimer.Reset();
                    _stateTimer.Start(_stateChangeDelay);
                    break;
                case State.ChangeState:

                    _fsm.SetState(CollectMode.State.Paint);
                    break;
            }
        }
    }
}