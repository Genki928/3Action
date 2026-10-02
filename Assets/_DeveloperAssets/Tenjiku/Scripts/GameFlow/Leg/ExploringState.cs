using UnityEngine;

public class ExploringState : IExploringState
{
    public void  Enter()
    {
        Debug.Log("Enter ExploringState");
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