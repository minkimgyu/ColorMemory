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
        ClearPatternFactory _clearPatternFactory;

        public GameOverState(
            FSM<ChallengeMode.State> fsm,
            ChallengeStageUIPresenter challengeStageUIPresenter,
            Color[] pickColors,
            ClearPatternFactory clearPatternFactory,
            System.Func<ChallengeMode.Data> GetStageControllerData) : base(fsm)
        {
            _challengeStageUIPresenter = challengeStageUIPresenter;
            _pickColors = pickColors;
            _clearPatternFactory = clearPatternFactory;
            this.GetStageControllerData = GetStageControllerData;

            _clearPatternUIs = new List<ClearPatternUI>();
        }

        List<ClearPatternUI> _clearPatternUIs;

        public override void OnStateEnter()
        {
            _challengeStageUIPresenter.ActivateGameOverPanel(true);

            ChallengeMode.Data modeData = GetStageControllerData();

            for (int i = 0; i < modeData.ClearStageCount; i++)
            {
                ClearPatternUI patternUI = _clearPatternFactory.Create(modeData.StageData[i], _pickColors);
                _clearPatternUIs.Add(patternUI);

                _challengeStageUIPresenter.AddClearPattern(patternUI);
            }

            _challengeStageUIPresenter.ChangeClearStageCount(modeData.ClearStageCount);
        }

        public override void OnStateExit()
        {
            for (int i = 0; i < _clearPatternUIs.Count; i++)
            {
                _clearPatternUIs[i].DestroyObject();
            }
            _challengeStageUIPresenter.ActivateGameOverPanel(false);
        }
    }
}