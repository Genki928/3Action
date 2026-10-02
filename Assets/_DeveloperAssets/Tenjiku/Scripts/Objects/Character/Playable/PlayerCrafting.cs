using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerCrafting : ObjectBase
{
    [SerializeField] GameObject _craftUI;
    bool _toggle = false;

    public void ToggleCraftUI(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        _toggle = !_toggle;
        _craftUI.SetActive(_toggle);
    }
}