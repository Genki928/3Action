public interface IGameStateBase
{
    void Enter();
    void Execute();
    void Exit();
}

public interface IExploringState : IGameStateBase { }