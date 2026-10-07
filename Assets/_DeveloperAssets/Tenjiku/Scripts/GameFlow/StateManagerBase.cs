using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManagerBase : MonoBehaviour { }

public class StateManagerBase<TManager, TState> : StateManagerBase
    where TManager : StateManagerBase
    where TState : IGameStateBase<TManager>
{
    // --- フィールド ---
    // ステートの切り替え
    protected List<Func<TState>> _states = new();
    protected TState _currentState;
    protected int _currentStateIndex = 0;

    void Start()
    {
        ;
    }

    /// <summary> ステートを次に切り替える </summary>
    public void NextState(TManager stateManager)
    {
        _currentState?.Exit(stateManager);
        _currentState = _states[(++_currentStateIndex) % _states.Count]();
        _currentState.Enter(stateManager);
    }

    protected void StartState(TManager stateManager)
    {
        _currentState = _states[0]();
        _currentState.Enter(stateManager);
    }
}