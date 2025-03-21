using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverState : BaseState<ChallengeStageController.State>
{
    ChallengeStageUIPresenter _challengeStageUIPresenter;
    Color[] _pickColors;
    System.Func<ChallengeStageController.Data> GetChallengeModeData;

    public GameOverState(
        FSM<ChallengeStageController.State> fsm,
        ChallengeStageUIPresenter challengeStageUIPresenter,
        Color[] pickColors,
        System.Func<ChallengeStageController.Data> GetChallengeModeData) : base(fsm)
    {
        _challengeStageUIPresenter = challengeStageUIPresenter;
        _pickColors = pickColors;
        this.GetChallengeModeData = GetChallengeModeData;
    }

    public override void OnStateEnter()
    {
        _challengeStageUIPresenter.ActivateGameOverPanel(true);

        ChallengeStageController.Data modeData = GetChallengeModeData();
        _challengeStageUIPresenter.ChangeClearStageCount(modeData.ClearStageCount);
    }

    public override void OnStateExit()
    {
        _challengeStageUIPresenter.ActivateGameOverPanel(false);
    }
}
