using UnityEngine;

public class ExploringState : ExploringStateBase
{
    // --- プロパティ ---
    public override float Ratio => _dayCycleTimer == 0.0f ? 1.0f : _dayCycleTimer / _stateManager.ExploringTimerLimit;

    // --- メンバ ---
    LegStateManager _stateManager;

    public override void Enter(LegStateManager stateManager)
    {
        _stateManager = stateManager;
        Debug.Log(Ratio);
        stateManager.NotifyExploreStarted();
    }

    public override void Execute(LegStateManager stateManager)
    {
        Debug.Log(Ratio);
        // 時間を進めきったらフェーズを切り替える
        _dayCycleTimer += Time.deltaTime;
        if (_dayCycleTimer > stateManager.ExploringTimerLimit)
        {
            stateManager.NextState(stateManager);
        }
    }

    public override void Exit(LegStateManager stateManager)
    {
        ;
    }
}