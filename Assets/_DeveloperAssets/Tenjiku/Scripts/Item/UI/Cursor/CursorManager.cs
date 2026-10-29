//using System;
//using UnityEngine;
//using UnityEngine.InputSystem;
//using UnityEngine.UI;

//public class CursorManager : MonoBehaviour
//{
//    // --- プロパティ ---
//    ItemSlot[] SlotList => _ui.ItemSlot;

//    // --- フィールド ---
//    [SerializeField] InventoryManager _ui;
//    [SerializeField] InventoryCursor _invCursor;
//    bool _active = true;

//    // --- メソッド ---

//    void Start()
//    {
//        // 描画
//        Draw();
//    }

//    public void CursorUp(InputAction.CallbackContext ctx)
//    {
//        if (!_ui.IsOpenUI) return;

//        // 位置の計算
//        _currentSlotIndex = (_currentSlotIndex - 5 + 15) % 15;
//        _currentSlot = SlotList[_currentSlotIndex];

//        // 描画
//        Draw();
//    }

//    public void CursorDown(InputAction.CallbackContext ctx)
//    {
//        if (!IsMoveCursor(ctx)) return;
//        if (!_ui.IsOpenUI) return;

//        // 位置の計算
//        _currentSlotIndex = (_currentSlotIndex + 5) % 15;
//        _currentSlot = SlotList[_currentSlotIndex];

//        // 描画
//        Draw();
//    }

//    public void CursorRight(InputAction.CallbackContext ctx)
//    {
//        if (!IsMoveCursor(ctx)) return;

//        // 位置の計算
//        _currentSlotIndex = (_currentSlotIndex / 5 * 5) + (_currentSlotIndex + 1) % 5;
//        _currentSlot = SlotList[_currentSlotIndex];
//        if (!_ui.IsOpenUI) _mainhand = _currentSlot;

//        // 描画
//        Draw();
//    }

//    public void CursorLeft(InputAction.CallbackContext ctx)
//    {
//        if (!IsMoveCursor(ctx)) return;

//        // 位置の計算
//        _currentSlotIndex = (_currentSlotIndex / 5 * 5) + (--_currentSlotIndex + 5) % 5;
//        _currentSlot = SlotList[_currentSlotIndex];
//        if (!_ui.IsOpenUI) _mainhand = _currentSlot;

//        // 描画
//        Draw();
//    }

//    public void ChangeDrawing(bool active)
//    {
//        _active = active;
//        if (TryGetComponent<Image>(out var img))
//            img.enabled = _active;
//    }

//    void Draw()
//    {
//        transform.position = _currentSlot.transform.position;
//        //Debug.Log(Mainhand.Stack.Item.Name);
//    }

//    public void Interact(InputAction.CallbackContext ctx)
//    {
//        if (!ctx.performed) return;
//        if (!_ui.IsOpenUI) return;

//        if (_selectSlot == null)
//        {
//            _selectSlot = _currentSlot;
//            _selectSlot.SlotSelect();
//        }
//        else
//        {
//            // スロットの番号を調べて、データ側で入れ替える
//            int from = Array.IndexOf(_ui.ItemSlot, _currentSlot);
//            int to = Array.IndexOf(_ui.ItemSlot, _selectSlot);
//            if (from != -1 && to != -1)
//                _ui.Inventory.Swap(from, to);
//            _selectSlot.SlotDeselect();
//            _selectSlot = null;
//        }
//    }

//    void UIOpend()
//    {
//        _mainhand = _currentSlot;
//    }

//    void UIClosed()
//    {
//        // 位置の計算
//        _currentSlotIndex %= 5;
//        _currentSlot = SlotList[_currentSlotIndex];
//        if (_selectSlot != null)
//        {
//            _selectSlot.SlotDeselect();
//            _selectSlot = null;
//        }
//        _mainhand = _currentSlot;

//        // 描画
//        Draw();
//    }

//    void OnEnable()
//    {
//        _ui.OnOpenUI += UIOpend;
//        _ui.OnCloseUI += UIClosed;
//        _ui.OnToggleCategory += ChangeDrawing;
//    }

//    void OnDisable()
//    {
//        _ui.OnOpenUI -= UIOpend;
//        _ui.OnCloseUI -= UIClosed;
//        _ui.OnToggleCategory -= ChangeDrawing;
//    }
//}