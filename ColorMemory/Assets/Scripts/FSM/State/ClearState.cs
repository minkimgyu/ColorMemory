using UnityEngine;
using System;
using DG.Tweening;

public class ClearState : BaseState<ChallengeStageController.State>
{
    Vector2Int _levelSize;
    Action DestroyDots;

    public ClearState(
        FSM<ChallengeStageController.State> fsm,
        Action DestroyDots
    ) : base(fsm)
    {
        this.DestroyDots = DestroyDots;
    }

    public override void OnStateEnter()
    {
        DOVirtual.DelayedCall(1.5f, () =>
        {
            DestroyDots?.Invoke();
            _fsm.SetState(ChallengeStageController.State.Init);
        });
    }

    public override void OnStateExit()
    {
    }

    public override void OnStateUpdate()
    {
    }
}
