using UnityEngine;
using System.Collections.Generic;
using System;

public class FSM<State>
{
    Dictionary<State, BaseState<State>> _states;

    State _currentState;
    public State CurrentState { get => _currentState; }

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

    public void ChangeLanguage() => _states[_currentState].ChangeLanguage();
    public void OnClickDot(Vector2Int index) => _states[_currentState].OnClickDot(index);
    public void OnClickDot(int index) => _states[_currentState].OnClickDot(index);
}
