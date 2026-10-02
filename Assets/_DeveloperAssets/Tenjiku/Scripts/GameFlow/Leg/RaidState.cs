using UnityEngine;

public class RaidState : IExploringState
{
    public void  Enter()
    {
        Debug.Log("Enter RaidgState");
    }

    public void Execute()
    {
        ;
    }

    public void Exit()
    {
        Debug.Log("Exit RaidgState");
    }
}