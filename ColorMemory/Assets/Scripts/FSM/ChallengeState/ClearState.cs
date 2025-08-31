using DG.Tweening;
using NetworkService.DTO;
using System;
using UnityEngine;

namespace Challenge
{
    public class ClearState : BaseState<ChallengeMode.State>
    {
        Action DestroyDots;

        Func<Tuple<Dot[,], Dot[], MapData>> GetStage;
        ChallengeStageUIPresenter _challengeStageUIPresenter;

        ChallengeMode.ModeData _modeData;

        public ClearState(
            FSM<ChallengeMode.State> fsm,
            ChallengeStageUIPresenter challengeStageUIPresenter,

            ChallengeMode.ModeData modeData,

            Func<Tuple<Dot[,], Dot[], MapData>> GetStage,
            Action DestroyDots) : base(fsm)
        {
            _stateTimer = new Timer();
            _challengeStageUIPresenter = challengeStageUIPresenter;
            _challengeStageUIPresenter.OnClickPauseGameExitBtn += () => { _fsm.SetState(ChallengeMode.State.GameOver); };

            _modeData = modeData;
            this.GetStage = GetStage;
            this.DestroyDots = DestroyDots;
        }

        PaintState.Data _sentData;

        public enum State
        {
            DelayAfterClear,
            Minimizing,
            CompleteStage,
        }

        State _currentState;
        Timer _stateTimer;
        const float _stateChangeDelay = 1.5f;

        void MinimizeAllDots()
        {
            ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.StageClear);

            Tuple<Dot[,], Dot[], MapData> levelData = GetStage();
            Dot[,] dots = levelData.Item1;
            Vector2Int levelSize = new Vector2Int(dots.GetLength(0), dots.GetLength(1));

            for (int i = 0; i < levelSize.x; i++)
            {
                for (int j = 0; j < levelSize.y; j++)
                {
                    dots[i, j].Minimize(1f);
                }
            }
        }

        void CompleteStage()
        {
            DestroyDots?.Invoke(); // 모든 닷 제거
            _fsm.SetState(ChallengeMode.State.Initialize);
        }

        public override void OnStateEnter(PaintState.Data sentData)
        {
            _sentData = sentData;
            _currentState = State.DelayAfterClear;
            _stateTimer.Reset();
        }

        public override void OnStateUpdate()
        {
            if (_stateTimer.CurrentState == Timer.State.Running) return;

            // 타이머가 완료되었다면 현재 상태에서 다음 상태로 넘어가기
            if (_stateTimer.CurrentState == Timer.State.Finish)
            {
                switch (_currentState)
                {
                    case State.DelayAfterClear:
                        _currentState = State.Minimizing;
                        break;
                    case State.Minimizing:
                        _currentState = State.CompleteStage;
                        break;
                }
            }

            // 다음 상태로 넘어가기
            switch (_currentState)
            {
                case State.DelayAfterClear:
                    _stateTimer.Reset();
                    _stateTimer.Start(0.5f);
                    break;
                case State.Minimizing:
                    MinimizeAllDots();

                    _stateTimer.Reset();
                    _stateTimer.Start(_stateChangeDelay);
                    break;
                case State.CompleteStage:
                    // 다음 스테이지로 넘어가기
                    CompleteStage();
                    break;
            }
        }

        const int clearPoint = 100;
        const int pointFixedWeight = 1;

        public override void OnStateExit()
        {
            float weight = pointFixedWeight + _sentData.LeftDurationRatio;
            _modeData.MyScore += Mathf.RoundToInt(clearPoint * weight);
            _modeData.ClearStageCount += 1;

            _challengeStageUIPresenter.ChangeNowScore(_modeData.MyScore);
            _challengeStageUIPresenter.ChangeBestScore(_modeData.BestScore);
        }
    }
}