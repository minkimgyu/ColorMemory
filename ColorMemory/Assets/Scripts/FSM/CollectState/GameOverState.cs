//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Collect
//{
//    public class GameOverState : BaseState<CollectMode.State>
//    {
//        ChallengeStageUIPresenter _challengeStageUIPresenter;
//        Color[] _pickColors;
//        ClearPatternUIFactory _clearPatternUIFactory;

//        public GameOverState(
//            FSM<CollectMode.State> fsm,
//            ChallengeStageUIPresenter challengeStageUIPresenter,
//            Color[] pickColors,
//            ClearPatternUIFactory clearPatternUIFactory,
//            System.Func<CollectMode.Data> GetStageControllerData) : base(fsm)
//        {
//            _challengeStageUIPresenter = challengeStageUIPresenter;
//            _pickColors = pickColors;
//            _clearPatternUIFactory = clearPatternUIFactory;
//        }

//        public override void OnStateEnter()
//        {
//            _challengeStageUIPresenter.ActivateGameOverPanel(true);

//            CollectMode.Data modeData = GetStageControllerData();

//            //for (int i = 0; i < modeData.ClearStageCount; i++)
//            //{
//            //    SpawnableUI patternUI = _clearPatternUIFactory.Create(modeData.StageData[i], _pickColors);
//            //    _challengeStageUIPresenter.AddClearPattern(patternUI);
//            //}

//            //_challengeStageUIPresenter.ChangeClearStageCount(modeData.ClearStageCount);
//        }

//        public override void OnStateExit()
//        {
//            _challengeStageUIPresenter.RemoveClearPattern();
//            _challengeStageUIPresenter.ActivateGameOverPanel(false);
//        }
//    }
//}