using UnityEngine;

public class EndState : BaseState<ChallengeStageController.State>
{
    ChallengeStageUIController _challengeStageUIController;

    public EndState(
        FSM<ChallengeStageController.State> fsm,
        ChallengeStageUIController challengeStageUIController) : base(fsm)
    {
        _challengeStageUIController = challengeStageUIController;
    }

    public override void OnStateEnter()
    {
        _challengeStageUIController.ActivateEndPanel(true);
    }
}
