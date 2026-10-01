using UnityEngine.InputSystem;

public class Pause : CharacterBase
{
    public void TogglePause(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        GameState.ToggleState();
    }
}