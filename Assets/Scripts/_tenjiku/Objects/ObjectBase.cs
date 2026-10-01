using System;
using UnityEngine;

public class ObjectBase : MonoBehaviour
{
    protected bool isPaused = false;

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
        isPaused = GameState.IsPaused;
    }
}

public static class GameState
{
    public static bool IsPaused => isPaused;
    static bool isPaused = false;
    public static event Action OnPaused;
    public static void ToggleState()
    {
        isPaused = !isPaused;
        OnPaused?.Invoke();
    }
}