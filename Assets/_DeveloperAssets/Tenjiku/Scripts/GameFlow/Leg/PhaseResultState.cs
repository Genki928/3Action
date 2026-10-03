using UnityEngine;

public class PhaseResultState : IExploringState
{
    public void Enter(LegStateManager stateManager)
    {
        Debug.Log("Enter ResultState");
    }

    public void Execute(LegStateManager stateManager)
    {
        ;
    }

    public void Exit(LegStateManager stateManager)
    {
        stateManager.NotifyRaidFinished();
        stateManager.pNextPhase();
        Debug.Log("Exit ResultState");
    }
}