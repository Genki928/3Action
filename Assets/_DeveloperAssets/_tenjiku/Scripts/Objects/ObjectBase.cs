using System;
using UnityEngine;

public class ObjectBase : MonoBehaviour
{
    // --- フィールド ---
    protected bool _isPaused = false;

    // --- メソッド ---
    void OnEnable()
    {
        GameState.OnPaused += ChangedGameState;
    }

    void OnDisable()
    {
        GameState.OnPaused -= ChangedGameState;
    }

    protected void ChangedGameState()
    {
        _isPaused = GameState.IsPaused;
    }
}
public static class GameState
{
    // --- プロパティ ---
    public static bool IsPaused => _isPaused;

    // --- フィールド ---
    static bool _isPaused = false;

    // --- イベント ---
    public static event Action OnPaused;

    // --- メソッド ---
    public static void ToggleState()
    {
        _isPaused = !_isPaused;
        OnPaused?.Invoke();
    }
}