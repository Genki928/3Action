using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Cursor : MonoBehaviour
{
    // --- プロパティ ---
    public ItemSlot SelectingSlot => _currentSlot;
    ItemSlot[] SlotList => _ui.ItemSlot;

    // --- フィールド ---
    [SerializeField] ItemSlot _baseSlot;
    [SerializeField] InventoryManager _ui;
    ItemSlot _currentSlot;
    ItemSlot _selectSlot;
    bool _active = true;
    int _currentSlotIndex = 0;

    bool IsMoveCursor(InputAction.CallbackContext ctx) => _active && ctx.performed;

    void Start()
    {
        // 位置の計算
        _currentSlot = _baseSlot;

        // 描画
        Draw();
    }

    public void CursorUp(InputAction.CallbackContext ctx)
    {
        if (!IsMoveCursor(ctx)) return;
        if (!_ui.IsOpenUI) return;

        // 位置の計算
        _currentSlotIndex = (_currentSlotIndex - 5 + 15) % 15;
        _currentSlot = SlotList[_currentSlotIndex];

        // 描画
        Draw();
    }

    public void CursorDown(InputAction.CallbackContext ctx)
    {
        if (!IsMoveCursor(ctx)) return;
        if (!_ui.IsOpenUI) return;

        // 位置の計算
        _currentSlotIndex = (_currentSlotIndex + 5) % 15;
        _currentSlot = SlotList[_currentSlotIndex];

        // 描画
        Draw();
    }

    public void CursorRight(InputAction.CallbackContext ctx)
    {
        if (!IsMoveCursor(ctx)) return;

        // 位置の計算
        _currentSlotIndex = (_currentSlotIndex / 5 * 5) + (_currentSlotIndex + 1) % 5;
        _currentSlot = SlotList[_currentSlotIndex];

        // 描画
        Draw();
    }

    public void CursorLeft(InputAction.CallbackContext ctx)
    {
        if (!IsMoveCursor(ctx)) return;

        // 位置の計算
        _currentSlotIndex = (_currentSlotIndex / 5 * 5) + (--_currentSlotIndex + 5) % 5;
        _currentSlot = SlotList[_currentSlotIndex];

        // 描画
        Draw();
    }

    public void ChangeDrawing(bool active)
    {
        _active = active;
        if (TryGetComponent<Image>(out var img))
            img.enabled = _active;
    }

    void Draw()
    {
        transform.position = _currentSlot.transform.position;
    }

    public void Interact(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (_selectSlot == null)
        {
            _selectSlot = _currentSlot;
        }
        else
        {
            // スロットの番号を調べて、データ側で入れ替える
            int from = Array.IndexOf(_ui.ItemSlot, _currentSlot);
            int to = Array.IndexOf(_ui.ItemSlot, _selectSlot);
            if (from != -1 && to != -1)
                _ui.Inventory.Swap(from, to);
            _selectSlot = null;
        }
    }

    void CursorCorrection()
    {
        // 位置の計算
        _currentSlotIndex %= 5;
        _currentSlot = SlotList[_currentSlotIndex];

        // 描画
        Draw();
    }

    void OnEnable()
    {
        _ui.OnCloseCraftUI += CursorCorrection;
        _ui.OnToggleCategory += ChangeDrawing;
    }

    void OnDisable()
    {
        _ui.OnCloseCraftUI -= CursorCorrection;
        _ui.OnToggleCategory -= ChangeDrawing;
    }
}