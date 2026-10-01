using UnityEngine.InputSystem;

public class Pause : CharacterBase
{
    // --- ƒƒ\ƒbƒh ---
    public void TogglePause(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        GameState.ToggleState();
    }
}