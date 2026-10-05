using UnityEngine;

public class PhaseResultState : ExploringStateBase
{
    public override void Enter(LegStateManager stateManager)
    {
        Debug.Log("Enter ResultState");
    }

    public override void Execute(LegStateManager stateManager)
    {
        ;
    }

    public override void Exit(LegStateManager stateManager)
    {
        stateManager.NotifyRaidFinished();
        stateManager.pNextPhase();
        Debug.Log("Exit ResultState");
    }
}