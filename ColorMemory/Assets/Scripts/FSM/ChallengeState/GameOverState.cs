using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Challenge
{
    public class GameOverState : BaseState<ChallengeMode.State>
    {
        ChallengeStageUIPresenter _challengeStageUIPresenter;
        Color[] _pickColors;
        System.Func<ChallengeMode.Data> GetStageControllerData;
        ClearPatternUIFactory _clearPatternUIFactory;

        public GameOverState(
            FSM<ChallengeMode.State> fsm,
            ChallengeStageUIPresenter challengeStageUIPresenter,
            Color[] pickColors,
            ClearPatternUIFactory clearPatternUIFactory,
            System.Func<ChallengeMode.Data> GetStageControllerData) : base(fsm)
        {
            _challengeStageUIPresenter = challengeStageUIPresenter;
            _pickColors = pickColors;
            _clearPatternUIFactory = clearPatternUIFactory;
            this.GetStageControllerData = GetStageControllerData;
        }


        public override void OnStateEnter()
        {
            _challengeStageUIPresenter.ActivateGameOverPanel(true);

            ChallengeMode.Data modeData = GetStageControllerData();

            for (int i = 0; i < modeData.ClearStageCount; i++)
            {
                SpawnableUI patternUI = _clearPatternUIFactory.Create(modeData.StageData[i], _pickColors);
                _challengeStageUIPresenter.AddClearPattern(patternUI);
            }

            _challengeStageUIPresenter.ChangeClearStageCount(modeData.ClearStageCount);
        }

        public override void OnStateExit()
        {
            _challengeStageUIPresenter.RemoveClearPattern();
            _challengeStageUIPresenter.ActivateGameOverPanel(false);
        }
    }
}