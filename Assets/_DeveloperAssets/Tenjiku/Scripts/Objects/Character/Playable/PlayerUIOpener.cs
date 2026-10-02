using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUIOpener : ObjectBase
{
    [SerializeField] GameObject _inventoryUI;
    bool _toggle = false;

    public void ToggleCraftUI(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        _toggle = !_toggle;
        _inventoryUI.SetActive(_toggle);
    }
}