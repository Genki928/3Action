using UnityEngine;

public class PhaseResultState : ExploringStateBase
{
    public override void Enter(LegStateManager stateManager)
    {
        ;
    }

    public override void Execute(LegStateManager stateManager)
    {
        ;
    }

    public override void Exit(LegStateManager stateManager)
    {
        stateManager.NotifyRaidFinished();
    }

    public override void Interact(LegStateManager stateManager)
    {
        stateManager.NextState(stateManager);
    }
}