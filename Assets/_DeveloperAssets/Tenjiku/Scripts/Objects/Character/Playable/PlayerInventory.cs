using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : ObjectBase
{
    // --- プロパティ --- //
    public bool IsOpenUI => _isOpen;
    public Cursor Cursor => _cursor;
    public Inventory Items => _inventory;

    // --- フィールド --- //
    [Header("▼ Inventory")]
    [SerializeField] List<ItemSlot> _itemSlot = new();
    [SerializeField] List<ItemStack> _startItem = new();
    Inventory _inventory = new();
    [SerializeField] Cursor _cursor;
    [SerializeField] InventoryUI[] _inventoryUI = new InventoryUI[2];
    bool _isOpen = false;

    // --- イベント --- //
    public event Action OnCloseCraftUI;

    // --- メソッド --- //
    void Start()
    {
        // 開始時のアイテムを付与
        foreach (var item in _startItem)
            _inventory.AddItem(item);

        Draw();
    }

    public void ToggleCraftUI(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        // 画面を切り替える
        _isOpen = !_isOpen;
        foreach (var toggle in _inventoryUI)
            foreach (var ui in toggle.inventory)
                ui.SetActive(!ui.activeSelf);

        // カーソルの位置が外れてたら修正
        if (!_isOpen) OnCloseCraftUI?.Invoke();
    }

    void Draw()
    {
        int hasItemCount = _inventory.Stack.Count, j = 0;
        for (int i = 0; i < _itemSlot.Count; i++)
        {
            if (j++ < hasItemCount)
            {
                _itemSlot[i].ChangeSprite(_inventory.Stack[i].Item.Sprite);
            }
            else
            {
                _itemSlot[i].ChangeSprite();
            }

        }
    }

    void OnEnable()
    {
        _inventory.OnGetItem += Draw;
    }

    /// <summary> List(List(T))ができないから作ったクラス "</summary>
    [Serializable] class InventoryUI { public List<GameObject> inventory; }
}