using UnityEngine;
using System;
using DG.Tweening;

public class ClearState : BaseState<ChallengeStageController.State>
{
    Action DestroyDots;
    Func<Tuple<Dot[,], Dot[], MapData>> GetLevelData;

    ChallengeStageUIController _challengeStageUIController;

    public ClearState(
        FSM<ChallengeStageController.State> fsm,
        ChallengeStageUIController challengeStageUIController,

        Func<Tuple<Dot[,], Dot[], MapData>> GetLevelData,
        Action DestroyDots
    ) : base(fsm)
    {
        _challengeStageUIController = challengeStageUIController;
        this.GetLevelData = GetLevelData;
        this.DestroyDots = DestroyDots;
    }

    public override void OnStateEnter()
    {
        DOVirtual.DelayedCall(0.5f, () =>
        {
            Tuple<Dot[,], Dot[], MapData> levelData = GetLevelData();
            Dot[,] dots = levelData.Item1;
            Vector2Int levelSize = new Vector2Int(dots.GetLength(0), dots.GetLength(1));

            for (int i = 0; i < levelSize.x; i++)
            {
                for (int j = 0; j < levelSize.y; j++)
                {
                    // 랜덤하게 키우기
                    dots[i, j].Minimize(1f);
                }
            }

            DOVirtual.DelayedCall(1.5f, () =>
            {
                DestroyDots?.Invoke();
                _fsm.SetState(ChallengeStageController.State.Init);
            });
        });
    }

    public override void OnStateExit()
    {
    }

    public override void OnStateUpdate()
    {
    }
}
