using UnityEngine;

public class ExploringState : ExploringStateBase
{
    public override void Enter(LegStateManager stateManager)
    {
        Debug.Log("Enter ExploringState");
    }

    public override void Execute(LegStateManager stateManager)
    {
        _dayCycleTimer += Time.deltaTime;
        if (_dayCycleTimer > stateManager._exploringTimerLimit)
        {
            stateManager.NextState(stateManager);
        }
    }

    public override void Exit(LegStateManager stateManager)
    {
        Debug.Log("Exit RaidgState");
    }
}