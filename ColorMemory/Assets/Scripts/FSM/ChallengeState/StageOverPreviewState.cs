using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Challenge
{
    public class StageOverPreviewState : BaseState<ChallengeMode.State>
    {
        Color[] _pickColors;
        ChallengeMode.ModeData _modeData;
        ChallengeStageUIPresenter _challengeStageUIPresenter;

        public StageOverPreviewState(
             FSM<ChallengeMode.State> fsm,
             Color[] pickColors,
             ChallengeMode.ModeData modeData,
             ChallengeStageUIPresenter challengeStageUIPresenter) : base(fsm)
        {
            _pickColors = pickColors;
            _modeData = modeData;
            _challengeStageUIPresenter = challengeStageUIPresenter;
        }

        public override void OnStateUpdate()
        {
            _challengeStageUIPresenter.ActivateStageOverPreviewPanel(true);

            int lastStageIndex = _modeData.StageData.Count - 1;
            _challengeStageUIPresenter.ChangeLastStagePattern(_modeData.StageData[lastStageIndex], _pickColors);
            _challengeStageUIPresenter.ChangeStageOverInfo();
        }


    }
}