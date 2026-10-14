
public interface IGameStateBase<T> where T : StateManagerBase
{
    // --- メソッド ---
    /// <summary> ステート開始時に実行されるメソッド </summary>
    void Enter(T stateManager);

    /// <summary> 毎フレーム実行されるメソッド </summary>
    void Execute(T stateManager);

    /// <summary> ステート終了時に実行されるメソッド </summary>
    void Exit(T stateManager);

    /// <summary> 決定ボタンを押した時に実行されるメソッド </summary>
    public void Interact(T stateManagerx);
}

public interface IExploringState : IGameStateBase<LegStateManager>
{
    public float Ratio { get; }
}