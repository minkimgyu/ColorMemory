using UnityEngine;

public class EndState : BaseState<ChallengeStageController.State>
{
    ChallengeStageUIPresenter _challengeStageUIPresenter;

    public EndState(
        FSM<ChallengeStageController.State> fsm,
        ChallengeStageUIPresenter challengeStageUIController) : base(fsm)
    {
        _challengeStageUIPresenter = challengeStageUIController;
    }

    public override void OnStateEnter()
    {
        _challengeStageUIPresenter.ActivateGameOverPanel(true);
    }
}
