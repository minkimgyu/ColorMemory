using UnityEngine;

public class EndState : BaseState<ChallengeStageController.State>
{
    ChallengeStageUIPresenter _challengeStageUIController;

    public EndState(
        FSM<ChallengeStageController.State> fsm,
        ChallengeStageUIPresenter challengeStageUIController) : base(fsm)
    {
        _challengeStageUIController = challengeStageUIController;
    }

    public override void OnStateEnter()
    {
        _challengeStageUIController.ActivateEndPanel(true);
    }
}
