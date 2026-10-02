using System;
using System.Collections.Generic;
using UnityEngine;

public class StateManagerBase<T> : MonoBehaviour where T : IGameStateBase
{
    protected List<Func<T>> _states = new();
    protected T _currentState;
    protected int _currentStateIndex = 0;

    void Start()
    {
        ;
    }

    protected void NextState()
    {
        _currentState?.Exit();
        _currentState = _states[(++_currentStateIndex) % _states.Count]();
        _currentState.Enter();
    }

    protected void StartState()
    {
        _currentState = _states[0]();
        _currentState.Enter();
    }
}