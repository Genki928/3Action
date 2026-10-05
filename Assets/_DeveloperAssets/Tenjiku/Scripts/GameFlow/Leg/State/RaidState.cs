using UnityEngine;

public class RaidState : ExploringStateBase
{
    public override void Enter(LegStateManager stateManager)
    {
        stateManager.NotifyRaidStarted();
    }

    public override void Execute(LegStateManager stateManager)
    {
        // 時間を進めきったらフェーズを切り替える
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