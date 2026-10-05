using UnityEngine;

public class RaidState : ExploringStateBase
{
    public override void Enter(LegStateManager stateManager)
    {
        Debug.Log("Enter RaidgState");
    }

    public override void Execute(LegStateManager stateManager)
    {
        _dayCycleTimer += Time.deltaTime;
        if (_dayCycleTimer > stateManager._raidTimerLimit)
        {
            stateManager.NextState(stateManager);
        }
    }

    public override void Exit(LegStateManager stateManager)
    {
        Debug.Log("Exit RaidState");
    }
}