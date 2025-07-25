using System;
using UnityEngine;

abstract public class BaseState<T>
{
    protected FSM<T> _fsm;
    public BaseState(FSM<T> fsm) 
    { 
        _fsm = fsm; 
    }

    public virtual void OnStateEnter(Challenge.PaintState.Data data) { }
    public virtual void OnStateEnter() { }
    public virtual void OnStateUpdate() { }
    public virtual void OnStateExit() { }

    //public virtual void OnLanguageChanged() { }

    //public virtual void OnClickNextStageBtn() { }
    //public virtual void OnClickExitBtn() { }

    //public virtual void OnClickGoToGameOver() { }

    //public virtual void OnClickNextBtn() { }
    //public virtual void OnClickRetryBtn() { }

    //public virtual void OnClickGoBackHint() { }
    //public virtual void OnClickOneZoneHint() { }
    //public virtual void OnClickOneColorHint() { }

    public virtual void OnClickDot(Vector2Int index) { }
    public virtual void OnClickDot(int index) { }

    public virtual void OnClickHomeBtn() { }
    public virtual void OnClickShopBtn() { }
    public virtual void OnClickRankingBtn() { }
    //public virtual void OnClickSettingBtn() { }

    //public virtual void OnClickPauseBtn() { }

    //public virtual void OnClickSkipBtn() { }

    public virtual void ChangeLanguage() { }
}
