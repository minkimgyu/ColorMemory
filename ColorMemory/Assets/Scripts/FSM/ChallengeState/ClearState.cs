using UnityEngine;
using System;
using DG.Tweening;
using static UnityEditor.Timeline.TimelinePlaybackControls;

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
            _challengeStageUIPresenter = challengeStageUIPresenter;
            _modeData = modeData;
            this.GetStage = GetStage;
            this.DestroyDots = DestroyDots;
        }

        PaintState.Data _sentData;

        public override void OnStateEnter(PaintState.Data sentData)
        {
            _sentData = sentData;

            DOVirtual.DelayedCall(0.5f, () =>
            {
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

                DOVirtual.DelayedCall(1.5f, () =>
                {
                    DestroyDots?.Invoke();
                    _fsm.SetState(ChallengeMode.State.Initialize);
                });
            }); // 이 GameObject가 파괴되면 자동 취소;;
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

        public override void OnStateUpdate()
        {
        }
    }
}