using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Challenge
{
    public class GameOverState : BaseState<ChallengeMode.State>
    {
        ChallengeStageUIPresenter _challengeStageUIPresenter;
        Color[] _pickColors;
        ClearPatternUIFactory _clearPatternUIFactory;

        ChallengeMode.ModeData _modeData;

        public GameOverState(
            FSM<ChallengeMode.State> fsm,
            ChallengeStageUIPresenter challengeStageUIPresenter,
            Color[] pickColors,
            ClearPatternUIFactory clearPatternUIFactory,
            ChallengeMode.ModeData modeData) : base(fsm)
        {
            _challengeStageUIPresenter = challengeStageUIPresenter;
            _pickColors = pickColors;
            _clearPatternUIFactory = clearPatternUIFactory;
            _modeData = modeData;
        }

        public override void OnClickNextBtn()
        {
            _fsm.SetState(ChallengeMode.State.Result);
        }

        public override void OnStateEnter()
        {
            _challengeStageUIPresenter.ActivateGameOverPanel(true);

            for (int i = 0; i < _modeData.ClearStageCount; i++)
            {
                SpawnableUI patternUI = _clearPatternUIFactory.Create(i + 1, _modeData.ClearStageCount, _modeData.StageData[i], _pickColors);
                _challengeStageUIPresenter.AddClearPattern(patternUI);
            }

            _challengeStageUIPresenter.ChangeClearStageCount((int)_modeData.PassedDuration, _modeData.ClearStageCount);
        }

        public override void OnStateExit()
        {
            _challengeStageUIPresenter.RemoveClearPattern();
            _challengeStageUIPresenter.ActivateGameOverPanel(false);
        }
    }
}