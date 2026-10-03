using UnityEngine;

public class RaidState : IExploringState
{
    public void  Enter(LegStateManager stateManager)
    {
        Debug.Log("Enter RaidgState");
    }

    public void Execute(LegStateManager stateManager)
    {
        ;
    }

    public void Exit(LegStateManager stateManager)
    {
        Debug.Log("Exit RaidState");
    }
}