using System;
using UnityEngine;

abstract public class BaseState<T>
{
    protected FSM<T> _fsm;
    public BaseState(FSM<T> fsm) 
    { 
        _fsm = fsm; 
    }

    public virtual void OnStateEnter(PaintState.Data data) { }
    public virtual void OnStateEnter() { }
    public virtual void OnStateUpdate() { }
    public virtual void OnStateExit() { }

    public virtual void OnClickRandomFillHint() { }
    public virtual void OnClickRevealSameColorHint() { }

    public virtual void OnClickDot(Vector2Int index) { }
    public virtual void OnClickDot(int index) { }

    public virtual void OnClickHomeBtn() { }
    public virtual void OnClickShopBtn() { }
    public virtual void OnClickRankingBtn() { }
    public virtual void OnClickSettingBtn() { }
}
