using UnityEngine;

public class ExploringState : ExploringStateBase
{
    public override void Enter(LegStateManager stateManager)
    {
        stateManager.NotifyExploreStarted();
    }

    public override void Execute(LegStateManager stateManager)
    {
        // 時間を進めきったらフェーズを切り替える
        _dayCycleTimer += Time.deltaTime;
        if (_dayCycleTimer > stateManager._exploringTimerLimit)
        {
            stateManager.NextState(stateManager);
        }
    }

    public override void Exit(LegStateManager stateManager)
    {
        ;
    }
}