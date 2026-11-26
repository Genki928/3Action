using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCursor : CursorBase
{
    // --- プロパティ ---
    public ItemSlot Mainhand => _mainhand;
    ItemSlot[] SlotList => _ui.ItemSlot;

    // --- フィールド ---
    [SerializeField] ItemSlot _baseSlot;
    ItemSlot _currentSlot;
    ItemSlot _mainhand;
    int _currentSlotIndex = 0;


    void Start()
    {
        // 位置の計算
        _currentSlot = _baseSlot;
        _mainhand = _currentSlot;

        // 描画
        Draw();
    }

    public override void CursorUp()
    {
        if (!_ui.IsOpenUI) return;

        // 位置の計算
        _currentSlotIndex = (_currentSlotIndex - 5 + 15) % 15;
        _currentSlot = SlotList[_currentSlotIndex];

        // 描画
        Draw();
    }

    public override void CursorDown()
    {
        if (!_ui.IsOpenUI) return;

        // 位置の計算
        _currentSlotIndex = (_currentSlotIndex + 5) % 15;
        _currentSlot = SlotList[_currentSlotIndex];

        // 描画
        Draw();
    }

    public override void CursorRight()
    {
        // 位置の計算
        _currentSlotIndex = (_currentSlotIndex / 5 * 5) + (_currentSlotIndex + 1) % 5;
        _currentSlot = SlotList[_currentSlotIndex];
        if (!_ui.IsOpenUI) _mainhand = _currentSlot;

        // 描画
        Draw();
    }

    public override void CursorLeft()
    {
        // 位置の計算
        _currentSlotIndex = (_currentSlotIndex / 5 * 5) + (--_currentSlotIndex + 5) % 5;
        _currentSlot = SlotList[_currentSlotIndex];
        if (!_ui.IsOpenUI) _mainhand = _currentSlot;

        // 描画
        Draw();
    }

    void Draw()
    {
        transform.position = _currentSlot.transform.position;
    }

    public override void Interact()
    {
        // 選択中のスロットがなければ、選択
        if (_selectSlot == null)
        {
            _selectSlot = _currentSlot;
            _selectSlot.SlotSelect();
        }

        // 選択中のスロットがあれば、位置を入れ替える
        else
        {
            // スロットの番号を調べて、データ側で入れ替える
            int from = Array.IndexOf(_ui.ItemSlot, _currentSlot);
            int to = Array.IndexOf(_ui.ItemSlot, _selectSlot);
            if (from != -1 && to != -1)
                _ui.Inventory.Swap(from, to);

            // 選択を解除
            _selectSlot.SlotDeselect();
            _selectSlot = null;
        }
    }

    public void Throw()
    {
        if (_currentSlot.Stack.Item == null || _ui.Cauldron.IsMax) return;

        // 窯に入れられないアイテムなら中断
        if (!_currentSlot.Stack.Item.CanMix) return;

        // アイテムを投入->持ち物から削除
        _ui.Cauldron.ReceiveItem(_currentSlot.Stack.Item);
        _ui.Inventory.RemoveItem(_currentSlotIndex, 1);

        // 描画
        _ui.Draw(_ui.Inventory);
    }

    void UIOpend()
    {
        _mainhand = _currentSlot;
    }

    protected override void UIClosed()
    {
        // 位置の計算
        _currentSlotIndex %= 5;
        _currentSlot = SlotList[_currentSlotIndex];
        _mainhand = _currentSlot;

        // 選択中のスロットがあるなら、選択解除
        if (_selectSlot != null)
        {
            _selectSlot.SlotDeselect();
            _selectSlot = null;
        }

        if (TryGetComponent<Image>(out var img))
            img.enabled = true;

        // 描画
        Draw();
    }

    public override void CategoryChange(bool active)
    {
        base.CategoryChange(active);

        // 選択中のスロットがあるなら、選択解除
        if (_selectSlot != null)
        {
            _selectSlot.SlotDeselect();
            _selectSlot = null;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _ui.OnOpenUI += UIOpend;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _ui.OnOpenUI -= UIOpend;
    }
}