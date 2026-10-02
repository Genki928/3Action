using UnityEngine.InputSystem;

public class LegStateManager : StateManagerBase<IExploringState>
{
    void Start()
    {
        // ステート追加
        _states.Add(() => new ExploringState());
        _states.Add(() => new RaidState());

        // 処理開始
        StartState();
    }

    public void DebugChangeState(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        NextState();
    }
}