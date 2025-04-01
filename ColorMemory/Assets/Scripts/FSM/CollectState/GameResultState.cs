using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collect
{
    public class GameResultState : BaseState<CollectMode.State>
    {
        CollectStageUIPresenter _collectStageUIPresenter;
        CollectMode.Data _modeData;

        public GameResultState(
            FSM<CollectMode.State> fsm,
            CollectStageUIPresenter collectStageUIPresenter,
            CollectMode.Data modeData) : base(fsm)
        {
            _collectStageUIPresenter = collectStageUIPresenter;
            _modeData = modeData;
        }

        public override void OnClickNextBtn()
        {
            ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.HomeScene);
        }

        public override void OnStateEnter()
        {
            _collectStageUIPresenter.ActivatePlayPanel(false);
            _collectStageUIPresenter.ActivateGameResultPanel(true);

            AddressableHandler addressableHandler = UnityEngine.Object.FindObjectOfType<AddressableHandler>();
            if (addressableHandler == null) return;

            SaveData saveData = ServiceLocater.ReturnSaveManager().GetSaveData();

            //_collectStageUIPresenter.ChangeArtwork();
            //_collectStageUIPresenter.ChangeGetRank();
            //_collectStageUIPresenter.ChangeRank();
            //_collectStageUIPresenter.ChangeCollectionRatio();
        }

        public override void OnStateExit()
        {
            _collectStageUIPresenter.ActivateGameResultPanel(false);
        }
    }
}