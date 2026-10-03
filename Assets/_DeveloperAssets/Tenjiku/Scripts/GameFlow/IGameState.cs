
public interface IGameStateBase<T> where T : StateManagerBase
{
    void Enter(T stateManager);
    void Execute(T stateManager);
    void Exit(T stateManager);
}

public interface IExploringState : IGameStateBase<LegStateManager> { }