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
    [SerializeField] PlayerController _player;
    [SerializeField] List<ItemStack> _startItem = new();

    [Header("▼ UI")]
    [SerializeField] Cursor _cursor;
    [SerializeField] List<ItemSlot> _itemSlot = new();
    [SerializeField] List<GameObject> _hotbarUI = new();
    [SerializeField] List<GameObject> _inventoryUI = new();
    bool _isOpen = false;
    bool _isInventory = true;

    // --- イベント --- //
    public event Action OnCloseCraftUI;
    public event Action<bool> OnToggleCategory;

    // --- メソッド --- //
    void Start()
    {
        // 開始時のアイテムを付与
        foreach (var item in _startItem)
            _player.Inventory.AddItem(item);
    }

    /// <summary> インベントリのUIを開閉する </summary>
    /// <param name="ctx"></param>
    public void ToggleCraftUI(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        // 画面を切り替える
        _isOpen = !_isOpen;
        foreach (var hotbar in _hotbarUI)
            hotbar.SetActive(_isOpen);
        foreach (var inventory in _inventoryUI)
            inventory.SetActive(!_isOpen);

        // カーソルの位置が外れてたら修正
        if (!_isOpen) OnCloseCraftUI?.Invoke();
        if (!_isInventory) _isInventory = true;
    }

    public void CategoryChange(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (!_isOpen) return;

        _isInventory = !_isInventory;
        OnToggleCategory?.Invoke(_isInventory);
    }

    /// <summary> インベントリの描画 </summary>
    void Draw(Inventory inventory)
    {
        // インベントリのスロットの数を取得
        int hasItemCount = _player.Inventory.Stack.Count;
        for (int i = 0; i < _itemSlot.Count; i++)
        {
            // そのスロットがアイテムを持つなら、
            if (i < hasItemCount)
            {
                // アイテムを描画
                _itemSlot[i].GetItem(_player.Inventory.Stack[i]);
            }
            else
            {
                // 描画をリセット
                _itemSlot[i].GetItem();
            }
        }
    }

    void OnEnable()
    {
        _player.Inventory.OnGetItem += Draw;
    }

    void OnDisable()
    {
        _player.Inventory.OnGetItem -= Draw;
    }
}

public enum InventoryCategory
{
    Inventory,
    Craft,
}