using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUIOpener : ObjectBase
{
    // --- プロパティ --- //
    public bool IsOpenUI => _isOpen;
    public Cursor Cursor => _cursor;

    // --- フィールド --- //
    [SerializeField] InventoryUI[] _inventory = new InventoryUI[2];
    [SerializeField] Cursor _cursor;
    bool _isOpen = false;

    // --- イベント --- //
    public event Action OnCloseCraftUI;

    // --- メソッド --- //
    public void ToggleCraftUI(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        // 画面を切り替える
        _isOpen = !_isOpen;
        foreach (var toggle in _inventory)
            foreach (var ui in toggle.inventory)
                ui.SetActive(!ui.activeSelf);

        // カーソルの位置が外れてたら修正
        if (!_isOpen) OnCloseCraftUI?.Invoke();
    }
    [Serializable] class InventoryUI { public List<GameObject> inventory; }
}