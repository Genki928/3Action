using UnityEngine;

public class ExploringState : IExploringState
{
    public void  Enter(LegStateManager stateManager)
    {
        Debug.Log("Enter ExploringState");
    }

    public void Execute(LegStateManager stateManager)
    {
        ;
    }

    public void Exit(LegStateManager stateManager)
    {
        Debug.Log("Exit RaidgState");
    }
}