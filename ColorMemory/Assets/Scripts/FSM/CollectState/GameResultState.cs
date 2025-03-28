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

        public override void OnStateEnter()
        {
            _collectStageUIPresenter.ActivateGameResultPanel(true);

            _collectStageUIPresenter.ChangeGoldCount(_modeData.MyScore);
        }

        public override void OnStateExit()
        {
            _collectStageUIPresenter.ActivateGameResultPanel(false);
        }
    }
}