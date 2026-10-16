using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUIOpener : ObjectBase
{
    public bool IsOpenUI => _toggle;
    [SerializeField] List<GameObject> _inventoryUI;
    [SerializeField] List<GameObject> _toggleInventoryUI;
    bool _toggle = false;

    public void ToggleCraftUI(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        // âÊñ ÇêÿÇËë÷Ç¶ÇÈ
        _toggle = !_toggle;
        
        foreach (var ui in _inventoryUI)
            ui.SetActive(_toggle);
        foreach (var ui in _toggleInventoryUI)
            ui.SetActive(!_toggle);
    }
}