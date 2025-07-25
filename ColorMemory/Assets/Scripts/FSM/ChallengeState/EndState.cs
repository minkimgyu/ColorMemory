using NetworkService.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Challenge
{
    public class EndState : BaseState<ChallengeMode.State>
    {
        ChallengeStageUIPresenter _challengeStageUIPresenter;
        Color[] _pickColors;
        ClearPatternUIFactory _clearPatternUIFactory;

        ChallengeMode.ModeData _modeData;

        public EndState(
            FSM<ChallengeMode.State> fsm,
            ChallengeStageUIPresenter challengeStageUIPresenter,
            Color[] pickColors,
            ClearPatternUIFactory clearPatternUIFactory,
            ChallengeMode.ModeData modeData) : base(fsm)
        {
            _challengeStageUIPresenter = challengeStageUIPresenter;

            _challengeStageUIPresenter.OnClickNextBtn += OnClickNextBtn;
            _challengeStageUIPresenter.OnClickGameOverExitBtn += OnClickExitBtn;

            _pickColors = pickColors;
            _clearPatternUIFactory = clearPatternUIFactory;
            _modeData = modeData;
        }

        void OnClickExitBtn()
        {
            ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.HomeScene);
        }

        void OnClickNextBtn()
        {
            _fsm.SetState(ChallengeMode.State.Result);
        }

        public override void OnStateEnter()
        {
            _challengeStageUIPresenter.ActivatePlayPanel(false);
            _challengeStageUIPresenter.ActivateGameOverPanel(true);

            ServiceLocater.ReturnSoundPlayer().PlayBGM(ISoundPlayable.SoundName.ChallengeResultBGM);

            for (int i = 0; i < _modeData.ClearStageCount; i++)
            {
                SpawnableUI patternUI = _clearPatternUIFactory.Create(i + 1, _modeData.ClearStageCount, _modeData.StageData[i], _pickColors);
                _challengeStageUIPresenter.AddClearPattern(patternUI);
            }

            _challengeStageUIPresenter.ChangeClearStageCount(_modeData.ClearStageCount, _modeData.MyScore);
        }

        public override void OnStateExit()
        {
            _challengeStageUIPresenter.RemoveClearPattern();
            _challengeStageUIPresenter.ActivateGameOverPanel(false);
        }
    }
}