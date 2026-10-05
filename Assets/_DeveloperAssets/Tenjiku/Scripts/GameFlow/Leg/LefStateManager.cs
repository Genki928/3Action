using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LegStateManager : StateManagerBase<LegStateManager, IExploringState>
{
    // --- イベント ---
    public event Action OnExploreFinished;
    public event Action OnRaidFinished;
    public event Action OnResultFinished;

    // --- プロパティ ---
    public int Phase => _phase;

    // --- フィールド ---
    // フェーズ
    [SerializeField] public float _raidTimerLimit = 0.0f;
    [SerializeField] public float _exploringTimerLimit = 0.0f;
    protected int _phase = 0;

    // --- メソッド ---
    public void NotifyExploreFinished() => OnExploreFinished?.Invoke();
    public void NotifyRaidFinished() => OnRaidFinished?.Invoke();
    public void NotifyResultFinished() => OnResultFinished?.Invoke();

    void Start()
    {
        // ステート追加
        _states.Add(() => new ExploringState());
        _states.Add(() => new RaidState());
        _states.Add(() => new PhaseResultState());

        OnRaidFinished += FinishedMessage;

        // 処理開始
        StartState(this);

    }

    void Update()
    {
        _currentState.Execute(this);
    }

    public void DebugChangeState(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        NextState(this);
    }

    public void FinishedMessage()
    {
        Debug.Log($"( Phase.{_phase} Finished )");
    }

    public void pNextPhase() => ++_phase;
}