using UnityEngine;
using System.Collections.Generic;
using System;

public class FSM<State>
{
    Dictionary<State, BaseState<State>> _states;
    State _currentState;

    public void Initialize(Dictionary<State, BaseState<State>> states, State currentState, Vector2Int sectionIndex)
    {
        _states = states;
        _currentState = currentState;
        _states[_currentState].OnStateEnter(sectionIndex);
    }

    public void Initialize(Dictionary<State, BaseState<State>> states, State currentState)
    {
        _states = states;
        _currentState = currentState;
        _states[_currentState].OnStateEnter();
    }

    public void OnUpdate()
    {
        _states[_currentState].OnStateUpdate();
    }

    public void SetState(State state)
    {
        _states[_currentState].OnStateExit();
        _currentState = state;
        _states[_currentState].OnStateEnter();
    }

    public void SetState(State state, Challenge.PaintState.Data data)
    {
        _states[_currentState].OnStateExit();
        _currentState = state;
        _states[_currentState].OnStateEnter(data);
    }

    public void SetState(State state, Vector2Int sectionIndex)
    {
        _states[_currentState].OnStateExit();
        _currentState = state;
        _states[_currentState].OnStateEnter(sectionIndex);
    }

    public void OnClickHomeBtn() => _states[_currentState].OnClickHomeBtn();
    public void OnClickShopBtn() => _states[_currentState].OnClickShopBtn();
    public void OnClickRankingBtn() => _states[_currentState].OnClickRankingBtn();
    public void OnClickSettingBtn() => _states[_currentState].OnClickSettingBtn();

    public void OnClickNextBtn() => _states[_currentState].OnClickNextBtn();
    public void OnClickRetryBtn() => _states[_currentState].OnClickRetryBtn();

    public void OnClickNextStageBtn() => _states[_currentState].OnClickNextStageBtn();
    public void OnClickExitBtn() => _states[_currentState].OnClickExitBtn();

    public void OnClickDot(Vector2Int index) => _states[_currentState].OnClickDot(index);
    public void OnClickDot(int index) => _states[_currentState].OnClickDot(index);

    public void OnClickGoBackHint() => _states[_currentState].OnClickGoBackHint();
    public void OnClickRandomFillHint() => _states[_currentState].OnClickRandomFillHint();
    public void OnClickRevealSameColorHint() => _states[_currentState].OnClickRevealSameColorHint();
}
