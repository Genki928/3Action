using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    // --- プロパティ --- //
    public bool IsOpenUI => _isOpen;
    public ItemSlot[] ItemSlot => _itemSlot;
    public Inventory Inventory => _player.Inventory;
    public InventoryCursor Cursor => _cursor.InventoryCursor;
    public Cauldron Cauldron => _cauldron;
    public bool IsInventory => _isInventory;
    public int MainhandIndexOfSlot => Array.FindIndex(_itemSlot, x => x == _cursor.InventoryCursor.Mainhand);
    public ItemStack MainhandItem => _cursor.InventoryCursor.Mainhand.Stack;

    // --- フィールド --- //
    [Header("▼ Inventory")]
    [SerializeField] PlayerController _player;
    [SerializeField] List<ItemStack> _startItem = new();
    [SerializeField] Cauldron _cauldron;

    [Header("▼ UI")]
    [SerializeField] CursorManager _cursor;
    [SerializeField] ItemSlot[] _itemSlot = new ItemSlot[15];
    [SerializeField] List<GameObject> _hotbarUI = new();
    [SerializeField] List<GameObject> _inventoryUI = new();
    bool _isOpen = false;
    bool _isInventory = true;

    // --- イベント --- //
    public event Action OnOpenUI;
    public event Action OnCloseUI;
    public event Action<bool> OnToggleCategory;

    // --- メソッド --- //
    void Start()
    {
        // 開始時のアイテムを付与
        foreach (var item in _startItem)
            _player.Inventory.AddItem(new(item.Item, item.Count));
    }

    /// <summary> インベントリのUIを開閉する </summary>
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
        if (!_isOpen) OnCloseUI?.Invoke();
        else OnOpenUI?.Invoke();

        if (!_isInventory) _isInventory = true;
    }

    public void CategoryChange(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || !_isOpen) return;

        _isInventory = !_isInventory;
        OnToggleCategory?.Invoke(_isInventory);
    }

    /// <summary> インベントリの描画 </summary>
    public void Draw(Inventory inventory)
    {
        // インベントリのスロットの数を取得
        int hasItemCount = _player.Inventory.Stack.Length;
        for (int i = 0; i < _itemSlot.Length; i++)
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

    public void Interact(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || !IsOpenUI) return;

        _cursor.Interact();
    }

    public void SubInteract(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || !IsOpenUI) return;

        _cursor.SubInteract();
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