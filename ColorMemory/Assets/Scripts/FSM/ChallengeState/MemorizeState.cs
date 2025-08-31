using Collect;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Challenge
{
    public class MemorizeState : BaseState<ChallengeMode.State>
    {
        Dot[,] _dots;
        Vector2Int _levelSize;
        MapData _mapData;

        Color[] _pickColors;

        readonly Color _fadeColor = new Color(236f / 255f, 232f / 255f, 232f / 255f);

        ChallengeMode.ModeData _data;
        List<LevelData> _stageDatas;

        Func<Tuple<Dot[,], Dot[], MapData>> GetStage;

        ChallengeStageUIPresenter _challengeStageUIPresenter;

        public MemorizeState(
            FSM<ChallengeMode.State> fsm,
            Color[] pickColors,
            ChallengeMode.ModeData modeData,
            List<LevelData> stageDatas,

            ChallengeStageUIPresenter challengeStageUIPresenter,
            Func<Tuple<Dot[,], Dot[], MapData>> GetStage
        ) : base(fsm)
        {
            _stateTimer = new Timer();
            _pickColors = pickColors;
            _data = modeData;
            _stageDatas = stageDatas;

            _challengeStageUIPresenter = challengeStageUIPresenter;
            _challengeStageUIPresenter.OnClickSkipBtn += OnClickSkipBtn;
            this.GetStage = GetStage;
        }

        Color GetDotColor(int row, int col)
        {
            return _pickColors[_mapData.DotColor[row, col]];
        }

        void OnClickSkipBtn()
        {
            _stateTimer.Reset();
            _currentState = State.Fade;
        }

        public void MaximizePreviewDots()
        {
            // 초기화 진행
            Tuple<Dot[,], Dot[], MapData> levelData = GetStage();
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
                    // 원래 레벨 색으로 변경해주기
                    _dots[i, j].ChangeColor(GetDotColor(i, j));

                    // 랜덤하게 키우기
                    _dots[i, j].Maximize(1f);
                }
            }

            int index = Mathf.Clamp(_data.StageCount - 1, 0, _stageDatas.Count - 1);
            float memorizeDuration = _stageDatas[index].MemorizeDuration;

            _challengeStageUIPresenter.ActivateHint(false, false);
            _challengeStageUIPresenter.ActivateBottomContent(false);
            _challengeStageUIPresenter.ActivateSkipBtn(true);

            string rememberTxt = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.RememberTitle);
            _challengeStageUIPresenter.ActivateRememberPanel(true, rememberTxt);
            _challengeStageUIPresenter.ChangeTotalTime(memorizeDuration);
        }

        void FadePreviewDots()
        {
            _challengeStageUIPresenter.ActivateBottomContent(true);
            _challengeStageUIPresenter.ActivateSkipBtn(false);

            string rememberTxt = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.RememberTitle);
            _challengeStageUIPresenter.ActivateRememberPanel(false, rememberTxt);

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
                    _challengeStageUIPresenter.ChangeLeftTime(_stateTimer.LeftTime, 1 - _stateTimer.Ratio);
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

                    int index = Mathf.Clamp(_data.StageCount - 1, 0, _stageDatas.Count - 1);
                    float memorizeDuration = _stageDatas[index].MemorizeDuration;

                    _stateTimer.Reset();
                    _stateTimer.Start(memorizeDuration);
                    break;
                case State.Fade:
                    FadePreviewDots();

                    _stateTimer.Reset();
                    _stateTimer.Start(_stateChangeDelay);
                    break;
                case State.ChangeState:

                    _fsm.SetState(ChallengeMode.State.Paint);
                    break;
            }
        }
    }
}