public class ExploringStateBase : IExploringState
{
    // --- フィールド ---
    protected float _dayCycleTimerLimit = 0.0f;
    protected float _dayCycleTimer = 0.0f;

    // --- メソッド ---
    public virtual void Enter(LegStateManager stateManager) { }
    public virtual void Execute(LegStateManager stateManager) { }
    public virtual void Exit(LegStateManager stateManager) { }
    public virtual void Interact(LegStateManager stateManagerx) { }
}