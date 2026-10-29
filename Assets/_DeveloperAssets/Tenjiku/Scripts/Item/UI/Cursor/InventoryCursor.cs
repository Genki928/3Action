using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryCursor : MonoBehaviour
{
    // --- プロパティ ---
    public ItemSlot Mainhand => _mainhand;
    ItemSlot[] SlotList => _ui.ItemSlot;

    // --- フィールド ---
    [SerializeField] ItemSlot _baseSlot;
    [SerializeField] InventoryManager _ui;
    ItemSlot _currentSlot;
    ItemSlot _selectSlot;
    ItemSlot _mainhand;
    bool _active = true;
    int _currentSlotIndex = 0;

    // --- メソッド ---
    bool IsMoveCursor(InputAction.CallbackContext ctx) => _active && ctx.performed;

    void Start()
    {
        // 位置の計算
        _currentSlot = _baseSlot;
        _mainhand = _currentSlot;

        // 描画
        Draw();
    }

    public void CursorUp()
    {
        // 位置の計算
        _currentSlotIndex = (_currentSlotIndex - 5 + 15) % 15;
        _currentSlot = SlotList[_currentSlotIndex];

        // 描画
        Draw();
    }

    public void CursorDown()
    {
        // 位置の計算
        _currentSlotIndex = (_currentSlotIndex + 5) % 15;
        _currentSlot = SlotList[_currentSlotIndex];

        // 描画
        Draw();
    }

    public void CursorRight()
    {
        // 位置の計算
        _currentSlotIndex = (_currentSlotIndex / 5 * 5) + (_currentSlotIndex + 1) % 5;
        _currentSlot = SlotList[_currentSlotIndex];
        if (!_ui.IsOpenUI) _mainhand = _currentSlot;

        // 描画
        Draw();
    }

    public void CursorLeft()
    {
        // 位置の計算
        _currentSlotIndex = (_currentSlotIndex / 5 * 5) + (--_currentSlotIndex + 5) % 5;
        _currentSlot = SlotList[_currentSlotIndex];
        if (!_ui.IsOpenUI) _mainhand = _currentSlot;

        // 描画
        Draw();
    }

    public void ChangeDrawing(bool active)
    {
        _active = active;
        if (TryGetComponent<Image>(out var img))
            img.enabled = _active;

        if (_selectSlot != null)
        {
            _selectSlot.SlotDeselect();
            _selectSlot = null;
        }
    }

    void Draw()
    {
        transform.position = _currentSlot.transform.position;
    }

    public void Interact()
    {
        if (_selectSlot == null)
        {
            _selectSlot = _currentSlot;
            _selectSlot.SlotSelect();
        }
        else
        {
            // スロットの番号を調べて、データ側で入れ替える
            int from = Array.IndexOf(_ui.ItemSlot, _currentSlot);
            int to = Array.IndexOf(_ui.ItemSlot, _selectSlot);
            if (from != -1 && to != -1)
                _ui.Inventory.Swap(from, to);
            _selectSlot.SlotDeselect();
            _selectSlot = null;
        }
    }

    public void Throw()
    {
        if (_currentSlot.Stack.Item == null) return;
        if (_ui.Cauldron.IsMax) return;
        ItemType[] types = { ItemType.Tool, ItemType.Potion };
        if (types.Any(x => _currentSlot.Stack.Item.Type.Contains(x))) return;

        _ui.Cauldron.ReceiveItem(_currentSlot.Stack.Item);
        _ui.Inventory.RemoveItem(_currentSlotIndex, 1);
        _ui.Draw(_ui.Inventory);
    }

    void UIOpend()
    {
        _mainhand = _currentSlot;
    }

    void UIClosed()
    {
        // 位置の計算
        _currentSlotIndex %= 5;
        _currentSlot = SlotList[_currentSlotIndex];
        if (_selectSlot != null)
        {
            _selectSlot.SlotDeselect();
            _selectSlot = null;
        }
        _mainhand = _currentSlot;

        // 描画
        Draw();
    }

    void OnEnable()
    {
        _ui.OnOpenUI += UIOpend;
        _ui.OnCloseUI += UIClosed;
        _ui.OnToggleCategory += ChangeDrawing;
    }

    void OnDisable()
    {
        _ui.OnOpenUI -= UIOpend;
        _ui.OnCloseUI -= UIClosed;
        _ui.OnToggleCategory -= ChangeDrawing;
    }
}