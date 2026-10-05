using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class  ExploringStateBase : IExploringState
{
    //
    protected float _dayCycleTimerLimit = 0.0f;
    protected float _dayCycleTimer = 0.0f;

    //
    public virtual void Enter(LegStateManager stateManager) { }
    public virtual void Execute(LegStateManager stateManager) { }
    public virtual void Exit(LegStateManager stateManager) { }
}

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
        ;
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