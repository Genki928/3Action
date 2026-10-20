using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    // --- プロパティ --- //
    public bool IsOpenUI => _isOpen;


    // --- フィールド --- //
    [Header("▼ Inventory")]
    [SerializeField] PlayerController _playerInventory;
    [SerializeField] List<ItemStack> _startItem = new();
    Inventory _inventory = new();

    [Header("▼ UI")]
    [SerializeField] Cursor _cursor;
    [SerializeField] List<ItemSlot> _itemSlot = new();
    [SerializeField] InventoryUI[] _inventoryUI = new InventoryUI[2];
    bool _isOpen = false;

    // --- イベント --- //
    public event Action OnCloseCraftUI;
    void Start()
    {
        // 開始時のアイテムを付与
        _inventory = _playerInventory.Inventory;
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
        Debug.Log("tes");
        int hasItemCount = _inventory.Stack.Count, j = 0;
        for (int i = 0; i < _itemSlot.Count; i++)
        {
            if (j++ < hasItemCount)
            {
                _itemSlot[i].GetItem(_inventory.Stack[i]);
            }
            else
            {
                _itemSlot[i].GetItem();
            }
        }
    }

    void OnEnable()
    {
        _inventory.OnGetItem += Draw;
    }

    void OnDisable()
    {
        _inventory.OnGetItem -= Draw;
    }
}