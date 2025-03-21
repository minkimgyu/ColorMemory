using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResultState : BaseState<ChallengeStageController.State>
{
    ChallengeStageUIPresenter _challengeStageUIPresenter;
    System.Func<ChallengeStageController.Data> GetChallengeModeData;

    public GameResultState(
        FSM<ChallengeStageController.State> fsm,
        ChallengeStageUIPresenter challengeStageUIPresenter,
        System.Func<ChallengeStageController.Data> GetChallengeModeData) : base(fsm)
    {
        _challengeStageUIPresenter = challengeStageUIPresenter;
        this.GetChallengeModeData = GetChallengeModeData;
    }

    public override void OnStateEnter()
    {
        _challengeStageUIPresenter.ActivateGameResultPanel(true);

        ChallengeStageController.Data modeData = GetChallengeModeData();
        _challengeStageUIPresenter.ChangeBestScore(modeData.ClearStageCount);
    }

    public override void OnStateExit()
    {
        _challengeStageUIPresenter.ActivateGameResultPanel(false);
    }
}
