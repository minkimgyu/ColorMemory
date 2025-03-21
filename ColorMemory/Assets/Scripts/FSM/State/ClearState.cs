using UnityEngine;
using System;
using DG.Tweening;

public class ClearState : BaseState<ChallengeStageController.State>
{
    

    Action DestroyDots;
    Action<PaintState.Data> OnStageClear;

    Func<Tuple<Dot[,], Dot[], MapData>> GetLevelData;

    ChallengeStageUIPresenter _challengeStageUIController;

    public ClearState(
        FSM<ChallengeStageController.State> fsm,
        ChallengeStageUIPresenter challengeStageUIPresenter,

        Func<Tuple<Dot[,], Dot[], MapData>> GetLevelData,
        Action DestroyDots,
        Action<PaintState.Data> OnStageClear
    ) : base(fsm)
    {
        _challengeStageUIController = challengeStageUIPresenter;
        this.GetLevelData = GetLevelData;
        this.DestroyDots = DestroyDots;
        this.OnStageClear = OnStageClear;
    }

    PaintState.Data _data;

    public override void OnStateEnter(PaintState.Data data)
    {
        _data = data;

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
        OnStageClear?.Invoke(_data);
    }

    public override void OnStateUpdate()
    {
    }
}
