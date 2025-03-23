using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collect
{
    public class GameResultState : BaseState<CollectMode.State>
    {
        ChallengeStageUIPresenter _challengeStageUIPresenter;
        System.Func<CollectMode.Data> GetChallengeModeData;

        public GameResultState(
            FSM<CollectMode.State> fsm,
            ChallengeStageUIPresenter challengeStageUIPresenter,
            System.Func<CollectMode.Data> GetChallengeModeData) : base(fsm)
        {
            _challengeStageUIPresenter = challengeStageUIPresenter;
            this.GetChallengeModeData = GetChallengeModeData;
        }

        public override void OnStateEnter()
        {
            _challengeStageUIPresenter.ActivateGameResultPanel(true);

            CollectMode.Data modeData = GetChallengeModeData();
            _challengeStageUIPresenter.ChangeResultScore(modeData.MyScore);
            _challengeStageUIPresenter.ChangeGoldCount(modeData.MyScore);
        }

        public override void OnStateExit()
        {
            _challengeStageUIPresenter.ActivateGameResultPanel(false);
        }
    }
}