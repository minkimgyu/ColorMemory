using UnityEngine;
using System;
using DG.Tweening;

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
            _challengeStageUIPresenter.OnClickPauseGameExitBtn += () => { _fsm.SetState(ChallengeMode.State.GameOver); };

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

                DOVirtual.DelayedCall(1.5f, () =>
                {
                    // 만약 현재 상태가 다른 상태라면 실행되지 못하게 막아야함
                    if (_fsm.CurrentState != ChallengeMode.State.Clear) return;

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
    }
}